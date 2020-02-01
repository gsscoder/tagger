using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CSharpx;

namespace Tagger
{
    public sealed class Mirror
    {
        Metadata _metadata = new Metadata();
        bool _built;
        object _object;
        
        public Mirror() { }

        public Mirror(object template)
        {
            Guard.AgainstNull(nameof(template), template);

            _metadata = new Metadata(
                Maybe.Just(template),
                new Dictionary<string, IEnumerable<AttributeMeta>>(),
                from p in template.GetType().GetProperties()
                select new PropertyMeta(p.Name, p.PropertyType),
                Enumerable.Empty<Type>());
        }

        Mirror(Metadata metadata)
        {
            _metadata = metadata;
        }

        public Mirror Implement<T>()
        {
            Guard.AgainstExceptInterface<T>();

            return new Mirror(
                _metadata.WithInterfaces(
                    _metadata.Interfaces.Concat(new[] { typeof(T) })));
        }

        public Mirror Add(Action<AttributeBinder> binder)
        {
            Guard.AgainstNull(nameof(binder), binder);

            var config = new AttributeBinder();
            binder(config);
            var meta = config.ToMetadata();

            if (_metadata.Attributes.ContainsKey(meta.PropertyName)) {
                _metadata.Attributes[meta.PropertyName] = _metadata.Attributes[meta.PropertyName].Concat(new[] { meta });
            }
            else {
                _metadata.Attributes.Add(meta.PropertyName, new[] { meta });
            }
            return new Mirror(_metadata);
        }

        public Mirror Add(Action<PropertyBinder> binder)
        {
            Guard.AgainstNull(nameof(binder), binder);

            var config = new PropertyBinder();
            binder(config);

            return new Mirror(
                _metadata.WithProperties(
                    _metadata.Properties.Concat(new[] { config.ToMetadata() })));
        }

        public T Unwrap<T>()
            where T : class
        {
            return (T)Object;
        }

        public object Object
        {
            get
            {
                if (_built) {
                    return _object;
                }
                lock (this) {
                    var typeName = _metadata.Template.Return(t => t.GetType().Name, GenerateTypeName());
                    _object = BuildObject(typeName);
                    _built = true;
                    return _object;
                }
            }
        }

        object BuildObject(string typeName)
        {
            var name = new AssemblyName("_Tagger.Dynamic");
            var builder = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            var moduleBuilder = builder.DefineDynamicModule(name.Name);
            var typeBuilder = moduleBuilder.DefineType(
                string.Concat("_", typeName, "Mirror"), TypeAttributes.Public);

            _metadata.Interfaces.ForEach(@interface => typeBuilder.AddInterfaceImplementation(@interface));

            var props = (from @interface in _metadata.Interfaces
                         select @interface.GetProperties(
                             from meta in _metadata.Properties
                             select meta.Name)).SelectMany(p => p).Memoize();
            _metadata = _metadata.WithProperties(_metadata.Properties.Concat(props));

            foreach(var prop in _metadata.Properties) {
                var propBuilder = typeBuilder.BuildProperty(prop.Name, prop.Type, _metadata.Interfaces);

                if (!_metadata.Attributes.Keys.Contains(prop.Name)) continue;

                var attrInfos = _metadata.Attributes[prop.Name];
                attrInfos.ForEach(info =>
                    {
                        var ctorTypes = info.CtorLayout == null 
                                        ? new Type[] {}
                                        : (from p in info.CtorLayout.GetType().GetProperties()
                                           select p.PropertyType).ToArray();
                        var ctorInfo = info.AttributeType.GetConstructor(ctorTypes);
                        if (!ctorInfo.ValidateNames(info.CtorLayout)) {
                            throw new ArgumentException("Anonymous type doesn't match constructor parameters names.");
                        }
                        var ctorValues = info.CtorLayout == null 
                                         ? new object[] {}
                                         : (from p in info.CtorLayout.GetType().GetProperties()
                                          select p.GetValue(info.CtorLayout)).ToArray();

                        if (!info.PropertyValues.Any()) {
                            propBuilder.SetCustomAttribute(
                                new CustomAttributeBuilder(ctorInfo, ctorValues));
                        }
                        else {
                            var propWithValues =
                                from pi in info.AttributeType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                join pv in info.PropertyValues on pi.Name equals pv.Key
                                select new { PropInfo = pi, PropValue = pv.Value };
                            propBuilder.SetCustomAttribute(
                                new CustomAttributeBuilder(
                                    ctorInfo,
                                    ctorValues,
                                    (from p in propWithValues select p.PropInfo).ToArray(),
                                    (from v in propWithValues select v.PropValue).ToArray()));
                        }
                    });
            }

            var newType = typeBuilder.CreateTypeInfo();
            var instance = Activator.CreateInstance(newType);
            return instance;
        }

        static string GenerateTypeName()
        {
            return new Guid().ToString();
        }
    }
}
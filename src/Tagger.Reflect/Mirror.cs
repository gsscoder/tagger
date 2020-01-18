using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CSharpx;

namespace Tagger.Reflect
{
    public sealed class Mirror
    {
        private readonly Maybe<object> _template;
        private readonly IDictionary<string, IEnumerable<AttributeInfo>> _attributes;
        private readonly IEnumerable<Tagger.Reflect.PropertyInfo> _properties;
        private readonly IEnumerable<Type> _interfaces; 
        private bool built;
        private object newObject;
        
        public Mirror()
        {
            _template = Maybe.Nothing<object>();
            _attributes = new Dictionary<string, IEnumerable<AttributeInfo>>();
            _properties = Enumerable.Empty<Tagger.Reflect.PropertyInfo>();
            _interfaces = Enumerable.Empty<Type>();
        }

        public Mirror(object template)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));

            _template = Maybe.Just(template);
            _attributes = new Dictionary<string, IEnumerable<AttributeInfo>>();
            _properties = from p in template.GetType().GetProperties()
                              select new Tagger.Reflect.PropertyInfo(p.Name, p.PropertyType);
            _interfaces = Enumerable.Empty<Type>();
        }

        private Mirror(
            Maybe<object> template,
            IDictionary<string, IEnumerable<AttributeInfo>> attributes,
            IEnumerable<Tagger.Reflect.PropertyInfo> properties,
            IEnumerable<Type> interfaces)
        {
            _template = template;
            _attributes = attributes;
            _properties = properties;
            _interfaces = interfaces;
        }

        public Mirror AddAttribute<T>(string propertyName, AttributeConfiguration configuration)
            where T : Attribute
        {
            var info = new AttributeInfo(typeof(T), configuration);

            IEnumerable<AttributeInfo> infos;
            if (_attributes.ContainsKey(propertyName)) {
                infos = _attributes[propertyName].Concat(new[] { info });
            }
            else {
                _attributes.Add(propertyName, new[] { info });
            }

            var attrs = new Dictionary<string, IEnumerable<AttributeInfo>>(_attributes);
            return new Mirror(_template, attrs, _properties, _interfaces);
        }

        public Mirror Implement<T>()
            where T : class
        {
            return new Mirror(_template, _attributes, _properties,
                _interfaces.Concat(new[] { typeof(T) }));
        }

        public Mirror AddProperty(string name, Type type)
        {
            var property = new Tagger.Reflect.PropertyInfo(name, type);
            return new Mirror(_template, _attributes, _properties.Concat(new [] { property }), _interfaces);
        }

        public object Object
        {
            get
            {
                if (built) {
                    return newObject;
                }
                var typeName = _template.Return(t => t.GetType().Name, GenerateTypeName());
                newObject = BuildObject(typeName);
                built = true;
                return newObject;
            }
        }

        public T Unwrap<T>()
            where T : class
        {
            return (T)Object;
        }

        private object BuildObject(string typeName)
        {
            var name = new AssemblyName("_Tagger.Dynamic");
            var builder = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            var moduleBuilder = builder.DefineDynamicModule(name.Name);
            var typeBuilder = moduleBuilder.DefineType(
                string.Concat("_", typeName, "Mirror"), TypeAttributes.Public);

            _interfaces.ForEach(@interface => typeBuilder.AddInterfaceImplementation(@interface));

            foreach(var prop in _properties) {
                var propBuilder = typeBuilder.BuildProperty(prop.Name, prop.Type, _interfaces);

                if (!_attributes.Keys.Contains(prop.Name)) continue;

                var attrInfos = _attributes[prop.Name];
                attrInfos.ForEach(info =>
                    {
                        var ctorTypes = (from type in info.CtorParameterValues
                                         select type.GetType()).ToArray();
                        var ctorInfo = info.AttributeType.GetConstructor(ctorTypes);

                        if (!info.PropertyValues.Any()) {
                            propBuilder.SetCustomAttribute(
                                new CustomAttributeBuilder(ctorInfo, info.CtorParameterValues.ToArray()));
                        }
                        else {
                            var propWithValues =
                                from pi in info.AttributeType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                join pv in info.PropertyValues on pi.Name equals pv.Key
                                select new { PropInfo = pi, PropValue = pv.Value };
                            propBuilder.SetCustomAttribute(
                                new CustomAttributeBuilder(
                                    ctorInfo,
                                    info.CtorParameterValues.ToArray(),
                                    (from p in propWithValues select p.PropInfo).ToArray(),
                                    (from v in propWithValues select v.PropValue).ToArray()));
                        }
                    });
            }

            var newType = typeBuilder.CreateTypeInfo();
            var instance = Activator.CreateInstance(newType);
            return instance;
        }

        private static string GenerateTypeName()
        {
            return new Guid().ToString();
        }
    }
}
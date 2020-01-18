using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CSharpx;

public sealed class Mirror
{
    private readonly Metadata _metadata = new Metadata();
    private bool _built;
    private object _object;
    
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

    private Mirror(Metadata metadata)
    {
        _metadata = metadata;
    }

    public Mirror Implement<T>()
        where T : class
    {
        return new Mirror(
            _metadata.WithInterfaces(
                _metadata.Interfaces.Concat(new[] { typeof(T) })));
    }

    public Mirror Add(Action<AttributeBinder> binder)
    {
        var config = new AttributeBinder();
        binder(config);
        var meta = config.ToAttributeMeta();

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
        var config = new PropertyBinder();
        binder(config);

        return new Mirror(
            _metadata.WithProperties(
                _metadata.Properties.Concat(new[] { config.ToPropertyMeta() })));
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

        foreach(var prop in _metadata.Properties) {
            var propBuilder = typeBuilder.BuildProperty(prop.Name, prop.Type, _metadata.Interfaces);

            if (!_metadata.Attributes.Keys.Contains(prop.Name)) continue;

            var attrInfos = _metadata.Attributes[prop.Name];
            attrInfos.ForEach(info =>
                {
                    var ctorTypes = (from type in info.CtorParameters
                                        select type.GetType()).ToArray();
                    var ctorInfo = info.AttributeType.GetConstructor(ctorTypes);

                    if (!info.PropertyValues.Any()) {
                        propBuilder.SetCustomAttribute(
                            new CustomAttributeBuilder(ctorInfo, info.CtorParameters.ToArray()));
                    }
                    else {
                        var propWithValues =
                            from pi in info.AttributeType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            join pv in info.PropertyValues on pi.Name equals pv.Key
                            select new { PropInfo = pi, PropValue = pv.Value };
                        propBuilder.SetCustomAttribute(
                            new CustomAttributeBuilder(
                                ctorInfo,
                                info.CtorParameters.ToArray(),
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
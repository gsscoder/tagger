// Copyright 2015 Giacomo Stelluti Scala. All rights reserved. See doc/License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Tagger.Infrastructure;

namespace Tagger
{
    public sealed class Mirror
    {
        private readonly Maybe<object> template;
        private readonly IDictionary<string, IEnumerable<AttributeInfo>> attributes;
        private readonly IEnumerable<Tagger.PropertyInfo> properties;
        private readonly IEnumerable<Type> interfaces; 
        private bool built;
        private object newObject;
        
        public Mirror()
        {
            this.template = Maybe.Nothing<object>();
            this.attributes = new Dictionary<string, IEnumerable<AttributeInfo>>();
            this.properties = Enumerable.Empty<Tagger.PropertyInfo>();
            this.interfaces = Enumerable.Empty<Type>();
        }

        public Mirror(object template)
        {
            if (template == null) throw new ArgumentNullException("template");

            this.template = Maybe.Just(template);
            this.attributes = new Dictionary<string, IEnumerable<AttributeInfo>>();
            this.properties = from p in template.GetType().GetProperties()
                              select new Tagger.PropertyInfo(p.Name, p.PropertyType);
            this.interfaces = Enumerable.Empty<Type>();
        }

        private Mirror(
            Maybe<object> template,
            IDictionary<string, IEnumerable<AttributeInfo>> attributes,
            IEnumerable<Tagger.PropertyInfo> properties,
            IEnumerable<Type> interfaces)
        {
            this.template = template;
            this.attributes = attributes;
            this.properties = properties;
            this.interfaces = interfaces;
        }

        public Mirror AddAttribute<T>(string propertyName, Action<AttributeConfiguration> configurator)
            where T : Attribute
        {
            var attrConfig = new AttributeConfiguration();
            configurator(attrConfig);
            var info = new AttributeInfo(typeof(T), attrConfig);

            IEnumerable<AttributeInfo> infos;
            if (attributes.ContainsKey(propertyName))
            {
                infos = attributes[propertyName].Concat(new[] { info });
            }
            else
            {
                attributes.Add(propertyName, new[] { info });
            }

            var attrs = new Dictionary<string, IEnumerable<AttributeInfo>>(attributes);
            return new Mirror(this.template, attrs, this.properties, this.interfaces);
        }

        public Mirror Implement<T>()
            where T : class
        {
            return new Mirror(this.template, this.attributes, this.properties,
                this.interfaces.Concat(new[] { typeof(T) }));
        }

        public Mirror AddProperty(string name, Type type)
        {
            var property = new Tagger.PropertyInfo(name, type);
            return new Mirror(this.template, this.attributes, this.properties.Concat(new [] { property }), this.interfaces);
        }

        public object Object
        {
            get
            {
                if (built)
                {
                    return newObject;
                }
                var typeName = template.Return(t => t.GetType().Name, GenerateTypeName());
                newObject = BuildObject(typeName);
                built = true;
                return newObject;
            }
        }

        public T Unwrap<T>()
            where T : class
        {
            return (T)this.Object;
        }

        private object BuildObject(string typeName)
        {
            var name = new AssemblyName("_Tagger.Dynamic");
            var builder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            var moduleBuilder = builder.DefineDynamicModule(name.Name);
            var typeBuilder = moduleBuilder.DefineType(
                string.Concat("_", typeName, "Mirror"), TypeAttributes.Public);

            this.interfaces.ForEach(@interface => typeBuilder.AddInterfaceImplementation(@interface));

            foreach (var prop in properties)
            {
                var propBuilder = typeBuilder.BuildProperty(prop.Name, prop.Type, this.interfaces);

                if (!attributes.Keys.Contains(prop.Name)) continue;

                var attrInfos = attributes[prop.Name];
                foreach (var info in attrInfos)
                {
                    var ctorTypes = info.CtorParameterValues.Select(v => v.GetType()).ToArray();
                    var ctorInfo = info.AttributeType.GetConstructor(ctorTypes);

                    if (!info.PropertyValues.Any())
                    {
                        propBuilder.SetCustomAttribute(new CustomAttributeBuilder(ctorInfo, info.CtorParameterValues));
                    }
                    else
                    {
                        var propWithValues =
                            from pi in info.AttributeType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            join pv in info.PropertyValues on pi.Name equals pv.Key
                            select new { PropInfo = pi, PropValue = pv.Value };
                        propBuilder.SetCustomAttribute(
                            new CustomAttributeBuilder(ctorInfo, info.CtorParameterValues,
                                (from p in propWithValues select p.PropInfo).ToArray(),
                                (from v in propWithValues select v.PropValue).ToArray()));
                    }
                }
            }
            var newType = typeBuilder.CreateType();
            var instance = Activator.CreateInstance(newType);
            return instance;
        }

        private static string GenerateTypeName()
        {
            return new Guid().ToString();
        }
    }
}

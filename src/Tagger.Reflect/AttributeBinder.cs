using System;
using System.Linq;
using System.Collections.Generic;

namespace Tagger
{
    public sealed class AttributeBinder
    {
        public AttributeBinder()
        {
            PropertyLayouts = new List<object>();
        }

        public AttributeBinder ForProperty(string name)
        {
            Guard.AgainstNull(nameof(name), name);
            Guard.AgainstEmptyWhiteSpace(nameof(name), name);

            PropertyName = name;
            return this;
        }

        public AttributeBinder Define<T>()
            where T : Attribute
        {
            if (PropertyName == null) throw new InvalidOperationException();

            Type = typeof(T);
            return this;
        }

        public AttributeBinder AttributeCtor(object paramsAsAnonymous)
        {
            Guard.AgainstExceptAnonymous("paramsAsAnonymous", paramsAsAnonymous);

            CtorLayout = paramsAsAnonymous;
            return this;
        }

        public AttributeBinder AttributeProperty(object propAsAnonymous)
        {
            Guard.AgainstExceptAnonymous("paramsAsAnonymous", propAsAnonymous);
            Guard.AgainstExceptSingleProperty("paramsAsAnonymous", propAsAnonymous);

            if (PropertyName == null) throw new InvalidOperationException();

            PropertyLayouts.Add(propAsAnonymous);

            return this;
        }

        internal string PropertyName { get; private set; }

        internal Type Type { get; private set; }

        internal object CtorLayout { get; private set; }

        internal List<object> PropertyLayouts { get; private set; }

        internal AttributeMeta ToAttributeMeta()
        {
            if (PropertyName == null || Type == null) throw new InvalidOperationException();

            return new AttributeMeta(
                PropertyName,
                Type,
                CtorLayout,
                LayoutsToDictionary(PropertyLayouts));
        }

        static IDictionary<string, object> LayoutsToDictionary(IEnumerable<object> propLayouts)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var dynamic in propLayouts) {
                var prop = dynamic.GetType().GetProperties().Single();
                dictionary.Add(prop.Name, prop.GetValue(dynamic));
            }
            return dictionary;
        }
    }
}
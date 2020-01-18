using System;

namespace Tagger.Reflect
{
    sealed class PropertyInfo
    {
        private readonly string _name;
        private readonly Type _type;

        public PropertyInfo(string name, Type type)
        {
            this._name = name;
            this._type = type;
        }

        public string Name
        {
            get { return _name; }
        }

        public Type Type
        {
            get { return _type; }
        }
    }
}
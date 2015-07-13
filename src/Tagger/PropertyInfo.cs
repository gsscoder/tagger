// Copyright 2015 Giacomo Stelluti Scala. All rights reserved. See doc/License.md in the project root for license information.

using System;

namespace Tagger
{
    sealed class PropertyInfo
    {
        private readonly string name;
        private readonly Type type;

        public PropertyInfo(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }

        public string Name
        {
            get { return name; }
        }

        public Type Type
        {
            get { return type; }
        }
    }
}

// Copyright 2015 Giacomo Stelluti Scala. All rights reserved. See doc/License.md in the project root for license information.

namespace Tagger.Infrastructure
{
    using System;
    using System.Collections.Generic;

    static partial class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var element in source)
            {
                action(element);
            }
        }
    }
}

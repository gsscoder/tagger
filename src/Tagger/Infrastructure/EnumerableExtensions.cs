using System;
using System.Collections.Generic;

namespace Tagger.Infrastructure
{
    static partial class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var element in source) {
                action(element);
            }
        }
    }
}
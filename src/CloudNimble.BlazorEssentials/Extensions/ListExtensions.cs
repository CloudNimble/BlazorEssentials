using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudNimble.BlazorEssentials.Extensions
{
    public static class ListExtensions
    {

        /// <summary>
        /// Return item and all children recursively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="childSelector"></param>
        /// <returns></returns>
        /// <remarks>https://stackoverflow.com/a/32655815/403765</remarks>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }


    }
}

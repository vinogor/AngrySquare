using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.Utility
{
    public static class ListExtension
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> origin)
            => origin.OrderBy(_ => Random.value);
    }
}
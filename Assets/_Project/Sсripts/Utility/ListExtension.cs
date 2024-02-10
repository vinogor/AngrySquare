using System;
using System.Collections.Generic;

namespace Sсripts.Utility
{
    public static class ListExtension
    {
        private static Random s_random = new();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            List<T> newList = new List<T>(list);

            int counter = newList.Count;

            while (counter > 1)
            {
                counter--;
                int k = s_random.Next(counter + 1);
                (newList[k], newList[counter]) = (newList[counter], newList[k]);
            }

            return newList;
        }
    }
}
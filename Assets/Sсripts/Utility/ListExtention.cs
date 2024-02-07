using System;
using System.Collections.Generic;

public static class ListExtention
{
    private static Random s_random = new();

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        int counter = list.Count;

        while (counter > 1)
        {
            counter--;
            int k = s_random.Next(counter + 1);
            (list[k], list[counter]) = (list[counter], list[k]);
        }

        return list;
    }
}
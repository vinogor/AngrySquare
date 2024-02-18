using System;
using System.Collections;

namespace _Project.Sсripts.Utility
{
    public static class Validator
    {
        public static void ValidateAmount(ICollection collection, int amount)
        {
            if (collection.Count != amount)
            {
                throw new ArgumentOutOfRangeException(
                    $"Wrong amount of elements in the collection, must be {amount}, but {collection.Count}");
            }
        }
    }
}
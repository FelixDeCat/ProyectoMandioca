using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class RoulletteWheel
    {
        public static Dictionary<KEY, VALUE> CreateDictionary<KEY, VALUE>(List<KEY> listT, List<VALUE> listK)
        {
            Dictionary<KEY, VALUE> result = new Dictionary<KEY, VALUE>();

            for (int i = 0; i < listT.Count; i++)
            {
                if (i < listK.Count)
                    result.Add(listT[i], listK[i]);
                else
                    break;
            }

            return result;
        }

        public static T Roullette<T>(Dictionary<T, int> myPosibilities)
        {
            int totalCoef = 0;
            foreach (var item in myPosibilities)
            {
                totalCoef += item.Value;
            }

            int random = Random.Range(1, totalCoef);

            foreach (var item in myPosibilities)
            {
                random -= item.Value;

                if (random <= 0)
                    return item.Key;
            }

            return default(T);
        }
    }
}

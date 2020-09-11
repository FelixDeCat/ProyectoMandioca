using UnityEngine;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Utility {
    // Transformaciones

    public static int Clampi(int v, int min, int max)
    {
        return v < min ? min : (v > max ? max : v);
    }

	public static IEnumerable<Src> Generate<Src>(Src seed, Func<Src, Src> generator) {
		while (true) {
			yield return seed;
			seed = generator(seed);
		}
	}

    public static T Log<T>(T value, string prefix = "")
    {
        Debug.Log(prefix + value);
        return value;
    }

    public static bool In<T>(this T x, HashSet<T> set)
    {
        return set.Contains(x);
    }

    public static IEnumerable<T> Test<T>(this IEnumerable<T> x, Func<bool> condition)
    {
        return x.Where(t => condition());
    }

    public static bool In<K, V>(this KeyValuePair<K, V> x, Dictionary<K, V> dict)
    {
        return dict.Contains(x);
    }

    public static void UpdateWith<K, V>(this Dictionary<K, V> a, Dictionary<K, V> b)
    {
        foreach (var kvp in b)
        {
            a[kvp.Key] = kvp.Value;
        }
    }

    public static void UpdateWith<K>(this List<K> a, List<K> b)
    {
        foreach (var k in b)
        {
            if (!a.Contains(k))
                a.Add(k);
        }
    }

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> list)
    {
        return new HashSet<T>(list);
    }


    public static T Clone<T>(this object item)
    {
        if (item != null)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);

            T result = (T)formatter.Deserialize(stream);

            stream.Close();

            return result;
        }
        else
            return default(T);
    }
    public static V DefaultGet<K, V>(
        this Dictionary<K, V> dict,
        K key,
        Func<V> defaultFactory
    )
    {
        V v;
        if (!dict.TryGetValue(key, out v))
            dict[key] = v = defaultFactory();
        return v;
    }
}

using System.Collections.Generic;
using System.Linq;

namespace IA2Final
{
    public class GOAPState
    {
        public GOAPValue values = new GOAPValue();
        public GOAPAction generatingAction = null;
        public int step = 0;

        #region CONSTRUCTOR
        public GOAPState(GOAPAction gen = null)
        {
            generatingAction = gen;
        }

        public GOAPState(GOAPState source, GOAPAction gen = null)
        {
            foreach (var elem in source.values.boolValues) values.boolValues.Add(elem.Key, elem.Value);
            foreach (var elem in source.values.intValues) values.intValues.Add(elem.Key, elem.Value);
            foreach (var elem in source.values.floatValues) values.floatValues.Add(elem.Key, elem.Value);
            foreach (var elem in source.values.stringValues) values.stringValues.Add(elem.Key, elem.Value);
            generatingAction = gen;
        }
        #endregion

        public override bool Equals(object obj)
        {
            var other = obj as GOAPState;
            var result =
                other != null
                && other.generatingAction == generatingAction       //Very important to keep! TODO: REVIEW
                && other.values.boolValues.Count == values.boolValues.Count
                && other.values.boolValues.All(kv => kv.In(values.boolValues))
                && other.values.floatValues.Count == values.floatValues.Count
                && other.values.floatValues.All(kv => kv.In(values.floatValues))
                && other.values.intValues.Count == values.intValues.Count
                && other.values.intValues.All(kv => kv.In(values.intValues))
                && other.values.stringValues.Count == values.stringValues.Count
                && other.values.stringValues.All(kv => kv.In(values.stringValues));
            //&& other.values.All(kv => values.Contains(kv));
            return result;
        }

        public override int GetHashCode()
        {
            //Better hashing but slow.
            //var x = 31;
            //var hashCode = 0;
            //foreach(var kv in values) {
            //	hashCode += x*(kv.Key + ":" + kv.Value).GetHashCode);
            //	x*=31;
            //}
            //return hashCode;

            //Heuristic count+first value hash multiplied by polynomial primes
            return values.boolValues.Count == 0 ? 0 : 31 * values.boolValues.Count + 31 * 31 * values.boolValues.First().GetHashCode();
        }

        public override string ToString()
        {
            var str = "";
            foreach (var kv in values.boolValues.OrderBy(x => x.Key))
            {
                str += $"{kv.Key:12} : {kv.Value}\n";
            }
            return "--->" + (generatingAction != null ? generatingAction.name : "NULL") + "\n" + str;
        }
    }
}

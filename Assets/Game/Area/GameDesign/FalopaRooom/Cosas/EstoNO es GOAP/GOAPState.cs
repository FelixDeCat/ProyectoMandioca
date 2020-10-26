using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class GoapState
    {
        //podrias tener las variables directamente en GoapState y no necesariamente en una variable como esta adentro,
        //lo que te sea mas prolijo
        public WorldStateSnapShot worldStateSnap;

        public Dictionary<string, bool> valoresBool = new Dictionary<string, bool>();
        public Dictionary<string, float> valoresFloat = new Dictionary<string, float>();
        public Dictionary<string, int> valoresInt = new Dictionary<string, int>();
        public List<Item> misItems = new List<Item>();

        public GoapAction generatingAction = null;
        public int step = 0;

        public GoapState() { }

        #region CONSTRUCTOR

        public GoapState(GoapState source, GoapAction gen = null)
        {
            //worldStateSnap = new WorldStateSnapShot();

            //worldStateSnap.values = new Dictionary<string, bool>();
            //worldStateSnap.values.UpdateWith(source.worldStateSnap.values);

            //worldStateSnap.allItems = new List<Item>();
            //worldStateSnap.allItems = source.worldStateSnap.allItems.GetRange(0, source.worldStateSnap.allItems.Count);

            for (int i = 0; i < source.misItems.Count; i++)
            {
                misItems.Add(source.misItems[i]);
            }

            valoresBool = source.valoresBool.Clone<Dictionary<string, bool>>();
            valoresFloat = source.valoresFloat.Clone<Dictionary<string, float>>();
            valoresInt = source.valoresInt.Clone<Dictionary<string, int>>();

            //valoresBool.UpdateWith(source.valoresBool);
            //valoresInt.UpdateWith(source.valoresInt);
            //valoresFloat.UpdateWith(source.valoresFloat);

            generatingAction = gen;
        }
        #endregion
    }
}




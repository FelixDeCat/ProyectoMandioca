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

        public GoapAction generatingAction = null;
        public int step = 0;

        public GoapState() { }

        #region CONSTRUCTOR

        public GoapState(GoapState source, GoapAction gen = null)
        {
            worldStateSnap = new WorldStateSnapShot();

            worldStateSnap.values = new Dictionary<string, bool>();
            worldStateSnap.values.UpdateWith(source.worldStateSnap.values);

            worldStateSnap.eData = source.worldStateSnap.eData;


            worldStateSnap.allItems = new List<Item>();

            //        for(var i = source.worldStateSnap.allItems.Count - 1; i > -1; i--)
            //        {
            //            if (source.worldStateSnap.allItems[i] == null)
            //                source.worldStateSnap.allItems.RemoveAt(i);
            //        }
            worldStateSnap.allItems = source.worldStateSnap.allItems.GetRange(0, source.worldStateSnap.allItems.Count);

            generatingAction = gen;
        }
        #endregion
    }
}




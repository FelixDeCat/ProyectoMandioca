using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class WorldState : MonoBehaviour
    {
        public static WorldState instance;

        public Dictionary<string, bool> values = new Dictionary<string, bool>();
        public List<Item> allItems;
        public Ente ente;
        public float pressurePlate;

        public bool debugSpotted;

        public Action OnDudeHidden = delegate { };
        public Action OnDudeSpotted = delegate { };

        public bool hiddenDebug;


        [Header("MANDIOCA")]

        public Item objectiveTEST;


        public EnteDATA enteDATA;
        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Start()
        {
            //Primera imagen del mundo al darle play
            values["hasKey"] = false;


       


       
        }
       

        //Copia del mundo
        public WorldStateSnapShot WorldStateSnapShot()
        {
            var snap = new WorldStateSnapShot();

            snap.allItems = allItems.Where(x => x != null && x.interactuable).ToList();
            snap.values.UpdateWith(values);

           
            snap.eData.hpMax = ente.health;
            snap.eData.hp = ente.healthCurrent;
            snap.eData.pos = ente.transform.position;
            

            /// ////

            snap.objTEST = objectiveTEST;

            return snap;
        }

    }

    //Data de la entidad "dude"
    public struct EnteDATA
    {
        public Vector3 pos;
        public float hpMax;
        public float hp;
        public bool hidden;
    }

    public class WorldStateSnapShot
    {
        public List<Item> allItems = new List<Item>();
        public Dictionary<string, bool> values = new Dictionary<string, bool>();
        public EnteDATA eData;

        public Item objTEST;

        public WorldStateSnapShot()
        {

        }
    }
}



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
        public CharacterHead characterhead;
        public float distanceToHero;
        public bool canSpeedbuff;


        [Header("MANDIOCA")]
        public WorldStateSnapShot snapDebug;
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

            characterhead = Main.instance.GetChar();

            allItems.Add(characterhead.GetComponent<Item>());

            

        }
       

        //Copia del mundo
        public WorldStateSnapShot WorldStateSnapShot()
        {
            var snap = new WorldStateSnapShot();

            ente = FindObjectOfType<Ente>();
            snap.allItems = allItems.Where(x => x != null && x.interactuable).ToList();
            snap.values.UpdateWith(values);

           
            snap.eData.hpMax = ente.health;
            snap.eData.hp = ente.healthCurrent;
            snap.eData.pos = ente.transform.position;
            snap.canSpeedbuff = ente.canSpeedBuff;
            

            /// ////

            //snap.objTEST = objectiveTEST;
            snap.charRoot = this.characterhead.Root;
            snap.charLife = this.characterhead.Life.GetLife();
            snap.distanceToHero = Vector3.Distance(snap.charRoot.position, snap.eData.pos);



            snapDebug = snap;
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
    [Serializable]
    public class WorldStateSnapShot
    {
        public List<Item> allItems = new List<Item>();
        public Dictionary<string, bool> values = new Dictionary<string, bool>();
        public EnteDATA eData;
        public Transform charRoot;
        public int charLife;
        public float distanceToHero;
        public bool canSpeedbuff;

        public Item objTEST;

        public WorldStateSnapShot()
        {

        }
    }
}



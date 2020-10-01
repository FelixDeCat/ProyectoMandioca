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
        public CharacterHead characterhead;
        public float distanceToHero;



        [Header("MANDIOCA")]
        public WorldStateSnapShot snapDebug;
        public Item objectiveTEST;
        public float DEBUGdistanceToHero;

        //        public EnteDATA enteDATA;
        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Start()
        {
            characterhead = Main.instance.GetChar();

            allItems.Add(characterhead.GetComponent<Item>());

            values["HandOfDead"] = true;
            values["RagingPoolsOfFire"] = true;

        }
       

        //Copia del mundo
        public WorldStateSnapShot WorldStateSnapShot()
        {
            var snap = new WorldStateSnapShot();

            ente = FindObjectOfType<Ente>();
            snap.allItems = allItems.Where(x => x != null && x.interactuable).ToList();
            //snap.values.UpdateWith(values); 

            /// ////

            snap.charRoot = this.characterhead.Root;
            snap.charLife = this.characterhead.Life.GetLife();
            snap.distanceToHero = Vector3.Distance(snap.charRoot.position, ente.Root().position);

            snap.skills.UpdateWith(ente.skillManager.GetAllSkills);

            foreach (var s in snap.skills)
            {
                snap.allItems.Add(s.Value.GetComponent<Item>());
                snap.values.Add(s.Key, s.Value.isAvaliable);
            }

            //snap.distanceToHero = 

            //snapDebug = snap;
            //snap.handActive = ente.skillManager.GetAllSkills["HandOfDead"];
            //snap.poolActive = ente.skillManager.GetAllSkills["RagingPoolsOfFire"];
            return snap;
        }

    }

    //Data de la entidad "dude"
   
    //public struct EnteDATA
    //{
    //    public Vector3 pos;
    //    public float hpMax;
    //    public float hp;
    //    public bool hidden;
    //}
    [Serializable]
    public class WorldStateSnapShot
    {
        public List<Item> allItems = new List<Item>();
        public Dictionary<string, bool> values = new Dictionary<string, bool>();
        public Dictionary<string, GOAP_Skills_Base> skills = new Dictionary<string, GOAP_Skills_Base>();
        public bool handActive;
        public bool poolActive;
        //public EnteDATA eData;
        public Transform charRoot;
        public int charLife;
        public float distanceToHero;

        public WorldStateSnapShot()
        {

        }
    }
}



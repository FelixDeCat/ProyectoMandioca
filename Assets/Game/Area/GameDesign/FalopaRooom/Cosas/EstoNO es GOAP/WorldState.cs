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
        public bool debug_OnGround;


        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Start()
        {
            characterhead = Main.instance.GetChar();

            allItems.Add(characterhead.GetComponent<Item>());
            values["OnGround"] = true;
        }
       

        //Copia del mundo
        public WorldStateSnapShot WorldStateSnapShot()
        {
            var snap = new WorldStateSnapShot();

            ente = FindObjectOfType<Ente>();
            snap.allItems = allItems.Where(x => x != null && x.interactuable).ToList();
            snap.values.UpdateWith(values);
            snap.ente_highlevel = ente.heightLevel;

            snap.charRoot = this.characterhead.Root;
            snap.charLife = this.characterhead.Life.GetLife();
            snap.distanceToHero = Vector3.Distance(snap.charRoot.position, ente.Root().position);

            snap.skills.UpdateWith(ente.skillManager.GetAllSkills);

            foreach (var s in snap.skills)
            {
                snap.allItems.Add(s.Value.GetComponent<Item>());
                snap.values.Add(s.Key, s.Value.isAvaliable);
            }


            return snap;
        }


        private void Update()
        {
            debug_OnGround = values["OnGround"];
            if (characterhead == null || ente == null) return;

            DEBUGdistanceToHero = Vector3.Distance(characterhead.Root.position, ente.Root().position);
        }
    }

   

    [Serializable]
    public class WorldStateSnapShot
    {
        public List<Item> allItems = new List<Item>();
        public Dictionary<string, bool> values = new Dictionary<string, bool>();
        public Dictionary<string, GOAP_Skills_Base> skills = new Dictionary<string, GOAP_Skills_Base>();
        public Transform charRoot;
        public int charLife;
        public float distanceToHero;
        public bool debug_OnGround;
        public int ente_highlevel;

        public WorldStateSnapShot()
        {

        }
    }
}



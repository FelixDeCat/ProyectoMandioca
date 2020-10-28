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

        public Dictionary<string, bool> valoresBool = new Dictionary<string, bool>();
        public Dictionary<string, float> valoresFloat = new Dictionary<string, float>();
        public Dictionary<string, int> valoresInt = new Dictionary<string, int>();
        public List<Item> allItems;


        public Ente ente;
        public CharacterHead characterhead;



        [Header("Debug")]
        public float DEBUGdistanceToHero;
        public bool debug_OnGround;
        public bool debug_skills;


        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Start()
        {
            characterhead = Main.instance.GetChar();
            ente = FindObjectOfType<Ente>();

            //Estado inicial para meter al dic. Esto tiene que actualizarse constantemente de alguna otra manera que poniendolo aca
            valoresBool["OwnerGetDamage"] = false;
        }

        public void RefreshState()
        {
            allItems = new List<Item>();

            allItems.Add(characterhead.GetComponent<Item>());

            foreach (var s in ente.skillManager.GetAllSkills)
            {
                allItems.Add(s.Value.GetComponent<Item>());

                if(!valoresBool.ContainsKey(s.Key))
                {
                    valoresBool.Add(s.Key, s.Value.isAvaliable);
                }
                else
                {
                    valoresBool[s.Key] = s.Value.isAvaliable;
                }
            }

            valoresBool["OnGround"] = ente.heightLevel == 0 ? true : false;
            
            valoresInt["HeroLife"] = characterhead.Life.GetLife();
            valoresFloat["DistanceToHero"] = Vector3.Distance(characterhead.Root.position, ente.Root().position);
        }      

        private void Update()
        {
            
            if (characterhead == null || ente == null) return;

            DEBUGdistanceToHero = Vector3.Distance(characterhead.Root.position, ente.Root().position);

            if(valoresBool.ContainsKey("OnGround"))
            debug_OnGround = valoresBool["OnGround"];
            //debug_skills = valoresBool["LaserShoot"];
        }
    }

 
}



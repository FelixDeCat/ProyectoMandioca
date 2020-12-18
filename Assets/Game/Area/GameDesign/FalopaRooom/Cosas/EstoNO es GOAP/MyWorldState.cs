using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    [System.Serializable]
    public class MyWorldState : EntityData
    {
        public Dictionary<string, bool> valoresBool = new Dictionary<string, bool>();
        public Dictionary<string, float> valoresFloat = new Dictionary<string, float>();
        public Dictionary<string, int> valoresInt = new Dictionary<string, int>();
        public List<Item> allItems;

        CharacterHead characterhead;

        [Header("Debug")]
        public float DEBUGdistanceToHero;
        public bool debug_skills;

        public void Initialize()
        {
            characterhead = Main.instance.GetChar();
            valoresBool["OwnerGetDamage"] = false;
        }

        public void RefreshState()
        {
            allItems = new List<Item>();

            foreach (var s in Skills.GetAllSkills)
            {
                allItems.Add(s.Value.GetComponent<Item>());

                if (!valoresBool.ContainsKey(s.Key))
                {
                    valoresBool.Add(s.Key, s.Value.isAvaliable);
                }
                else
                {
                    valoresBool[s.Key] = s.Value.isAvaliable;
                }
            }

            valoresInt["HeroLife"] = characterhead.Life.GetLife();
            valoresFloat["DistanceToHero"] = Vector3.Distance(characterhead.Root.position, Root.position);
        }

        public override void ManualUpdate()
        {
            if (characterhead == null) return;
            DEBUGdistanceToHero = Vector3.Distance(characterhead.Root.position, Root.position);
        }
    }


}



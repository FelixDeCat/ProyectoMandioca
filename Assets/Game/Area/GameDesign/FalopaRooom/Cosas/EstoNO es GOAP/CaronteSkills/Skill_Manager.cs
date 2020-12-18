using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [System.Serializable]
    public class SkillManager : EntityData
    {
        Dictionary<string, GOAP_Skills_Base> skillRegistry = new Dictionary<string, GOAP_Skills_Base>();
        public void Initialize()
        {
            foreach (Transform skill in Root)
            {
                var s = skill.GetComponent<GOAP_Skills_Base>();
                if (s != null)
                {
                    skillRegistry.Add(s.skillName, s);
                    s.Initialize(Root);
                }
            }
        }
        public GOAP_Skills_Base GetSkill(string skillName) => skillRegistry[skillName];
        public Dictionary<string, GOAP_Skills_Base> GetAllSkills => skillRegistry;
    }
}



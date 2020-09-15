using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class CaronteSkill_Manager : MonoBehaviour
    {
        Dictionary<string, GOAP_Skills_Base> skillRegistry = new Dictionary<string, GOAP_Skills_Base>();

    void Start()
        {
            foreach (Transform skill in transform)
            {
                var s = skill.GetComponent<GOAP_Skills_Base>();
                if (s != null)
                {
                    skillRegistry.Add(s.skillName, s);
                }
            }
        }

        public GOAP_Skills_Base GetSkill(string skillName) => skillRegistry[skillName];
        public Dictionary<string, GOAP_Skills_Base> GetAllSkills => skillRegistry;


    }
}



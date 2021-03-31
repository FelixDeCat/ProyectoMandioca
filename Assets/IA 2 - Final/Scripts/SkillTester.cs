using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTester : MonoBehaviour
{
    [SerializeField] bool active = false;
    bool usingSkill;
    [SerializeField] BossSkills skillToTest = null;
    [SerializeField] float timeToUseSkill = 3;
    float timer;

    void Start()
    {
        skillToTest.Initialize();   
    }

    void Update()
    {
        if(active && !usingSkill)
        {
            timer += Time.deltaTime;

            if (timer >= timeToUseSkill)
            {
                usingSkill = true;
                skillToTest.UseSkill(() => usingSkill = false);
                timer = 0;
            }
        }
        else if (usingSkill)
        {
            skillToTest?.OnUpdate();
        }
    }
}

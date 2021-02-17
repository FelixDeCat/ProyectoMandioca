using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    [System.Serializable]
    public class IA_Linker
    {
        HumanBoss human;
        [SerializeField] Transform Root = null;
        public MyWorldState myWorld; MyWorldState Get_LocalREF_World() => myWorld;
        public SkillManager SkillManager; SkillManager Get_LocalRef_Skills() => SkillManager;
        public HumanStates States; HumanStates Get_LocalREF_States() => States;
        public GenBrainPlanner brainPlaner; GenBrainPlanner Get_LocalREF_Brain() => brainPlaner;

        public void Initialize(HumanBoss human)
        {
            this.human = human;

            /////////////////////////////////////////////////// WORLD

            myWorld
                .Set_Root(Root);
            
            myWorld.Initialize();

            /////////////////////////////////////////////////// SKILLS
            
            SkillManager
                .Set_Root(Root);
            
            SkillManager.Initialize();

            /////////////////////////////////////////////////// STATES

            States
                .Set_Human(human)
                .Set_Skills(Get_LocalRef_Skills)
                .Set_Brain(Get_LocalREF_Brain);
            
            States.Initialize();

            /////////////////////////////////////////////////// BRAIN PLANER

            brainPlaner
                .Set_WorldState(Get_LocalREF_World)
                .Set_States(Get_LocalREF_States);
            
            brainPlaner.Initialize(x => human.StartCoroutine(x), x => human.StopCoroutine(x));
        }

        public void Update()
        {
            myWorld.ManualUpdate();
            States.ManualUpdate();
        }
    }
}


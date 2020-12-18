using System;
using GOAP;
using UnityEngine;

public class EntityData
{
    HumanBoss human = null;
    public EntityData Set_Human(HumanBoss human) { this.human = human; return this; }
    protected HumanBoss Human => human;

    Transform root = null;
    public EntityData Set_Root(Transform _root) { root = _root; return this; }
    protected Transform Root => root;

    public EntityData Set_WorldState(Func<MyWorldState> world) { callbackworld = world; return this; }
    Func<MyWorldState> callbackworld = delegate { return null; };
    protected MyWorldState World => callbackworld.Invoke();

    public EntityData Set_Brain(Func<GenBrainPlanner> brain) { callbackBrain = brain; return this; }
    Func<GenBrainPlanner> callbackBrain = delegate { return null; };
    protected GenBrainPlanner Brain => callbackBrain.Invoke();

    public EntityData Set_States(Func<HumanStates> states) { callbackStates = states; return this; }
    Func<HumanStates> callbackStates = delegate { return null; };
    protected HumanStates States => callbackStates.Invoke();

    public EntityData Set_Skills(Func<SkillManager> skills) { callbackSkills = skills; return this; }
    Func<SkillManager> callbackSkills = delegate { return null; };
    protected SkillManager Skills => callbackSkills.Invoke();

    public virtual void ManualUpdate() { }
    public virtual void Pause() { }
    public virtual void Resume() { }


}

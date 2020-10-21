using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class DungeonBoss_BrainPlanner : BrainPlanner
{
    [Header("Cost settings")]
    public int move;
    public int laserShoot;
    public int summon;

    protected override List<GoapAction> GetActionList()
    {
        return new List<GoapAction>()
            {
                    new GoapAction("GoTo hero")
                        .SetCost(move)
                        //.Pre(gS =>  gS.distanceToHero > 2f)

                        .Effect(gS =>
                        {
                            gS.distanceToHero = 1;
                        }),
                    new GoapAction("useSkill LaserShoot")
                        .SetCost(laserShoot)
                        .Pre(gS =>  gS.values["LaserShoot"] && gS.distanceToHero >= 5f && gS.distanceToHero <= 15f)

                        .Effect(gS =>
                        {
                            gS.values["LaserShoot"] = false;
                            gS.charLife -= 5;
                        }),
                    new GoapAction("useSkill SummonMinions")
                        .SetCost(summon)
                        .Pre(gS => gS.values["SummonMinions"]) //&&  gS.distanceToHero >= 5f && gS.distanceToHero <= 10f)

                        .Effect(gS =>
                        {
                            gS.values["SummonMinions"] = false;
                            gS.charLife -= 10;
                        }),
            };
    }

    protected override Dictionary<string, ItemType> TypeDic()
    {
        return new Dictionary<string, ItemType>()
        {
          { "hero", ItemType.hero },
          { "SummonMinions", ItemType.skill },
          { "LaserShoot", ItemType.skill }
        };
    }
}

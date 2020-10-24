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
    public int fly;
    public int avoid;

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
                    new GoapAction("Avoid hero")
                        .SetCost(avoid)
                        .Pre(gS =>  gS.distanceToHero <= 2f)

                        .Effect(gS =>
                        {
                            gS.distanceToHero = 10;
                        }),
                    new GoapAction("useSkill LaserShoot")
                        .SetCost(laserShoot)
                        .Pre(gS =>  gS.values["LaserShoot"] && !gS.values["OnGround"] && gS.distanceToHero >= 1f && gS.distanceToHero <= 20f)

                        .Effect(gS =>
                        {
                            gS.values["LaserShoot"] = false;
                            gS.charLife -= 5;
                        }),
                    new GoapAction("useSkill Fly")
                        .SetCost(fly)
                        .Pre(gS => gS.values["Fly"] && gS.values["OnGround"]) //&&  gS.distanceToHero >= 5f && gS.distanceToHero <= 10f)

                        .Effect(gS =>
                        {
                            gS.distanceToHero = 10;
                            gS.values["OnGround"] = false;
                            gS.values["Fly"] = false;
                        }),
                    new GoapAction("useSkill SummonMinions")
                        .SetCost(summon)
                        .Pre(gS => gS.values["OnGround"] == false && gS.values["SummonMinions"] &&  gS.distanceToHero >= 5f && gS.distanceToHero <= 12f)

                        .Effect(gS =>
                        {
                            gS.values["SummonMinions"] = false;
                            gS.charLife -= 10;
                        })
                    
           
            };
    }

    protected override int Final(GoapState gS)
    {
        int h = 0;
        if (gS.worldStateSnap.charLife > 0) h += 1;
        if (gS.worldStateSnap.distanceToHero <= 5) h += 2;
        if (gS.worldStateSnap.values["OnGround"] == true) h += 1;
        return h;
    }

    protected override Dictionary<string, ItemType> TypeDic()
    {
        return new Dictionary<string, ItemType>()
        {
          { "hero", ItemType.hero },
          { "SummonMinions", ItemType.skill },
          { "LaserShoot", ItemType.skill },
          { "Fly", ItemType.skill }
        };
    }
}

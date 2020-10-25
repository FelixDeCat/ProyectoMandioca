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
    public int thunderWave;

    protected override List<GoapAction> GetActionList()
    {
        return new List<GoapAction>()
            {
                    new GoapAction("GoTo hero")
                        .SetCost(move)
                        //.Pre(gS =>  gS.distanceToHero > 2f)

                        .Effect(gS =>
                        {
                            gS.valoresInt["HeroLife"] -= 1;
                            gS.valoresFloat["DistanceToHero"] = 1;
                        }),
                    new GoapAction("Avoid hero")
                        .SetCost(avoid)
                        .Pre(gS =>  gS.valoresFloat["DistanceToHero"] <= 2f)

                        .Effect(gS =>
                        {
                            gS.valoresInt["HeroLife"] -= 1;
                            gS.valoresFloat["DistanceToHero"] = 10;
                        }),
                    new GoapAction("useSkill LaserShoot")
                        .SetCost(laserShoot)
                        .Pre(gS =>  gS.valoresBool["LaserShoot"] && gS.valoresFloat["DistanceToHero"] <= 30f) // !gS.values["OnGround"] && gS.distanceToHero >= 1f && 

                        .Effect(gS =>
                        {
                            gS.valoresBool["LaserShoot"] = false;
                            gS.valoresInt["HeroLife"] -= 5;
                        }),
                    new GoapAction("useSkill Fly")
                        .SetCost(fly)
                        .Pre(gS => gS.valoresBool["Fly"] && gS.valoresBool["OnGround"]) 

                        .Effect(gS =>
                        {
                            gS.valoresFloat["DistanceToHero"] = 10;
                            gS.valoresBool["OnGround"] = false;
                            gS.valoresBool["Fly"] = false;
                        }),
                    new GoapAction("useSkill SummonMinions")
                        .SetCost(summon)
                        .Pre(gS => gS.valoresBool["OnGround"] == false && gS.valoresBool["SummonMinions"] &&  gS.valoresFloat["DistanceToHero"] >= 5f && gS.valoresFloat["DistanceToHero"] <= 12f)

                        .Effect(gS =>
                        {
                            gS.valoresBool["SummonMinions"] = false;
                            gS.valoresInt["HeroLife"] -= 10;
                        }),
                     new GoapAction("useSkill ThunderWave")
                        .SetCost(thunderWave)
                        .Pre(gS => gS.valoresBool["ThunderWave"] && gS.valoresFloat["DistanceToHero"] <= 5f && gS.valoresBool["OnGround"]) //

                        .Effect(gS =>
                        {
                            gS.valoresBool["ThunderWave"] = false;
                            gS.valoresInt["HeroLife"] -= 15;
                            gS.valoresFloat["DistanceToHero"] += 15;
                        })


            };
    }

    protected override int Final(GoapState gS)
    {
        int h = 0;
        if (gS.valoresInt["HeroLife"] > 0) h += 1;
        if (gS.valoresFloat["DistanceToHero"] <= 5) h += 1;
        if (gS.valoresBool["OnGround"] == true) h += 1;
        return h;
    }

    protected override Dictionary<string, ItemType> TypeDic()
    {
        return new Dictionary<string, ItemType>()
        {
          { "hero", ItemType.hero },
          { "SummonMinions", ItemType.skill },
          { "LaserShoot", ItemType.skill },
          { "Fly", ItemType.skill },
          { "ThunderWave", ItemType.skill }
        };
    }
}

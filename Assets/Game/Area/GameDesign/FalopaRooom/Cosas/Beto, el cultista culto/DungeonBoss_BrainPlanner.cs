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
                    new GoapAction("GoTo Hero")
                        .SetCost(move)
                        //.Pre(gS =>  gS.distanceToHero > 2f)

                        .Effect(gS =>
                        {
                            gS.valoresBool["OwnerGetDamage"] = false;
                            gS.valoresBool["LaserShoot"] = true;
                            gS.valoresFloat["DistanceToHero"] = 10f;
                            gS.valoresBool["Fly"] = true;
                        }),
                    //new GoapAction("Avoid Hero")
                    //    .SetCost(avoid)
                    //    .Pre(gS =>  gS.valoresFloat["DistanceToHero"] <= 2f)

                    //    .Effect(gS =>
                    //    {
                    //        gS.valoresBool["OwnerGetDamage"] = false;
                    //        gS.valoresBool["Fly"] = true;
                    //        gS.valoresBool["LaserShoot"] = true;
                    //        gS.valoresFloat["DistanceToHero"] = 30f;
                    //    }),
                    new GoapAction("useSkill Fly")
                        .SetCost(fly)
                        .Pre(gS => gS.valoresBool["Fly"] && gS.valoresBool["OnGround"]) 

                        .Effect(gS =>
                        {
                            gS.valoresBool["OwnerGetDamage"] = false;
                            gS.valoresBool["LaserShoot"] = true;
                            gS.valoresFloat["DistanceToHero"] = 10;
                            gS.valoresBool["OnGround"] = false;
                            gS.valoresBool["Fly"] = false;
                        }),
                    new GoapAction("useSkill SummonMinions")
                        .SetCost(summon)
                        .Pre(gS => (!gS.valoresBool["OnGround"]) && gS.valoresBool["SummonMinions"] && gS.valoresFloat["DistanceToHero"] <= 20f)

                        .Effect(gS =>
                        {
                            gS.valoresBool["SummonMinions"] = false;
                            gS.valoresInt["HeroLife"] -= 25;
                        }),
                     new GoapAction("useSkill ThunderWave")
                        .SetCost(thunderWave)
                        .Pre(gS => gS.valoresBool["ThunderWave"] && gS.valoresFloat["DistanceToHero"] <= 10f && gS.valoresBool["OnGround"] && gS.valoresBool["OwnerGetDamage"]) //

                        .Effect(gS =>
                        {
                            gS.valoresBool["OwnerGetDamage"] = false;
                            gS.valoresBool["ThunderWave"] = false;
                            gS.valoresInt["HeroLife"] -= 25;
                            gS.valoresFloat["DistanceToHero"] += 15;
                        }),
                        new GoapAction("useSkill LaserShoot")
                        .SetCost(laserShoot)
                        .Pre(gS =>  gS.valoresBool["LaserShoot"] &&  gS.valoresFloat["DistanceToHero"] >= 2f && gS.valoresFloat["DistanceToHero"] <= 30f) // !gS.values["OnGround"]  && 

                        .Effect(gS =>
                        {
                            gS.valoresBool["LaserShoot"] = false;
                            gS.valoresInt["HeroLife"] -= 10;
                        })


            };
    }

    protected override int Final(GoapState gS)
    {
        int h = 0;
        if (gS.valoresInt["HeroLife"] > 0) h += 1;
        //if (gS.valoresFloat["DistanceToHero"] <= 5) h += 1;
        if (gS.valoresBool["OnGround"] == true) h += 3;
        if (gS.valoresBool["OwnerGetDamage"]) h += 1;
        return h;
    }

    protected override Dictionary<string, ItemType> TypeDic()
    {
        return new Dictionary<string, ItemType>()
        {
          { "Hero", ItemType.hero },
          { "SummonMinions", ItemType.skill },
          { "LaserShoot", ItemType.skill },
          { "Fly", ItemType.skill },
          { "ThunderWave", ItemType.skill }
        };
    }
}

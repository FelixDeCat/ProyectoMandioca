using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOAP
{
    public class BrainPlanner : MonoBehaviour
    {

        //[Header("Cost settings")]
        //public int meleeAttack;
        //public int handOfDead;
        //public int ragingPoolsOfFire;
        //public int flameThrower;
        //public int move;

        public float distanceDebug;
        public bool OnGround_debug;

        private readonly List<Tuple<Vector3, Vector3>> _debugRayList = new List<Tuple<Vector3, Vector3>>();

        public void StartPlanning() => StartCoroutine(Plan());

        private IEnumerator Plan()
        {
            yield return new WaitForSeconds(.3f);

            var snap = WorldState.instance.WorldStateSnapShot();

            var actions = GetActionList();
            
            var typeDict = TypeDic();
              
            var actDict = new Dictionary<string, ActionEntity>() {
              { "AttackMelee", ActionEntity.MeleeAttack },
              { "useSkill", ActionEntity.UseSkill},
              { "GoTo", ActionEntity.Move },
              { "Avoid", ActionEntity.Avoid }
        };

            Func<GoapState, int> final = (gS) => Final(gS);
                
            GoapState initial = new GoapState();
            initial.worldStateSnap = snap;

            //OnGround_debug = initial.worldStateSnap.values["OnGround"];
            
            //TimeSlicing 3 - En este caso queriamos guardar el path en algun lado
            //Al no estar usando una variable publica creamos una interna con un valor default
            IEnumerable<GoapAction> plan = Enumerable.Empty<GoapAction>();
            yield return Goap.Execute(initial, final, actions,
                (p) => plan = p);//TimeSlicing 3 - Este es el callBack al finalizar el Goap y se le pondria el plan encontrado a la variable que habiamos creado

            if (plan == null)
            {
                StartPlanning();
                Debug.Log("Couldn't plan");
            }
            else
            {
                GetComponent<Dude>().ExecutePlan(
                    plan
                    .Select(pa => pa.Name)
                    .Select(a =>
                    {
                        var possibleItems = snap.allItems.Where(x => x != null && x.interactuable).Where(i =>
                            typeDict.Any(kv => a.EndsWith(kv.Key)) &&
                            i.type == typeDict.First(kv => a.EndsWith(kv.Key)).Value);

                        var i2 = possibleItems.Skip(Random.Range(0, possibleItems.Count())).FirstOrDefault();

                        if (actDict.Any(kv => a.StartsWith(kv.Key)) && i2 != null)
                        {
                            return Tuple.Create(actDict.First(kv => a.StartsWith(kv.Key)).Value, i2);
                        }
                        else
                        {
                            return null;
                        }
                    }).Where(a => a != null)
                    .ToList()
                );
            }
        }

        //Estado final desacoplado para poder darle en las herencias el deseado
        protected virtual int Final(GoapState gS)
        {
            int h = 0;
            if (gS.worldStateSnap.charLife > 0) h += 1;
            return h;
        }

        protected virtual Dictionary<string, ItemType> TypeDic()
        {
            return default;

            //return new Dictionary<string, ItemType>()
            //{
            //  { "hero", ItemType.hero },
            //  { "handOfDead", ItemType.skill },
            //  { "ragingPoolsOfFire", ItemType.skill },
            //  { "flameThrower", ItemType.skill }
            //};
        }

        protected virtual List<GoapAction> GetActionList()
        {
            return default;
        }

        //private List<GoapAction> CreatePossibleActionsList()
        //{
        //    return new List<GoapAction>()
        //    {
        //            new GoapAction("useSkill handOfDead")
        //                .SetCost(handOfDead)
        //                .Pre(gS =>  gS.values["HandOfDead"] && gS.distanceToHero >= 5)

        //                .Effect(gS =>
        //                {
        //                    gS.values["HandOfDead"] = false;
        //                    gS.charLife -= 10;
        //                    gS.distanceToHero = 1;
        //                }),
        //             new GoapAction("useSkill ragingPoolsOfFire")
        //                .SetCost(handOfDead)
        //                .Pre(gS =>  gS.values["RagingPoolsOfFire"] && gS.distanceToHero >= 5)

        //                .Effect(gS =>
        //                {
        //                    gS.values["RagingPoolsOfFire"] = false;
        //                    gS.charLife -= 20;
        //                }),
        //             new GoapAction("useSkill flameThrower")
        //                .SetCost(handOfDead)
        //                .Pre(gS =>  gS.values["FlameThrower"] && gS.distanceToHero <= 8)

        //                .Effect(gS =>
        //                {
        //                    gS.values["FlameThrower"] = false;
        //                    gS.charLife -= 20;
        //                }),
        //            new GoapAction("GoTo hero")
        //                .SetCost(move)
        //                //.Pre(gS =>  gS.distanceToHero > 2f)

        //                .Effect(gS =>
        //                {
        //                    gS.distanceToHero = 1;
        //                }),


        //    };
        //}

        #region ejemplo
        //private List<GoapAction> CreatePossibleActionsList()
        //{
        //    return new List<GoapAction>()
        //{
        //    new GoapAction("OpenKD puertaK")
        //        .SetCost(openKD)
        //        .Pre(gS =>  gS.values["hasKey"] )

        //        .Effect(gS =>
        //        {
        //            gS.values["finalReachable"] = true;
        //            gS.values["keyDoor"] = false;
        //        }),

        //    new GoapAction("Stand pPlate")
        //        .SetCost(stand)
        //        .Pre(gS =>  !gS.values["hasKey"] && !gS.values["firstcloseDoor"] && !gS.values["secondcloseDoor"] && gS.values["rejas"])

        //        .Effect(gS => {gS.values["rejas"] = false;}),

        //    new GoapAction("OpenCD c")
        //        .SetCost(openCD)
        //        .Pre(gS => gS.values["secondcloseDoor"] && gS.eData.coins >= 3)

        //        .Effect(gS => gS.values["secondcloseDoor"] = false),

        //    new GoapAction("Get hp")
        //        .SetCost(getHp)
        //        .Pre(gS => gS.allItems.Any(i => i.type == ItemType.Hp) && (gS.eData.hp/gS.eData.hpMax) <= .4f)

        //        .Effect(gS =>
        //        {
        //            gS.eData.hp += 5;
        //        }),

        //    new GoapAction("Pickup l")
        //        .SetCost(pick_key)
        //        .Pre(gS =>   !gS.values["hasKey"] && !gS.values["firstcloseDoor"] && !gS.values["secondcloseDoor"] && !gS.values["rejas"])

        //        .Effect(gS =>  gS.values["hasKey"] = true),

        //    new GoapAction("Press btt")
        //        .SetCost(pressBtt)
        //        .Pre(gS => gS.values["firstcloseDoor"] && (gS.eData.hp/gS.eData.hpMax) >= .4f)

        //        .Effect(gS =>
        //        {
        //            gS.values["firstcloseDoor"] = false;
        //        }),

        //    new GoapAction("Collect coin")
        //        .SetCost(collectCoin)
        //        .Pre(gS => !gS.values["firstcloseDoor"] && gS.eData.coins <= 2)

        //        .Effect(gS => { gS.eData.coins++; }),


        //    new GoapAction("Go pf")
        //    .SetCost(pastafrola)
        //    .Pre(gS =>  gS.values["finalReachable"])

        //    .Effect(gS => {gS.values["aprobarFinal"] = true;}),

        //    new GoapAction("Hide hSpot")
        //    .SetCost(1)
        //    .Pre(gS =>  gS.values["spottedDude"] && !gS.values["secondcloseDoor"])

        //    .Effect(gS =>
        //    {
        //        gS.eData.hidden = true;
        //        gS.values["spottedDude"] = false;
        //    })
        //};
        //}
        #endregion



    }
}






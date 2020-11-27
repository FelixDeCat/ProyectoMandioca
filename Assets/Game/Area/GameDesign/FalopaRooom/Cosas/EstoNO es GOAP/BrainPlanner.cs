using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class BrainPlanner : MonoBehaviour
    {
        IEnumerator plan;
        public event Action OnCantPlan;
        public float distanceDebug;
        public bool OnGround_debug;

        private void Start()
        {
            plan = Plan();
        }

        public void StartPlanning()
        {
            plan = Plan();
            StartCoroutine(plan);
        }

        public void StopPlanning() => StopCoroutine(plan);

        private IEnumerator Plan()
        {
            var actions = GetActionList();
            var typeDict = TypeDic();

            var actDict = new Dictionary<string, ActionEntity>() {
              { "AttackMelee", ActionEntity.MeleeAttack },
              { "useSkill", ActionEntity.UseSkill},
              { "GoTo", ActionEntity.Move },
              { "Avoid", ActionEntity.Avoid }
        };

            Func<GoapState, int> final = (gS) => Final(gS);

            IEnumerable<GoapAction> plan = Enumerable.Empty<GoapAction>();
            yield return Goap.Execute(
                GetRefreshWorldState(), 
                final, 
                actions,
                (p) => plan = p, 
                () => plan = null);

            if (plan == null)
            {
                yield return new WaitForSeconds(0.2f);
                OnCantPlan?.Invoke();
            }
            else
            {
                GetComponent<Dude>().ExecutePlan(
                    plan
                    .Select(pa => pa.Name)
                    .Select(a =>
                    {
                        var i2 = WorldState.instance.allItems.Where(i => a.EndsWith(i.itemName_id) && i.type == typeDict.First(kv => a.EndsWith(kv.Key)).Value).First();

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

        GoapState GetRefreshWorldState()
        {
            WorldState.instance.RefreshState();
            GoapState initial = new GoapState();
            for (int i = 0; i < WorldState.instance.allItems.Count; i++)
                initial.misItems.Add(WorldState.instance.allItems[i]);
            initial.valoresBool = WorldState.instance.valoresBool.Clone<Dictionary<string, bool>>();
            initial.valoresFloat = WorldState.instance.valoresFloat.Clone<Dictionary<string, float>>();
            initial.valoresInt = WorldState.instance.valoresInt.Clone<Dictionary<string, int>>();
            return initial;
        }

        protected virtual int Final(GoapState gS) => (gS.valoresInt["HeroLife"] > 0) ? 1 : 0;
        protected virtual Dictionary<string, ItemType> TypeDic() => default;
        protected virtual List<GoapAction> GetActionList() => default;

    }
}






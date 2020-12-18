using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    [System.Serializable]
    public class GenBrainPlanner : EntityData
    {
        IEnumerator plan;
        public event Action OnCantPlan;
        public float distanceDebug;
        public bool OnGround_debug;

        Action<IEnumerator> StartCoroutine = delegate { };
        Action<IEnumerator> StopCoroutine = delegate { };

        public void Initialize(Action<IEnumerator> StartCoroutine, Action<IEnumerator> StopCoroutine)
        {
            this.StartCoroutine = StartCoroutine;
            this.StopCoroutine = StopCoroutine;
            StartPlanning();
        }

        public void StartPlanning() => StartCoroutine(plan);
        public void StopPlanning() => StopCoroutine(plan);

        private IEnumerator Plan()
        {
            var actions = GetActionList();
            var typeDict = TypeDic();

            var actDict = new Dictionary<string, HumanActions>() {
              { "AttackMelee", HumanActions.MeleeAttack },
              { "useSkill", HumanActions.UseSkill},
              { "GoTo", HumanActions.Move },
              { "Avoid", HumanActions.Avoid }
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
                States.ExecutePlan(
                    plan
                    .Select(pa => pa.Name)
                    .Select(a =>
                    {
                        var i2 = World.allItems.Where(i => a.EndsWith(i.itemName_id) && i.type == typeDict.First(kv => a.EndsWith(kv.Key)).Value).First();

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
            World.RefreshState();

            var allitems = World.allItems;

            GoapState initial = new GoapState();
            for (int i = 0; i < allitems.Count; i++)
                initial.misItems.Add(allitems[i]);
            initial.valoresBool = World.valoresBool.Clone<Dictionary<string, bool>>();
            initial.valoresFloat = World.valoresFloat.Clone<Dictionary<string, float>>();
            initial.valoresInt = World.valoresInt.Clone<Dictionary<string, int>>();
            return initial;
        }

        protected virtual int Final(GoapState gS) => (gS.valoresInt["HeroLife"] > 0) ? 1 : 0;
        protected virtual Dictionary<string, ItemType> TypeDic() => default;
        protected virtual List<GoapAction> GetActionList() => default;

    }
}






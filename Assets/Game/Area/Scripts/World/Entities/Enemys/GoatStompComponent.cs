using System.Linq;
using UnityEngine;

public class GoatStompComponent : CombatComponent
{
    [Header("Overlap")]
    [SerializeField] LayerMask _lm = 0;
    [SerializeField] float radious = 2;
    [SerializeField] Transform rot = null;


    public override void ManualTriggerAttack()
    {
        Calculate();
    }
    public override void BeginAutomaticAttack()
    {

    }

    public override void Play()
    {
    }

    public override void Stop()
    {
    }

    void Calculate()
    {
        var enemies = Physics.OverlapSphere(rot.position, radious, _lm).Select((x) => x.GetComponent<DamageReceiver>()).ToArray();
        for (int i = 0; i < enemies.Length; i++)
            giveDmgCallback.Invoke(enemies[i]);
    }
}

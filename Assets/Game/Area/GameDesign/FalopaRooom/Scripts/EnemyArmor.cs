using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmor : MonoBehaviour
{
    [SerializeField] DamageReceiver armoredObject;

    private void Start()
    {
        armoredObject.AddInvulnerability(Damagetype.Normal);
        armoredObject.AddInvulnerability(Damagetype.Heavy);
    }

    public void LoseArmor()
    {
        StartCoroutine(DelayedBrokeArmor());
    }

    IEnumerator DelayedBrokeArmor()
    {
        yield return new WaitForSeconds(1);
        armoredObject.RemoveInvulnerability(Damagetype.Heavy);
        armoredObject.RemoveInvulnerability(Damagetype.Normal);
    }
}

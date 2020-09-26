using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmor : MonoBehaviour
{
    [SerializeField] DamageReceiver armoredObject = null;
    [SerializeField] PropDestructible armor = null;

    private void Start()
    {
        armoredObject.AddInvulnerability(Damagetype.Normal);
        armoredObject.AddInvulnerability(Damagetype.Heavy);
    }

    public void LoseArmor()
    {
        StartCoroutine(DelayedBrokeArmor());
        armor.gameObject.SetActive(false);
    }

    public void OnReset()
    {
        armoredObject.AddInvulnerability(Damagetype.Normal);
        armoredObject.AddInvulnerability(Damagetype.Heavy);
        StopAllCoroutines();
        armor.gameObject.SetActive(true);
        armor.OnReset();
    }

    IEnumerator DelayedBrokeArmor()
    {
        yield return new WaitForSeconds(1);
        armoredObject.RemoveInvulnerability(Damagetype.Heavy);
        armoredObject.RemoveInvulnerability(Damagetype.Normal);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmor : MonoBehaviour
{
    [SerializeField] DamageReceiver armoredObject = null;
    [SerializeField] PropDestructible armor = null;
    [SerializeField] ParticleSystem armorHitParticle = null;
    [SerializeField] AudioClip armorHitSound = null;

    private void Start()
    {
        armor.Initialize();
        armor.On();
        armoredObject.AddInvulnerability(Damagetype.Normal);
        armoredObject.AddInmuneFeedback(InvulnerabilityFeedback);
        armoredObject.AddInvulnerability(Damagetype.Heavy);

        ParticlesManager.Instance.GetParticlePool(armorHitParticle.name, armorHitParticle);
        AudioManager.instance.GetSoundPool(armorHitSound.name, AudioGroups.GAME_FX, armorHitSound);
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
        armoredObject.AddInmuneFeedback(InvulnerabilityFeedback);
        StopAllCoroutines();
        armor.gameObject.SetActive(true);
        armor.OnReset();
    }

    IEnumerator DelayedBrokeArmor()
    {
        yield return new WaitForSeconds(1);
        armoredObject.RemoveInvulnerability(Damagetype.Heavy);
        armoredObject.RemoveInvulnerability(Damagetype.Normal);
        armoredObject.RestInmuneFeedback(InvulnerabilityFeedback);
    }

    void InvulnerabilityFeedback()
    {
        ParticlesManager.Instance.PlayParticle(armorHitParticle.name, transform.position);
        AudioManager.instance.PlaySound(armorHitSound.name);
    }
}

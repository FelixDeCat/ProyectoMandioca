using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AssassinRay : MonoBehaviour
{
    [SerializeField] ParticleSystem rayParticle = null;
    [SerializeField] AudioClip groundHit = null;
    [SerializeField] AudioClip lightningStrike = null;

    private void Start()
    {
        ParticlesManager.Instance.GetParticlePool(rayParticle.name, rayParticle);
        AudioManager.instance.GetSoundPool(groundHit.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, groundHit);
        AudioManager.instance.GetSoundPool(lightningStrike.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, lightningStrike);
    }

    public void Attack()
    {
        ParticlesManager.Instance.PlayParticle(rayParticle.name, transform.position);
        AudioManager.instance.PlaySound(lightningStrike.name, transform);
        AudioManager.instance.PlaySound(groundHit.name, transform);
        var overlap = Physics.OverlapSphere(transform.position, 15).Where(x => x.GetComponent<RagdollComponent>()).Where(x => !x.GetComponent<EnemyBase>())
            .Select(x => x.GetComponent<RagdollComponent>());

        foreach (var item in overlap)
        {
            item.ActivateRagdoll((item.transform.position - transform.position), () => Destroy(item.gameObject));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCW : SkillBase
{
    [SerializeField] float cdToExplosion = 8;
    [SerializeField] float radiousExp = 8;
    [SerializeField] int damageExp = 7;
    float timer;

    CharacterHead head;
    [SerializeField] ParticleSystem explosionParticles = null;

    bool buffActived;

    protected override void OnBeginSkill()
    {
        Debug.Log("ESNTRO A LA SKILL");

        if (head == null)
        {
            head = Main.instance.GetChar();
            explosionParticles.transform.position = head.transform.position;
            explosionParticles.transform.SetParent(head.transform);
        }

        head.ChangeWeaponPassives += IsExplosion;
    }

    void IsExplosion()
    {
        if (!buffActived)
            Explosion();
    }

    protected override void OnEndSkill()
    {
        head.ChangeWeaponPassives -= IsExplosion;
    }

    void Explosion()
    {
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.Play();

        var radiousToExplosion = Physics.OverlapSphere(head.transform.position, radiousExp);

        foreach (var item in radiousToExplosion)
        {
            if (item.GetComponent<EnemyBase>())
            {
                Debug.Log("0: Damage explosion: " + damageExp);

                item.GetComponent<EnemyBase>().TakeDamage(damageExp, (item.transform.position - head.transform.position).normalized,  Damagetype.explosion, head);
            }
        }

        buffActived = true;
    }

    protected override void OnUpdateSkill()
    {
        if (buffActived)
        {
            timer += Time.deltaTime;

            if (timer >= cdToExplosion)
            {
                EndCD();
            }
        }
    }

    void EndCD()
    {
        timer = 0;
        explosionParticles.Stop();
        explosionParticles.gameObject.SetActive(false);
        buffActived = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreDamageCW : SkillBase
{
    [SerializeField] float buffDamage = 5;
    [SerializeField] float timeToBuff = 3;
    float timer;

    CharacterHead head;

    [SerializeField] ParticleSystem feedbackParticle = null;

    bool buffActived;
    protected override void OnBeginSkill()
    {
        if (head == null)
        {
            head = Main.instance.GetChar();
            feedbackParticle.transform.position = head.transform.position;
            feedbackParticle.transform.SetParent(head.transform);
        }

        head.ChangeWeaponPassives += MoreDamage;
    }

    void MoreDamage()
    {
        if (!buffActived)
        {
            head.ChangeDamage(buffDamage);
            feedbackParticle.gameObject.SetActive(true);
            feedbackParticle.Play();
            buffActived = true;
        }
    }

    protected override void OnEndSkill()
    {
        head.ChangeWeaponPassives -= MoreDamage;
        LessDamage();
    }

    protected override void OnUpdateSkill()
    {
        if (buffActived)
        {
            timer += Time.deltaTime;

            if (timer >= timeToBuff)
                LessDamage();
        }
    }

    void LessDamage()
    {
        if (buffActived)
        {
            head.ChangeDamage(-buffDamage);
            timer = 0;
            feedbackParticle.Stop();
            feedbackParticle.gameObject.SetActive(false);
            buffActived = false;
        }
    }
}

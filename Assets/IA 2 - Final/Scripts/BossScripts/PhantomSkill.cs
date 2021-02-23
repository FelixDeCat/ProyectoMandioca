using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomSkill : BossSkills
{
    ThrowData data = new ThrowData();
    [SerializeField] ParticleSystem appearParticle = null;
    [SerializeField] BossProjectile projectile = null;
    [SerializeField] Transform model = null;
    [SerializeField] float projectileSpeed = 2;
    [SerializeField] int projectileDamage = 4;
    [SerializeField] int itteration = 6;
    [SerializeField] float timeToItteration = 1;
    [SerializeField] float distanceToChar = 5;
    Transform target;
    Vector3 firstPos;

    List<string> probPosition;
    string lastPosition = "Forward";

    public override void Initialize()
    {
        base.Initialize();
        data.Owner = model;
        data.Force = projectileSpeed;
        data.Damage = projectileDamage;
        ThrowablePoolsManager.instance.CreateAPool(projectile.name, projectile);
        ParticlesManager.Instance.GetParticlePool(appearParticle.name, appearParticle);
        target = Main.instance.GetChar().transform;
        probPosition = new List<string>() { "Right", "Left", "Backward", "Forward" };
    }

    protected override void OnUseSkill()
    {
        firstPos = model.position;
        StartCoroutine(PhantomItterator());
    }

    IEnumerator PhantomItterator()
    {
        for (int i = 0; i < itteration; i++)
        {
            probPosition.Remove(lastPosition);
            int index = Random.Range(0, probPosition.Count);
            model.position = PosSwitcher(probPosition[index]);
            probPosition.Add(lastPosition);
            lastPosition = probPosition[index];
            ParticlesManager.Instance.PlayParticle(appearParticle.name, model.position);
            model.forward = (target.position - model.position).normalized;
            data.Position = model.position;
            data.Direction = model.forward;
            ThrowablePoolsManager.instance.Throw(projectile.name, data);
            yield return new WaitForSeconds(timeToItteration);
        }

        OverSkill();
    }

    protected override void OnInterruptSkill()
    {
    }

    protected override void OnOverSkill()
    {
        model.position = firstPos;
        ParticlesManager.Instance.PlayParticle(appearParticle.name, model.position);
    }

    Vector3 PosSwitcher(string posName)
    {
        if (posName == "Right") return target.position + target.right * distanceToChar;
        else if (posName == "Forward") return target.position + target.forward * distanceToChar;
        else if (posName == "Left") return target.position - target.right * distanceToChar;
        else if (posName == "Backward") return target.position - target.forward * distanceToChar;

        return Vector3.zero;
    }
}

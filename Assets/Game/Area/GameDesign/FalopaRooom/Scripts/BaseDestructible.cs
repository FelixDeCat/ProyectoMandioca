using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DamageReceiver), typeof(_Base_Life_System))]
public abstract class BaseDestructible : Environment
{
    [SerializeField] protected DestroyedVersion model_destroyedVersion = null;
    protected DestroyedVersion savedDestroyedVersion;

    [SerializeField] protected DamageReceiver damageReceiver = null;
    [SerializeField] protected _Base_Life_System _lifeSytstem = null;

    [SerializeField] AudioClip destroyedSound = null;

    [SerializeField] UnityEvent OnTakeDamage = null;
    [SerializeField] UnityEvent OnDestroyed = null;

    public bool NoInitialize = false;

    private void Start()
    {
        if (NoInitialize) return;     
    }

    protected override void OnInitialize()
    {
        _lifeSytstem.Initialize( _lifeSytstem.life, ()=> { }, () => { }, () => { });

        damageReceiver.AddDead((x) => { OnDestroyed.Invoke(); DestroyDestructible(); }).AddTakeDamage((x) => OnTakeDamage.Invoke()).Initialize(transform,
            GetComponent<Rigidbody>(),
            _lifeSytstem);

        AudioManager.instance.GetSoundPool(destroyedSound.name, AudioGroups.AMBIENT_FX, destroyedSound);
    }

    public void DestroyDestructible()
    {
        AudioManager.instance.PlaySound(destroyedSound.name, transform);
        OnDestroyDestructible();
    }
    protected abstract void OnDestroyDestructible();
    protected abstract void FeedbackDamage();

}
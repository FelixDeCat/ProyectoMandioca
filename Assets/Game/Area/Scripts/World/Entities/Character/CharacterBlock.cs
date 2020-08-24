using UnityEngine;
using System;

[System.Serializable]
public class CharacterBlock : EntityBlock
{
    [Header("Shield Model")]
    public GameObject shield;
    [Header ("Character Block Parameters")]
    [SerializeField] private int maxBlockCharges = 3;
    [SerializeField] private int currentBlockCharges = 3;
    [SerializeField] private float timeToRecuperate = 5f;
    [SerializeField] private float parryForceToKnockBack = 650;
    [SerializeField] ParticleSystem parryParticles = null;
    public float ParryForce { get => parryForceToKnockBack; }
    public int CurrentBlockCharges { get => currentBlockCharges; }
    public Action callback_OnBlock;
    public Action callback_UpBlock;
    public Action callback_OnParry;
    public Action callback_EndBlock;
    private CharacterAnimator anim;
    CharFeedbacks feedbacks;
    float timerCharges;

    #region Set
    public CharacterBlock Initialize()
    {
        callback_OnBlock += OnBlockDown;
        callback_UpBlock += OnBlockUp;
        callback_OnParry += FinishParry;
        return this;
    }
    public CharacterBlock SetFeedbacks(CharFeedbacks _feedbacks) { feedbacks = _feedbacks; return this; }
    public CharacterBlock SetAnimator(CharacterAnimator _anim) { anim = _anim; return this; }
    #endregion

    public override void OnBlockDown() { if(!base.OnBlock) anim.Block(true); Parry(); ParryFeedback(); }
    public override void OnBlockUp() { anim.Block(false); FinishParry(); }

    //por animacion
    public override void OnBlockSuccessful()
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!base.OnBlock && currentBlockCharges < maxBlockCharges)
        {
            timerCharges += Time.deltaTime;

            if (timerCharges >= timeToRecuperate)
            {
                SetBlockCharges(1);
                timerCharges = 0;
            }
        }
    }

    public void SetBlockCharges(int chargesAmmount)
    {
        currentBlockCharges += chargesAmmount;

        if(currentBlockCharges <= 0)
        {
            currentBlockCharges = 0;
            callback_EndBlock();
        }
        else if (currentBlockCharges >= maxBlockCharges)
        {
            currentBlockCharges = maxBlockCharges;
            timerCharges = 0;
        }

        Main.instance.gameUiController.shieldsController.RefreshUI(currentBlockCharges, maxBlockCharges);
    }

    public bool CanUseCharge() => currentBlockCharges > maxBlockCharges-1;


    void ParryFeedback()
    {
        parryParticles.Play();
    }

    public void SetOnBlock(bool b)
    {
        OnBlock = b;
        if (!b)
            FinishParry();
    }

    public override void FinishParry()
    {
        base.FinishParry();
        parryParticles.Stop();
    }
}

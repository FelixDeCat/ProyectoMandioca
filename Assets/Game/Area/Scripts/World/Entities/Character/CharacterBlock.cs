using UnityEngine;
using System;
using Tools.StateMachine;

public class CharacterBlock : EntityBlock
{
    public Action callback_OnBlock;
    public Action callback_UpBlock;
    public Action callback_OnParry;
    public Action callback_EndBlock;

    private CharacterAnimator anim;

    ParticleSystem parryParticles;

    Func<EventStateMachine<CharacterHead.PlayerInputs>> sm;


    public int CurrentBlockCharges { get; private set; }
    int maxBlockCharges;

    float timeToRecuperate;
    float timerCharges;

    UI_GraphicContainer ui;

    public CharacterBlock(float timeParry,
                          float blockRange,
                          float _timeToBlock,
                          int maxCharges,
                          float timeRecuperate,
                          GameObject _ui,
                          CharacterAnimator _anim,
                          Func<EventStateMachine<CharacterHead.PlayerInputs>> _sm,
                          ParticleSystem _parryParticles) : base(timeParry, blockRange)
    {
        anim = _anim;
        callback_OnBlock += OnBlockDown;
        callback_UpBlock += OnBlockUp;
        sm = _sm;
        parryParticles = _parryParticles;
        callback_OnParry += FinishParry;
        //timeBlock = _timeToBlock;
        maxBlockCharges = maxCharges;
        CurrentBlockCharges = maxCharges;
        timeToRecuperate = timeRecuperate;
   
    }

    public override void OnBlockDown() { if(!base.OnBlock) anim.Block(true); Parry(); ParryFeedback(); }
    public override void OnBlockUp() { anim.Block(false); FinishParry(); }

    //por animacion
    public override void OnBlockSuccessful()
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!base.OnBlock && CurrentBlockCharges < maxBlockCharges)
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
        CurrentBlockCharges += chargesAmmount;

        if(CurrentBlockCharges <= 0)
        {
            CurrentBlockCharges = 0;
            callback_EndBlock();
        }
        else if (CurrentBlockCharges >= maxBlockCharges)
        {
            CurrentBlockCharges = maxBlockCharges;
            timerCharges = 0;
        }

        Main.instance.gameUiController.shieldsController.RefreshUI(CurrentBlockCharges, maxBlockCharges);
    }

    public bool CanUseCharge() => CurrentBlockCharges > maxBlockCharges-1;

    void ParryFeedback()
    {
        parryParticles.Play();
    }

    public void SetOnBlock(bool b)
    {
        onBlock = b;
        if (!b)
            FinishParry();
    }

    public override void FinishParry()
    {
        base.FinishParry();
        parryParticles.Stop();
        anim.Parry(false);
    }
}

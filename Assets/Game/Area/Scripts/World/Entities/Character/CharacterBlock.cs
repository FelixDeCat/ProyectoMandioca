using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class CharacterBlock : EntityBlock
{
    public Action OnBlock;
    public Action UpBlock;
    public Action OnParry;

    public Action BeginParry;
    public Action EndBlock;

    CharacterAnimator anim;

    ParticleSystem parryParticles;

    Func<EventStateMachine<CharacterHead.PlayerInputs>> sm;

    float timeBlock;
    float timerToUpBlock;

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
        OnBlock += OnBlockDown;
        UpBlock += OnBlockUp;
        sm = _sm;
        parryParticles = _parryParticles;
        OnParry += FinishParry;
        BeginParry += Parry;
        BeginParry += ParryFeedback;
        timeBlock = _timeToBlock;
        maxBlockCharges = maxCharges;
        CurrentBlockCharges = maxCharges;
        timeToRecuperate = timeRecuperate;
        
        //Saque esto para poner los escudos 3d
        //var newUi = MonoBehaviour.Instantiate(_ui, Main.instance.gameUiController.MyCanvas.transform);
        //ui = newUi.GetComponentInChildren<UI_GraphicContainer>();
        //ui.OnValueChange(CurrentBlockCharges, maxBlockCharges);
    }

    public override void OnBlockDown() { if(!onBlock) anim.Block(true); }
    public override void OnBlockUp() { anim.Block(false); FinishParry(); timerToUpBlock = 0;  }

    //por animacion
    public override void OnBlockSuccessful()
    {
        BeginParry();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //if (onBlock)
        //{
        //    timerToUpBlock += Time.deltaTime;

        //    if (timerToUpBlock >= timeBlock)
        //    {
        //        EndBlock();
        //    }
        //}

        if (!onBlock && CurrentBlockCharges < maxBlockCharges)
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
            EndBlock();
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

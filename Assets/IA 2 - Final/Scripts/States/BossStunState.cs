using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA2Final.FSM
{
    public class BossStunState : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        bool inUp;
        bool inDown;
        bool goDown;
        bool goUp;

        float timer;
        float timeInDown;
        float maxY;
        float minY;
        float speed;

        BossModel model;

        public BossStunState(float _timeInDown, float _minY, float _maxY, float _speed, BossModel _model)
        {
            timeInDown = _timeInDown;
            minY = _minY;
            maxY = _maxY;
            speed = _speed;
            model = _model;
        }

        public override void UpdateLoop()
        {
            if (goDown)
            {
                model.transform.position -= model.transform.up * speed * Time.deltaTime;

                if(model.transform.localPosition.y <= minY)
                {
                    model.transform.position = new Vector3(model.transform.position.x, minY, model.transform.position.z);
                    goDown = false;
                    inDown = true;
                }
            }
            else if (goUp)
            {
                model.transform.position += model.transform.up * speed * Time.deltaTime;

                if (model.transform.localPosition.y >= maxY)
                {
                    model.transform.position = new Vector3(model.transform.position.x, maxY, model.transform.position.z);
                    goUp = false;
                    inUp = true;
                }
            }

            if (inDown)
            {

                timer += Time.deltaTime;
                if(timer >= timeInDown)
                {
                    timer = 0;
                    inDown = false;
                    goUp = true;
                }
            }
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            goDown = true;
            model.RestartStamina();
            base.Enter(from, transitionParameters);
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            inUp = false;
            timer = 0;
            inDown = false;
            goUp = false;
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            if (inUp) OnNeedsReplan();
            return this;
        }
    }
}

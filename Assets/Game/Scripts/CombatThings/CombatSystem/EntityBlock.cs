using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBlock
{
    public bool onBlock;
    protected bool onParry;
    protected float timeToParry;
    protected float timer;

    //Mucho muy importante que sea de -1 a 1
    float blockAngle;

    public EntityBlock(float timeParry, float blockRange)
    {
        timeToParry = timeParry;
        blockAngle = blockRange;
    }

    public virtual void OnBlockDown() { }
    public virtual void OnBlockUp() { }
    public virtual void OnBlockSuccessful() => onBlock = true;
    public virtual void OnBlockUpSuccessful() => onBlock = false;

    public virtual bool IsParry(Vector3 mypos, Vector3 attackPos, Vector3 myForward)
    {
        if (onParry)
        {
            Vector3 attackDir = mypos - attackPos;
            attackDir.Normalize();

            float blockRange = Vector3.Dot(myForward, attackDir);

            if (blockRange <= blockAngle)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public virtual bool IsBlock(Vector3 mypos, Vector3 attackPos, Vector3 myForward)
    {
        if (onBlock)
        {
            Vector3 attackDir = mypos - attackPos;
            attackDir.Normalize();

            float blockRange = Vector3.Dot(myForward, attackDir);

            if (blockRange <= blockAngle)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public virtual void Parry()
    {
        if (!onParry)
        {
            onParry = true;
        }
    }

    public virtual void OnUpdate()
    {
        if (onParry)
        {
            timer += Time.deltaTime;
            if (timer >= timeToParry)
            {
                FinishParry();
            }
        }
    }

    public virtual void FinishParry()
    {
        onParry = false;
        timer = 0;
    }
}

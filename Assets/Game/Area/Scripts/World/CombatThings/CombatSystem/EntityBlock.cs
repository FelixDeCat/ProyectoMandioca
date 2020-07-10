using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityBlock
{
    [Header("Entity Block Parameters")]
    [SerializeField] [Range(0, 2)] float timeToParry;
    [SerializeField] [Range(-1, 1)] float blockAngle;

    private float timer;
    private bool onParry;
    private bool onBlock;
    public bool OnBlock { get => onBlock; set => onBlock = value; }

    public EntityBlock() { }

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
        if (OnBlock)
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

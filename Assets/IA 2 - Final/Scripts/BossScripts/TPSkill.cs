using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSkill : BossSkills
{
    Transform target;
    [SerializeField] TrueCaronteHand hand = null;
    [SerializeField] float handSpeed = 4;
    [SerializeField] float minDistance = 4;
    [SerializeField] float maxDistance = 13;
    [SerializeField] Transform model = null;
    [SerializeField] LayerMask wallLayer = 1 << 0;
    bool moving = false;
    Vector3 initPos;

    public override void Initialize()
    {
        base.Initialize();
        hand.gameObject.SetActive(true);
        hand.WallCollision += () => { moving = false; OverSkill(); };
        hand.gameObject.SetActive(false);
        target = Main.instance.GetChar().Root;
    }

    protected override void OnUseSkill()
    {
        hand.gameObject.SetActive(true);
        hand.transform.position = new Vector3(model.position.x, hand.transform.position.y, model.position.z);
        hand.transform.forward = CalculeBestDirection();
        initPos = hand.transform.position;
        moving = true;
        StartCoroutine(HandMove());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (moving)
        {
            hand.transform.position += hand.transform.forward * handSpeed * Time.deltaTime;
            if (Vector3.Distance(hand.transform.position, initPos) >= maxDistance) { moving = false; OverSkill(); }
        }

    }

    IEnumerator HandMove()
    {
        while (moving)
        {
            hand.transform.position += hand.transform.forward * handSpeed * Time.deltaTime;
            if (Vector3.Distance(hand.transform.position, initPos) >= maxDistance) { moving = false; }
            yield return new WaitForSeconds(0.01f);
        }

        OverSkill();
    }

    protected override void OnInterruptSkill()
    {
        hand.transform.position = initPos;
    }

    protected override void OnOverSkill()
    {
        moving = false;
        model.transform.position = new Vector3(hand.transform.position.x, model.position.y, hand.transform.position.z);
        hand.gameObject.SetActive(false);
    }

    Vector3 CalculeBestDirection()
    {
        Vector3 playerDir = target.forward;
        Vector3 dirToPlayer = (target.position - model.position).normalized;
        playerDir.y = 0;
        dirToPlayer.y = 0;

        Vector3 firstTest = -dirToPlayer;

        if (!Physics.Raycast(model.position, firstTest, minDistance, wallLayer, QueryTriggerInteraction.Ignore)){ return firstTest;}
        return dirToPlayer;
    }
}
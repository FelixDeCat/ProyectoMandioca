using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehavioursManager : MonoBehaviour
{
    [Header("Base Variables")]
    Transform root;
    Rigidbody rb;
    Transform target;

    [Header("Behaviours")]
    public FollowBehaviour followBehaviour;
    public AnimationPositionCapture animationPostionCapture;

    public void InitializeBehaviours(Transform root, Rigidbody rb)
    {
        this.root = root;
        this.rb = rb;
        target = Main.instance.GetChar().transform;
        followBehaviour.ConfigureFollowBehaviour(root,rb,target);
    }

    public void ChangeTarget(Transform newtarget)
    {
        target = newtarget;
        followBehaviour.ChangeTarget(newtarget);
    }
}

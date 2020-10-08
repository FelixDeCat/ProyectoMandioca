using UnityEngine;
public class Piston : MonoBehaviour
{
    [Header("Posiciones")]
    [SerializeField] Transform _startPos = null;
    [SerializeField] Transform _EndPos = null;
    [SerializeField] Transform ToMove = null;
    [Header("General configs")]
    [SerializeField] protected float delayToBegin = 1f;
    [Header("General configs")]
    [SerializeField] float speed = 1f;
    [SerializeField] protected float stay_position_time = 1f;
    [Header("Go configs")]
    [SerializeField] protected float staypositiontime_go = 1f;
    [SerializeField] protected float speed_go_multiplier = 1f;
    [Header("Back configs")]
    [SerializeField] protected float staypositiontime_back = 1f;
    [SerializeField] protected float speed_back_multiplier = 1f;
    [SerializeField] protected AudioClip ac_hitFloor;

    [Header("Anim by Code")]
    public bool Anim = true;
       
    protected PingPongLerp pingponglerp;
    public virtual void Start()
    {
        AudioManager.instance.GetSoundPool(ac_hitFloor.name, AudioGroups.GAME_FX, ac_hitFloor);
        if (Anim)
        {
            pingponglerp = new PingPongLerp();
            pingponglerp.Configure(AnimationResult, true, true, stay_position_time);

            pingponglerp.ConfigureSpeedsMovements(speed_go_multiplier, speed_back_multiplier);
            pingponglerp.ConfigueTimeStopsSides(staypositiontime_go, staypositiontime_back);

            Invoke("BeginAnimation", delayToBegin);
        }
    }

    void BeginAnimation()
    {
        pingponglerp.Play(speed);
    }

    protected virtual void hitFloorFeedBack()
    {
        AudioManager.instance.PlaySound(ac_hitFloor.name, transform);
    }
    public void AnimationResult(float val_anim) => ToMove.position = Vector3.Lerp(_startPos.position, _EndPos.position, val_anim);
    protected virtual void Update() =>  pingponglerp?.Updatear();
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_startPos.position, _EndPos.position);
    }
}

using UnityEngine;
public class Piston : MonoBehaviour
{
    [Header("Posiciones")]
    [SerializeField] Transform _startPos = null;
    [SerializeField] Transform _EndPos = null;
    [SerializeField] Transform ToMove = null;
    [Header("General configs")]
    [SerializeField] float delayToBegin = 1f;
    [Header("General configs")]
    [SerializeField] float speed = 1f;
    [SerializeField] float stay_position_time = 1f;
    [Header("Go configs")]
    [SerializeField] float staypositiontime_go = 1f;
    [SerializeField] float speed_go_multiplier = 1f;
    [Header("Back configs")]
    [SerializeField] float staypositiontime_back = 1f;
    [SerializeField] float speed_back_multiplier = 1f;

    [Header("Anim by Code")]
    public bool Anim = true;



    PingPongLerp pingponglerp;
    void Start()
    {
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

    public void AnimationResult(float val_anim) => ToMove.position = Vector3.Lerp(_startPos.position, _EndPos.position, val_anim);
    void Update() =>  pingponglerp?.Updatear();
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_startPos.position, _EndPos.position);
    }
}

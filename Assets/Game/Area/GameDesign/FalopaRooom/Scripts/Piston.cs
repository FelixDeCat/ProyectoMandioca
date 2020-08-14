using UnityEngine;
public class Piston : MonoBehaviour
{
    [Header("Posiciones")]
    [SerializeField] Transform _startPos = null;
    [SerializeField] Transform _EndPos = null;
    [SerializeField] Transform ToMove;

    [Header("Posiciones")]
    [SerializeField] float speed = 1f;
    [SerializeField] float stay_position_time = 1f;
    PingPongLerp pingponglerp;
    void Start()
    {
        pingponglerp = new PingPongLerp();
        pingponglerp.Configure(AnimationResult, true, true, stay_position_time);
        pingponglerp.Play(speed);
    }
    public void AnimationResult(float val_anim) => ToMove.position = Vector3.Lerp(_startPos.position, _EndPos.position, val_anim);
    void Update() => pingponglerp.Updatear();
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_startPos.position, _EndPos.position);
    }
}

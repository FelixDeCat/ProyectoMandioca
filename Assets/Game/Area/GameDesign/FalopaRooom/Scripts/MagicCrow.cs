using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCrow : MonoBehaviour
{

    [SerializeField] CharacterHead myHero;
    [SerializeField]  Waves magicBullet_pf;
    [SerializeField] CastingBar castingbar;
    [SerializeField] Transform shooter;

    [SerializeField] ParticleSystem hit;

    [SerializeField] float castCD;
   
    private void Start()
    {
        myHero = Main.instance.GetChar();
        castingbar.AddEventListener_OnFinishCasting(() => StartCoroutine(CDtoCast()));
        castingbar.AddEventListener_OnFinishCasting(OnFinishCast);

    }

    void OnFinishCast()
    {
        
        Vector3 currentPlayerPos = myHero.transform.position;
        shooter.LookAt(currentPlayerPos + Vector3.up*0.25f);
        var b = Instantiate<Waves>(magicBullet_pf);
        b.SetLifeTime(5).SetSpeed(2).SetSpeed(3).SetSpawner(shooter.gameObject);
        b.transform.position = shooter.transform.position;
        b.transform.rotation = shooter.transform.rotation;
       
    }

    IEnumerator CDtoCast()
    {
        yield return new WaitForSeconds(castCD);
        Activate();
    }

    public void Activate()
    {
        castingbar.StartCasting(3);
    }

    public void DeathFeedback()
    {
        hit.Play();
    }

    public void Deactivate()
    {
        castingbar.InterruptCasting();
    }

}

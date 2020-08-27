using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCrow : MonoBehaviour
{

    CharacterHead myHero;
    [SerializeField]  Throwable magicBullet_pf;
    [SerializeField] CastingBar castingbar;
    [SerializeField] Transform shooter;
    [SerializeField] Transform model;
    [SerializeField] float bulletSpeed;
    [SerializeField] ParticleSystem feedbackCast;

    ThrowData tData;

    [SerializeField] ParticleSystem hit;

    [SerializeField] float castCD;
   
    private void Start()
    {
        myHero = Main.instance.GetChar();
        castingbar.AddEventListener_OnFinishCasting(() => StartCoroutine(CDtoCast()));
        castingbar.AddEventListener_OnFinishCasting(OnFinishCast);

        
        ThrowablePoolsManager.instance.CreateAPool("bala magica", magicBullet_pf); 
    }

    void OnFinishCast()
    {
        feedbackCast.Stop();
        tData = new ThrowData();
        Vector3 currentPlayerPos = myHero.transform.position;
        tData.Configure(shooter.transform.position, (currentPlayerPos - shooter.transform.position).normalized, bulletSpeed, 2, transform);

        ThrowablePoolsManager.instance.Throw("bala magica", tData);

    }

    IEnumerator CDtoCast()
    {
        yield return new WaitForSeconds(castCD);
        Activate();
    }

    public void Activate()
    {
        castingbar.StartCasting(3);
        feedbackCast.Play();

    }

    public void LookPlayer()
    {
        model.LookAt(new Vector3(myHero.transform.position.x, 0, myHero.transform.position.z));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenericLifeSystem : MonoBehaviour
{
    LifeSystemBase lifeSystemEnemy;


    [SerializeField] protected WorldCanvasPopUp lifeBar;
    [SerializeField] protected RectTransform canvas;
    FrontendStatBase uilife;

    public int life = 100;

    bool isdeath;

    public event Action DeadCallback = delegate { };

    public void AddEventOnDeath(Action listener) { DeadCallback += listener; }
    public void RemoveEventOnDeath(Action listener) { DeadCallback -= listener; DeadCallback = delegate { }; }

    private void Start()
    {
        CreateLifeBar();
    }

    protected void CreateLifeBar()
    {
        var tempBar = Instantiate(lifeBar, canvas.transform);
        tempBar.SetCanvasPopUp(transform, canvas);
        uilife = tempBar.GetComponent<LifeBar>();
        lifeSystemEnemy = new LifeSystemBase();
        lifeSystemEnemy.Config(life, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, uilife, life);
    }



    void EVENT_OnLoseLife() { }
    void EVENT_OnGainLife() { }
    void EVENT_OnDeath()
    {
        if (!isdeath)
        {
            DeadCallback.Invoke();
            DeadCallback = delegate { };
            isdeath = true;
        }
    }

    public bool Hit(int _val)
    {
        return lifeSystemEnemy.Hit(_val);
    }

    
    public void DoTSystem(float duration, float timePerTick, int tickDamage, Damagetype damagetype, Action onFinishCallback )
    {
        StartCoroutine(DoT(duration, timePerTick, tickDamage, damagetype, onFinishCallback));
    }

    IEnumerator DoT(float duration, float timePerTick, int tickDamage, Damagetype damagetype, Action onFinishCallback)
    {
        float countTime = 0;
        float tickTimeCount = 0;
        
        while (countTime <= duration)
        {
            countTime += Time.fixedDeltaTime;

            tickTimeCount += Time.fixedDeltaTime;

            if (tickTimeCount >= timePerTick)
            {
                Hit(tickDamage);
                tickTimeCount = 0;
            }

            yield return null;
        }
        
        onFinishCallback.Invoke();
    }
}

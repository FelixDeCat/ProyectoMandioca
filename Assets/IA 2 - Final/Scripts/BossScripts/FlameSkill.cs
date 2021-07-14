using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlameSkill : BossSkills
{
    [Serializable]
    public class FlameData
    {
        public float timeToTick = 1;
        [Range(5, 15)] public int flamesAmmount = 5;
        public int roundsAmmount = 4;
        public float flameSeparation;
    }

    [SerializeField] FlameData circularData = new FlameData();
    [SerializeField] FlameData directionalData = new FlameData();
    [SerializeField] FireColumn flamePrefab = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AnimEvent animEvent = null;

    [SerializeField] AudioClip flameStartSound = null;
    ObjectPool_PlayObject myPool;
    Transform target;
    List<Action> modeSelector;

    public override void Initialize()
    {
        base.Initialize();

        myPool = PoolManager.instance.GetObjectPool(flamePrefab.name, flamePrefab);
        target = Main.instance.GetChar().Root;
        modeSelector = new List<Action>() { () => Itteration(directionalData.flamesAmmount,directionalData.timeToTick, DirectionalMode, OverSkill),
            () => Itteration(circularData.flamesAmmount,circularData.timeToTick, CircularMode, OverSkill) };
        AudioManager.instance.GetSoundPool(flameStartSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, flameStartSound);
    }

    void Callback()
    {
        int index = UnityEngine.Random.Range(0, modeSelector.Count);
        modeSelector[index].Invoke();
    }

    protected override void OnUseSkill()
    {
        AudioManager.instance.PlaySound(flameStartSound.name, transform);
        animEvent.Add_Callback("FlameSkill", Callback);
        anim.SetBool("OnFlame", true);
        anim.Play("FlameSkill");

    }

    protected override void OnInterruptSkill()
    {
    }

    protected override void OnOverSkill()
    {
        anim.SetBool("OnFlame", false);
        animEvent.Remove_Callback("FlameSkill", Callback);
    }

    IEnumerator SpawnFlames(FlameData data, Action spawnMode)
    {
        for (int i = 0; i < data.roundsAmmount; i++)
        {
            spawnMode.Invoke();
            yield return new WaitForSeconds(data.timeToTick);
        }

        OverSkill();
    }

    void CircularMode()
    {
        var centerFlame = myPool.GetPlayObject();
        centerFlame.transform.position = target.transform.position;
        var rightFlame = myPool.GetPlayObject();
        rightFlame.transform.position = target.transform.position + Vector3.right * circularData.flameSeparation;
        var leftFlame = myPool.GetPlayObject();
        leftFlame.transform.position = target.transform.position + Vector3.left * circularData.flameSeparation;
        var forwardFlame = myPool.GetPlayObject();
        forwardFlame.transform.position = target.transform.position + Vector3.forward * circularData.flameSeparation;
        var backwardFlame = myPool.GetPlayObject();
        backwardFlame.transform.position = target.transform.position + Vector3.back * circularData.flameSeparation;

        List<PlayObject> myFlames = new List<PlayObject>() { rightFlame, forwardFlame, leftFlame, backwardFlame };


        int currentIndex = 0;

        for (int i = 5; i < circularData.flamesAmmount; i++)
        {
            if (currentIndex >= myFlames.Count) currentIndex = 0;

            int nextIndex = currentIndex == myFlames.Count - 1 ? 0 : currentIndex + 1;

            var flame = myPool.GetPlayObject();
            flame.transform.position = Vector3.Lerp(myFlames[currentIndex].transform.position, myFlames[nextIndex].transform.position, 0.5f);
            myFlames.Insert(nextIndex, flame);
            currentIndex += 2;
        }

    }

    void DirectionalMode()
    {
        Vector3 referencePosition = target.position;
        Vector3 direction = target.forward;
        var initFlame = myPool.GetPlayObject();
        initFlame.transform.position = referencePosition;
        int flameCount = 0;

        for (int i = 1; i < directionalData.flamesAmmount; i++)
        {
            var flame = myPool.GetPlayObject();
            if (flameCount == 0)
            {
                referencePosition = referencePosition + direction * directionalData.flameSeparation;
                flame.transform.position = referencePosition;
                flameCount += 1;
            }
            else if(flameCount == 1)
            {
                flame.transform.position = referencePosition + (target.right - target.forward) * directionalData.flameSeparation/2;
                flameCount += 1;
            }
            else
            {
                flame.transform.position = referencePosition + (-target.right - target.forward) * directionalData.flameSeparation/2;
                flameCount = 0;
            }
        }

    }
}

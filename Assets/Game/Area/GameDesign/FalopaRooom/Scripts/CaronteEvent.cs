using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GOAP;
using UnityEngine.SceneManagement;


public class CaronteEvent : MonoBehaviour
{
    public static CaronteEvent instance; private void Awake() { if (instance == null) instance = this; }

    [Header("Settings")]
    //[SerializeField] GameObject carontePP = null;
    [SerializeField] LayerMask floor = 1 << 21;
    [SerializeField] SoulShard_controller ss_controller = null;
    //[SerializeField] CaronteHand hand_pf = null;
    [SerializeField] Ente caronte_pf = null;
    //[SerializeField] float delayedHand = 5;
    [SerializeField] float delayedCaronteSpawn = 5;
    [SerializeField] Transform caronteSpawnSpot = null;

    CaronteCinematic_Controller cinematic;

    public bool _spawnedHand = false;

    Ente caronte;
    CharacterHead character;

    public void Start()
    {
        character = Main.instance.GetChar();
        cinematic = GetComponent<CaronteCinematic_Controller>();
        cinematic.StartCinematic();
        cinematic.OnFinishCinematic += SpawnCaronte;

        character.ToggleAttack(false);
        character.ToggleBlock(false);
        character.ToggleShield(false);
        character.ToggleSword(false);
    }

    void SpawnCaronte()
    {
        character.Life.Heal(25);
        ss_controller.ReleaseShard();
        Navigation.instance.transform.position = character.transform.position;
        Navigation.instance.LocalizeNodes();
        character.GetCharMove().SetSpeed(character.GetCharMove().GetDefaultSpeed * .6f);
        caronte = GameObject.Instantiate<Ente>(caronte_pf, this.transform);
        WorldState.instance.ente = caronte;
        caronte.transform.position = caronteSpawnSpot.position;
        caronte.Initialize();
    }

    public void OnExitTheAqueronte()
    {
        character.ToggleAttack(true);
        character.ToggleBlock(true);
        character.ToggleShield(true);
        character.ToggleSword(true);

        GameLoop.instance.Resurrect(true);
    }

    #region aux func
    public Vector3 GetSurfacePos()
    {
        var pos = GetPosRandom(UnityEngine.Random.Range(8, 12), character.transform);
        pos.y += 20;

        RaycastHit hit;

        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, floor, QueryTriggerInteraction.Ignore))
            pos = hit.point;

        return pos;
    }
    Vector3 GetPosRandom(float radio, Transform t)
    {
        Vector3 min = new Vector3(t.position.x - radio, 0, t.position.z - radio);
        Vector3 max = new Vector3(t.position.x + radio, t.position.y, t.position.z + radio);
        return new Vector3(UnityEngine.Random.Range(min.x, max.x), t.position.y, UnityEngine.Random.Range(min.z, max.z));
    }
    #endregion

}

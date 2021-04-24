using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDestructible : MonoBehaviour
{
    [SerializeField] AudioClip destroyedSound = null;
    [SerializeField] DestroyedVersion destroyed_version = null;
    [SerializeField] Transform thunderImpactSpot = null;
    DestroyedVersion savedDestroyedVersion;

    private void Start()
    {
        if(destroyedSound) AudioManager.instance.GetSoundPool(destroyedSound.name, AudioGroups.AMBIENT_FX, destroyedSound);
        savedDestroyedVersion = Main.instance.GetSpawner().SpawnItem(destroyed_version.gameObject, transform).GetComponent<DestroyedVersion>();
        if (savedDestroyedVersion) savedDestroyedVersion.gameObject.SetActive(false);
    }

    public void BreakYourselfBaby()
    {
        if (destroyedSound) AudioManager.instance.PlaySound(destroyedSound.name, transform);
        if (savedDestroyedVersion)
        {
            savedDestroyedVersion.gameObject.SetActive(true);
            savedDestroyedVersion.transform.position = transform.position;
            savedDestroyedVersion.BeginDestroy();

            savedDestroyedVersion.ExplosionForce(thunderImpactSpot.position, 15, 4);
        }
        gameObject.SetActive(false);
    }
}
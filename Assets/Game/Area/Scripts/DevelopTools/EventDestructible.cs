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
        if (destroyedSound) AudioManager.instance.PlaySound(destroyedSound.name);
        if (savedDestroyedVersion)
        {
            savedDestroyedVersion.gameObject.SetActive(true);
            savedDestroyedVersion.transform.position = transform.position;
            savedDestroyedVersion.BeginDestroy();
        }
        var childs = savedDestroyedVersion.GetComponentsInChildren<Rigidbody>();

        if (savedDestroyedVersion.principalChild)
        {
            foreach (var c in childs)
            {
                Vector3 aux;
                if (c != savedDestroyedVersion.principalChild) aux = c.transform.position - savedDestroyedVersion.principalChild.transform.position;
                else aux = c.transform.position - thunderImpactSpot.position;
                aux.Normalize();
                c.AddForce(aux * 5, ForceMode.VelocityChange);
                c.AddTorque(aux);
            }
        }
        else
        {
            foreach (var c in childs)
            {
                var aux = c.transform.position - thunderImpactSpot.position;
                aux.Normalize();
                c.AddForce(aux * 15, ForceMode.VelocityChange);
                c.AddTorque(aux * 4);
            }
        }

        gameObject.SetActive(false);
    }
}
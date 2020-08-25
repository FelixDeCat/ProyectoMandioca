using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    [SerializeField] Waves _wave = null;
    [SerializeField] int speed = 5;
    [SerializeField] float lifeTime = 2;
    [SerializeField] float angleHorizontal = 0f;
    [SerializeField] float angleVertical = 0f;
    
    public void Spawn()
    {
        Vector3 center = transform.position + transform.forward * speed * lifeTime;

        int aux = Random.Range(0, 360);
        if(!onlyBorders) center += transform.up * Mathf.Sin(aux * Mathf.Deg2Rad) * Random.value *angleVertical + transform.right * Mathf.Cos(aux * Mathf.Deg2Rad) * Random.value * angleHorizontal;
        else center += transform.up * Mathf.Sin(aux * Mathf.Deg2Rad) * angleVertical + transform.right * Mathf.Cos(aux * Mathf.Deg2Rad) *angleHorizontal;

        var wave = Instantiate(_wave);
        wave.transform.position = transform.position;
        wave.transform.forward = (center - transform.position).normalized;
        wave = wave.setSpeed(speed).SetLifeTime(lifeTime);
    }

    [SerializeField] bool onlyBorders = false;

    [SerializeField] bool drawGizmos = false;
    [SerializeField] int ammount = 15;
    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        float rot = 360 / ammount;
        Vector3 aux = transform.position;
        for (int i = 0; i < ammount; i++)
        {
            float internalAngle = rot * i;

            Vector3 center = transform.forward * speed * lifeTime;
            if (i == 0)
                aux = center;
            center += transform.up * Mathf.Sin(internalAngle * Mathf.Deg2Rad) * angleVertical + transform.right * Mathf.Cos(internalAngle * Mathf.Deg2Rad) * angleHorizontal;

            Gizmos.DrawRay(transform.position, center);

            if (i == 0)
            {
                aux += transform.up * Mathf.Sin(rot * (ammount-1) * Mathf.Deg2Rad)* angleVertical + transform.right * Mathf.Cos(rot * (ammount - 1) * Mathf.Deg2Rad) * angleHorizontal;
                Gizmos.DrawRay(transform.position + aux, center - aux);
            }else
            {
                Gizmos.DrawRay(transform.position + aux, center - aux);
            }
            aux = center;
        }
    }
}

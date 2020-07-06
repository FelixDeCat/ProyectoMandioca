using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    [SerializeField] Waves _wave;
    [SerializeField] int speed;
    [SerializeField] float lifeTime;
    [SerializeField] float angle = 0f;
    [SerializeField] float range = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        Vector3 center = transform.position + transform.forward * range;

        int aux = Random.Range(0, 360);
        if(!onlyBorders) center += new Vector3(0, Mathf.Sin(aux * Mathf.Deg2Rad) * Random.value, Mathf.Cos(aux * Mathf.Deg2Rad) * Random.value) * angle;
        else center += new Vector3(0, Mathf.Sin(aux * Mathf.Deg2Rad), Mathf.Cos(aux * Mathf.Deg2Rad)) * angle;

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

            Vector3 center = transform.forward * range;
            if (i == 0)
                aux = center;
            center += new Vector3(0, Mathf.Sin(internalAngle * Mathf.Deg2Rad), Mathf.Cos(internalAngle * Mathf.Deg2Rad)) * angle;

            Gizmos.DrawRay(transform.position, center);

            if (i == 0)
            {
                aux += new Vector3(0, Mathf.Sin(rot * (ammount-1) * Mathf.Deg2Rad), Mathf.Cos(rot * (ammount - 1) * Mathf.Deg2Rad)) * angle;
                Gizmos.DrawRay(transform.position + aux, center - aux);
            }else
            {
                Gizmos.DrawRay(transform.position + aux, center - aux);
            }
            aux = center;
        }
    }
}

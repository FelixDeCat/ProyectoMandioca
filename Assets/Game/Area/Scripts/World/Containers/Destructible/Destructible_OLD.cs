using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible_OLD : DestructibleBase
{
    public List<GameObject> destroyedVersion;
    public float disp;

    public List<GameObject> posibles;
    //Aca insertar probabilidades 

    void Start()
    {

    }

    public void Rellenar()
    {
        if (posibles.Count > 0)
        {
            var random = Random.Range(0, 2);
            for (int i = 0; i < random; i++)
            {
                destroyedVersion.Add(posibles[Random.Range(0, posibles.Count - 1)]);
            }
        }
    }

    public void ReceiveDamage(int damage, Vector3 destinity, bool isenemy)
    {
        if (damage == -1)
        {
            foreach (var v in destroyedVersion)
            {
                var go = Instantiate(v, new Vector3(
                    Random.Range(destinity.x - disp, destinity.x + disp),
                    Random.Range(destinity.y - disp, destinity.y + disp),
                    Random.Range(destinity.z - disp, destinity.z + disp)), transform.rotation);
            }
        }
        else
        {
           //CompleteCameraController.instancia.Shake();
            foreach (var v in destroyedVersion)
            {
                var go = Instantiate(v, new Vector3(
                    Random.Range(transform.position.x - disp, transform.position.x + disp),
                    Random.Range(transform.position.y - disp, transform.position.y + disp),
                    Random.Range(transform.position.z - disp, transform.position.z + disp)), transform.rotation);
            }
            if (this.gameObject.GetComponent<Rigidbody>()) this.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100, transform.position, 20);
            Destroy(this.gameObject);
        }
    }



    protected override void FeedbackDamage() { }
    protected override void OnInitialize() { }
    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnUpdate() { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }

    protected override void OnDestroyDestructible()
    {
        
    }

    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype)
    {
        throw new System.NotImplementedException();
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using Random = UnityEngine.Random;


///////////////////////////////////////////////////////////////////////////////////
/*
    esto tiene el sistema viejo de drops que era... 
    calcular un maximo de items
    luego a cada item asignarle un lugar en el espacio muestral
    asignar un cursor
    e iba agegando los objetos a dropear
    luego los ponia en algun lugar alejado y los apagaba


*/
///////////////////////////////////////////////////////////////////////////////////
//public class Destructible_Normal : DestructibleBase
//{
//    [SerializeField] protected DestructibleData data = null;

//    public bool destroy;

//    [SerializeField] ParticleSystem dest_part = null;

//    Rigidbody onerig;
//    Vector3 dest;

//    [System.NonSerialized] public List<GameObject> objectsToDrop = new List<GameObject>();
    

//    protected override void OnInitialize()
//    {
//        base.OnInitialize();
//        Calculate();
//        Main.instance.AddEntity(this);        
//    }

//    void Calculate()
//    {
//        if(data.data.Count>0)
//            DropCalculate();
//        //esto lo hacemos en el principio y no cuando le pegamos para no generar esos picos de Procesamiento

//        savedDestroyedVersion = Main.instance.GetSpawner().SpawnItem(model_destroyedVersion.gameObject, transform).GetComponent<DestroyedVersion>();

//        if (savedDestroyedVersion) savedDestroyedVersion.gameObject.SetActive(false);
//    }

//    void DropCalculate()
//    {
//        //a todos les asigno su maximo y su minimo para ser recorrido con un cursor
//        int totalrate = 0;
//        for (int i = 0; i < data.data.Count; i++)
//        {
//            data.data[i].droprate.min = totalrate;
//            totalrate += data.data[i].rate;
//            data.data[i].droprate.max = totalrate - 1;
//        }

//        //obtengo la cantidad de objetos que voy a spawnear
//        int dropcant = UnityEngine.Random.Range(data.minDrop, data.maxDrop);
//        for (int i = 0; i < dropcant; i++)
//        {
//            int ratecursor = Random.Range(0, totalrate);
//            Item selected = null;
//            foreach (var itm in data.data)
//            {
//                if (ratecursor >= itm.droprate.min && ratecursor <= itm.droprate.max)
//                {
//                    selected = itm.item;
//                    break;
//                }
//            }
//            //if (!selected) throw new System.Exception("El ratecursor se fue a la mierda");
//            //esto lo hacemos en el principio y no cuando le pegamos para no generar esos picos de Procesamiento

//            var destinity = FindObjectOfType<ParentContainer>().transform.position;

//            var disp = 2;

//            Vector3 aux = new Vector3(
//                    Random.Range(destinity.x - disp, destinity.x + disp),
//                    Random.Range(destinity.y - disp, destinity.y + disp),
//                    Random.Range(destinity.z - disp, destinity.z + disp));

//            if (selected)
//                objectsToDrop.Add(Main.instance.GetSpawner().SpawnItem(selected, aux));
//        }

//        onerig = objectsToDrop[0].GetComponent<Rigidbody>();
//        objectsToDrop.ForEach(x => x.SetActive(false));
//    }

//    void Drop()
//    {
//        var dispercion = 0.5f;
//        dest_part.transform.position = this.transform.position;
//        dest_part.Play();


//        for (int i = 0; i < objectsToDrop.Count; i++)
//        {
//            if (objectsToDrop[i] != null)
//            {
//                objectsToDrop[i].SetActive(true);
//                objectsToDrop[i].transform.position = new Vector3(
//                    Random.Range(transform.position.x - dispercion, transform.position.x + dispercion),
//                    Random.Range(transform.position.y - dispercion, transform.position.y + dispercion),
//                    Random.Range(transform.position.z - dispercion, transform.position.z + dispercion));
//                objectsToDrop[i].GetComponent<ItemWorld>().OnAppearInScene();
//            }
//        }

//        if (savedDestroyedVersion)
//        {
//            savedDestroyedVersion.gameObject.SetActive(true);
//            savedDestroyedVersion.transform.position = transform.position;
//            savedDestroyedVersion.BeginDestroy();
//        }
//        //if (savedDestroyedVersion) savedDestroyedVersion.transform.position = transform.position;
//        //if (savedDestroyedVersion) savedDestroyedVersion.GetComponent<DestroyedVersion>().BeginDestroy();

//        var childs = savedDestroyedVersion.GetComponentsInChildren<Rigidbody>();

//        foreach (var c in childs)
//        {
//            var aux = c.transform.position - transform.position;
//            aux.Normalize();
//            c.AddForce(aux * 5, ForceMode.VelocityChange);
//        }
//        //onerig.AddExplosionForce(200, transform.localPosition, 50);
//    }
//    void OthersFeedbacks() { /*CompleteCameraController.instancia.Shake();*/ }

//    protected override void OnDestroyDestructible() 
//    {
//        Drop();
//        OthersFeedbacks();
//        if (destroy) {
//            Main.instance.RemoveEntity(this);
            
//            Destroy(this.gameObject); 
//        }
//    }

//    //////////////////////////////////////////////
//    protected override void FeedbackDamage() { }
    
//    protected override void OnTurnOn() { }
//    protected override void OnTurnOff() { }
//    protected override void OnUpdate() { }
//    protected override void OnFixedUpdate() { }
//    protected override void OnPause() { }
//    protected override void OnResume() { }

    
//}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI2D_Shields_controller : MonoBehaviour
{
    //[Header("Rotate settings")] 
    [HideInInspector] public float rotSpeed;
    [HideInInspector]public Vector3 v3;
    [HideInInspector]public bool pingPong = false;
    private float _count;
    private float timeToPong = 4; 
    
    //[SerializeField] private ParticleSystem shieldOn_ps;
    //[SerializeField] private float spacingHorizontal;
    //[SerializeField] private GameObject shield_pf;

    
    
    [SerializeField] private Transform[] pos;
    //[SerializeField] private Transform container;
    [SerializeField] private List<GameObject> currentShields = new List<GameObject>();
    
    
    private int prevShieldCount = 3;
    

    //Recorre los escudos y va prendiendo o apagando depende lo que le tiren
    public void RefreshUI(int current, int max)
    {
        if (prevShieldCount > current) // se fija si perdio o gano escudo
        {
            //perdi escudo
            for (int j = 0; j < currentShields.Count; j++)
            {
                if (current == j)
                {
                    currentShields[j].SetActive(false);
                    prevShieldCount = current;
                    break;
                }
            }
        }
        else
        {
            //gane escudo
            for (int j = 0; j < currentShields.Count; j++)
            {
                if (current - 1 == j)
                {
                    currentShields[j].SetActive(true);
                    prevShieldCount = current;
                    //shieldOn_ps.transform.position = currentShields[j].transform.position - Vector3.forward * 2;
                    //shieldOn_ps.Play();
                    break;
                }
            }
        }
    }

//    public void Init(int current, int max)
//    {
//        pos = new Transform[current];
//        
//        for (int i = 0; i < current; i++)
//        {
//            var newGO = Instantiate(shield_pf, container);
//            newGO.transform.position = pos[i].transform.position;
//            newGO.transform.rotation = pos[i].transform.rotation;
//            
//            //newGO.transform.localPosition = new Vector3(i * spacingHorizontal, newGO.transform.position.y, newGO.transform.position.z);
//            currentShields.Add(newGO);
//        }
//        
//        prevShieldCount = current;
//    }

    /// <summary>
    /// Escondes todos los shields. Esto puede ser un fade
    /// </summary>
    public void HideShields()
    {
        for (int i = 0; i < currentShields.Count; i++)
        {
            currentShields[i].SetActive(false);
        }
    }
    /// <summary>
    /// Muestra los shields que tenias
    /// </summary>
    public void ShowShields()
    {
        RefreshUI(prevShieldCount, 3);
    }

    private void Update()
    {
        RotateShields();
        
        PingPong();
        
    }

    #region Rotaciones que a nadie le gustan

    void RotateShields()
    {
        Vector3 aux = new Vector3(v3.x * rotSpeed, v3.y * rotSpeed, v3.z * rotSpeed);
        
        foreach (GameObject t in currentShields)
        {
            t.transform.Rotate(aux);
        }
    }
    
    void PingPong()
    {
        if (pingPong)
        {
            _count += Time.deltaTime;

            if (_count >= timeToPong)
            {
                _count = 0;
                v3 = v3 * -1;
            }
        }
    }
    

    #endregion

    
}

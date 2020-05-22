using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI3D_Shields_controller : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private float spacingHorizontal;
    [SerializeField] private GameObject shield_pf;

    [Header("Rotate settings")] 
    public float rotSpeed;
    public Vector3 v3;
    public bool pingPong = false;
    private float _count;
    [SerializeField] private float timeToPong; 

        [SerializeField] private List<GameObject> currentShields = new List<GameObject>();

    [SerializeField] private ParticleSystem shieldOn_ps;
    private int prevShieldCount;

    
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
                    shieldOn_ps.transform.position = currentShields[j].transform.position - Vector3.forward * 2;
                    shieldOn_ps.Play();
                    break;
                }
            }
        }
    }

    public void Init(int current, int max)
    {
        for (int i = 0; i < current; i++)
        {
            var newGO = Instantiate(shield_pf, container);
            newGO.transform.localPosition = new Vector3(i * spacingHorizontal, newGO.transform.position.y, newGO.transform.position.z);
            currentShields.Add(newGO);
        }
        
        prevShieldCount = current;
    }

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
}

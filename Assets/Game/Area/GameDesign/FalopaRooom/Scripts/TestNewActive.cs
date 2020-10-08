using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNewActive : SpawnWaves
{
    float countShoot;
    float countOrb;
    [SerializeField] float cdShoot = 0.5f;
    [SerializeField] float cdOrb = 1;
    bool canShoot;
    bool canOrb;
    [SerializeField] GameObject bolita;
    public int maxOrbs = 2;

    void Update()
    {
        if (canShoot && Input.GetKey(KeyCode.Joystick1Button5))
        {
            Spawn();
            canShoot = false;
            countShoot = 0;
            Main.instance.GetChar().SetSlow();
        }

        if (Input.GetKeyUp(KeyCode.Joystick1Button5))
            Main.instance.GetChar().SetNormalSpeed();

        if(canOrb && Input.GetKeyDown(KeyCode.Joystick1Button0) && maxOrbs > 0)
        {
            maxOrbs--;
            GameObject orb = GameObject.Instantiate(bolita);
            orb.transform.position = transform.position;
            canOrb = false;
            countOrb = 0;
        }

        if (canShoot == false)
        {
            countShoot += Time.deltaTime;

            if (countShoot >= cdShoot)
                canShoot = true;
        }

        if (canOrb == false)
        {
            countOrb += Time.deltaTime;

            if (countOrb >= cdOrb)
                canOrb = true;
        }
    }
}

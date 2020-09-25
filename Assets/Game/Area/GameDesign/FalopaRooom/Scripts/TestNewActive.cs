using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNewActive : SpawnWaves
{
    float count;
    [SerializeField] float cd = 0.5f;
    bool canShoot;

    void Update()
    {
        if (canShoot && Input.GetKey(KeyCode.Joystick1Button5))
        {
            Spawn();
            canShoot = false;
            count = 0;
            Main.instance.GetChar().SetSlow();
        }

        if (Input.GetKeyUp(KeyCode.Joystick1Button5))
            Main.instance.GetChar().SetNormalSpeed();

        if (canShoot == false)
        {
            count += Time.deltaTime;

            if (count >= cd)
                canShoot = true;
        }
    }
}

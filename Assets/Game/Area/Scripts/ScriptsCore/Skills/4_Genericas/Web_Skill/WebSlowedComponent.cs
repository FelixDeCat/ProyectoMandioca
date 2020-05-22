using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSlowedComponent : MonoBehaviour
{
    private EnemyBase _dummyEnemy;

    private float _originalSpeed;
    void Awake()
    {
        _dummyEnemy = GetComponent<EnemyBase>();

        _originalSpeed = _dummyEnemy.ChangeSpeed(-1);
    }

    /// <summary>
    /// Le paso un porcentaje para que reduzca la velocidad el enemigo
    /// </summary>
    /// <param name="percentDecrease"></param>
    public void DecreaseSpeed(float percentDecrease)
    {
        float newSpeedCalculation = _originalSpeed - (_originalSpeed * percentDecrease); 
        _dummyEnemy.ChangeSpeed(newSpeedCalculation);
    }

    /// <summary>
    /// Vuelve a su velocidad normal
    /// </summary>
    public void ReturnSpeedToOriginal()
    {
        _dummyEnemy.ChangeSpeed(_originalSpeed);
    }
    
}

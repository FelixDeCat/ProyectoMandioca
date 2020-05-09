using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponenet : MonoBehaviour
{
    [System.Serializable]
    public class MoveOptions
    {
        [Header("transform parent de rotacion")]
        [SerializeField] Transform rootTransform = null;
        [Header("Rotacion")]
        [SerializeField] float rotSpeed = 2;
        [Header("Movimiento")]
        [SerializeField] float speedMovement = 4;
        [Header("Avoidance")]
        [SerializeField] float avoidWeight = 2;
        [SerializeField] float avoidRadious = 2;

        private float currentSpeed = 0;

        public float GetRotationSpeed() => rotSpeed;
        public Transform GetRootTransform() => rootTransform;
        public float GetOriginalSpeed() => speedMovement;
        public Vector3 GetMyPosition() => rootTransform.transform.position;

        //current speed functions
        public float GetCurrentSpeed() => currentSpeed;
        public void SetCurrentSpeed(float _value) => currentSpeed = _value;
        public void MultiplyCurrentSpeed(float _value) => currentSpeed *= _value;
        public void AddCurrentSpeed(float _value) => currentSpeed += _value;
        public void DivideCurrentSpeed(float _value) => currentSpeed /= _value;
        public float ResetSpeedToOriginal() => currentSpeed = speedMovement;
    }
    public MoveOptions moveOptions = new MoveOptions();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoBrakeSystem : MonoBehaviour
{
    const int MAX_BRAKE = 17000;
    const int BRAKE_COEFFCIENT = 15;
    private WheelCollider _forward;
    private WheelCollider _rear;
    private IVelocityController _velocityController;

    [Header("Current brake velues")]
    [SerializeField, Range(0, MAX_BRAKE)] float _forwardBrake;
    [SerializeField, Range(0, MAX_BRAKE)] float _rearBrake;
    private float _brakeInput;
    private float _speedCoefficient;

    public float BrakeInput { get => _brakeInput; set => _brakeInput = value; }
    public float SpeedCoefficient { get => _speedCoefficient; set => _speedCoefficient = value; }

    private void Awake()
    {
        Cashing();
    }

    private void Cashing()
    {
        _velocityController = GetComponent<IVelocityController>();
    }

    public void BrakeSystemInnit(WheelCollider f, WheelCollider r)
    {
        _forward = f;
        _rear = r;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        SpeedCoefficientCalc();
        BrakeDifferntial();

        InspectorInfoUotPut();
    }

    private void BrakeDifferntial()
    {
        if (BrakeInput > 0)
        {
            StartCoroutine(ForwardBrake());
            StartCoroutine(RearBrake());
        }
        else if (BrakeInput < 0)
        {
            if (_rear.rpm > 0)
            {
                StartCoroutine(RearBrake());
            }
            else
            {
                ReleaseBrakes();
            }
        }
        else
        {
            ReleaseBrakes();
        }
    }

    private void SpeedCoefficientCalc()
    {
        SpeedCoefficient = _velocityController.GetCurrentSpeed();
        SpeedCoefficient = Mathf.Clamp(SpeedCoefficient, 1, 50);
    }

    private void InspectorInfoUotPut()
    {
        _forwardBrake = _forward.brakeTorque;
        _rearBrake = _rear.brakeTorque;
    }

    public void ReleaseBrakes()
    {
        _forward.brakeTorque = 0;
        _rear.brakeTorque = 0;
    }

    private void ClampBrakes()
    {
        _forward.brakeTorque = Mathf.Clamp(_forward.brakeTorque, 0, MAX_BRAKE);
        _rear.brakeTorque = Mathf.Clamp(_rear.brakeTorque, 0, MAX_BRAKE);
    }

    IEnumerator ForwardBrake()
    {
        while (_forward.brakeTorque < MAX_BRAKE)
        {
            var aplliedBrakeToeruq = Mathf.Abs(BrakeInput) * BRAKE_COEFFCIENT * SpeedCoefficient;
            _forward.brakeTorque += aplliedBrakeToeruq;
            _forward.brakeTorque = Mathf.Clamp(_forward.brakeTorque, 0, MAX_BRAKE);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator RearBrake()
    {

        while (_rear.brakeTorque < MAX_BRAKE)
        {
            var aplliedBrakeToeruq = Mathf.Abs(BrakeInput) * BRAKE_COEFFCIENT * SpeedCoefficient;
            _rear.brakeTorque += aplliedBrakeToeruq;
            _rear.brakeTorque = Mathf.Clamp(_rear.brakeTorque, 0, MAX_BRAKE);
            yield return new WaitForFixedUpdate();
        }
    }
}

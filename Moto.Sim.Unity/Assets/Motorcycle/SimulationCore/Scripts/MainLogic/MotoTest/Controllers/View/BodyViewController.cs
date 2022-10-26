using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyViewController : MonoBehaviour
{
    [SerializeField] Transform _steerRuler;
    [SerializeField] WheelCollider _steerWheel;
    private Transform _moto;
    private float _wheelSteer;
    //Bike Body Lean
    [SerializeField] private GameObject _chassis;
    private float _chassisVerticalLean = 4.0f;
    private float _chassisHorizontalLean = 4.0f;
    private float _chasisHorizontalLeanMin = 0.0f;
    private float _chasisHorizontalLeanMax = 32.0f;
    private float _horizontalLean = 0.0f;
    private float _verticalLean = 0.0f;


    private Rigidbody _rigidBody;
    private float _whellie;

    public float Speed
    {
        get
        {
            return _rigidBody.velocity.magnitude * ProjectConstants.UnityToRealSpeedCoeeficient;
        }

    }


    private void Awake()
    {
        Cashing();
    }

    private void Cashing()
    {
        _moto = transform.parent;
        _rigidBody = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RullerAimation();
        Lean();
    }

    private void RullerAimation()
    {
        _wheelSteer = Mathf.Clamp(_steerWheel.steerAngle, -20, 20);
        _steerRuler.localEulerAngles = new Vector3(0, _wheelSteer, 0);
    }

    void Lean()
    {

        FreezeMoto();

        _chassisHorizontalLean =
            Mathf.Lerp(_chasisHorizontalLeanMin, _chasisHorizontalLeanMax, Speed / 100);
        _verticalLean =
            Mathf.Clamp(
                Mathf.Lerp(_verticalLean,
                transform.InverseTransformDirection(_rigidBody.angularVelocity).x * _chassisVerticalLean, Time.deltaTime * 5f)
                , -10.0f
                , 10.0f);

        WheelHit CorrespondingGroundHit;
        _steerWheel.GetGroundHit(out CorrespondingGroundHit);

        float normalizedLeanAngle = Mathf.Clamp(CorrespondingGroundHit.sidewaysSlip, -1f, 1f);

        if (transform.InverseTransformDirection(_rigidBody.velocity).z > 0f)
            normalizedLeanAngle = -1;
        else
            normalizedLeanAngle = 1;

        _horizontalLean =
            Mathf.Clamp(
                Mathf.Lerp(
                    _horizontalLean
                    , (transform.InverseTransformDirection(_rigidBody.angularVelocity).y * normalizedLeanAngle) * _chassisHorizontalLean
                    , Time.deltaTime * 3f), -50.0f, 50.0f);

        Quaternion target = Quaternion.Euler(_verticalLean, _chassis.transform.localRotation.y + (_rigidBody.angularVelocity.z), _horizontalLean);
        _chassis.transform.localRotation = target;


    }


    private void FreezeMoto()
    {
        _moto.localEulerAngles = new Vector3(_moto.localEulerAngles.x, _moto.localEulerAngles.y, 0);
    }
}

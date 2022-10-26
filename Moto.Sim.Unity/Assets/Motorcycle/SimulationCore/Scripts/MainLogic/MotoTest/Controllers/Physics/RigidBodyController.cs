using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyController : MonoBehaviour, IVelocityController
{
    [SerializeField, Range(0, 300)] float _speed;
    private bool _maxSpeedReached;
    private Rigidbody _rigidBody;
    private float _maxSpeed;
    private float _minSpeed;
    private float _maxAccelaration;

    public float MaxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    public float MinSpeed { get => _minSpeed; set => _minSpeed = value; }
    public bool MaxSpeedReached { get => _maxSpeedReached; set => _maxSpeedReached = value; }
    public float MaxAccelaration { get => _maxAccelaration; set => _maxAccelaration = value; }
    public float Accelaration { get => _accelaration; set => _accelaration = value; }

    public Transform COM;
    private float _currentSpeed;
    private float _accelaration;
    [SerializeField] AnimationCurve _acceleration;

    private void Awake()
    {
        Cashing();
        LoadSettings();
    }
    private void Start()
    { 

    }
    private void Cashing()
    {
        _rigidBody = GetComponentInParent<Rigidbody>();
    }

    private void LoadSettings()
    {
        MaxSpeed = SettingsLoader.Instance.Settings.MAX_SPEED.value;
        _maxAccelaration = SettingsLoader.Instance.Settings.MAX_SPEED_ACCELEARTION.Value;
        _rigidBody.mass = SettingsLoader.Instance.Settings.MOTO_MASS.value;
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotationZ;
        _rigidBody.centerOfMass = new Vector3(COM.localPosition.x * transform.localScale.x, COM.localPosition.y * transform.localScale.y, COM.localPosition.z * transform.localScale.z);
        _rigidBody.maxAngularVelocity = 2f;

    }

    private void FixedUpdate()
    {
        MaxSpeedHandler(); 
    }

    private void AccelarationLimit()
    {

        if(Accelaration > MaxAccelaration)
        {
           // _rigidBody.velocity = _rigidBody.velocity.normalized * GetCurrentSpeed();
        }    

    }

    private void MaxSpeedHandler()
    {
        _speed = GetCurrentSpeed();
        _maxSpeedReached = _speed > MaxSpeed;
        if(_maxSpeedReached)
        {
            _rigidBody.velocity = _rigidBody.velocity.normalized * (MaxSpeed / ProjectConstants.UnityToRealSpeedCoeeficient);
        }
    }

    public float GetCurrentSpeed()
    {
        return _rigidBody.velocity.magnitude * ProjectConstants.UnityToRealSpeedCoeeficient;
    }

    IEnumerator MeasureAccelaration()
    {
        Accelaration =0 ;
        while (gameObject.activeInHierarchy)
        {
            _currentSpeed = GetCurrentSpeed();
            yield return new WaitForFixedUpdate();
            Accelaration = _currentSpeed - GetCurrentSpeed();
            _acceleration.AddKey(Time.frameCount, Accelaration);
        }
    }
}

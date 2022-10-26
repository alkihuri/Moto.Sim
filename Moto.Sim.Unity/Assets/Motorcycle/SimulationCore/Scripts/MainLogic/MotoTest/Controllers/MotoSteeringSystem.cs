using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoSteeringSystem : VehicleSteerSystemBase, IVehicleSteeringSystem
{

    [SerializeField] private WheelCollider _forwardWheel;
    [SerializeField] private WheelCollider _rearWheel;
    [Header("Settings:")]
    [SerializeField] AnimationCurve _steerCurve;
    private VehiclePhysicSetting _settings;

    private float _steer;
    private float _brake;

    private float steerInput;
    private float _defsteerAngle;
    private float _highSpeedSteerAngle;
    private float _highSpeedSteerAngleAtSpeed;

    private Rigidbody _rigidBody;


    private MotoBrakeSystem _brakeSystem;

    public float Steer { get => _steer; set => _steer = value; }
    public float Brake { get => _brake; set => _brake = value; }
    public float Speed
    {
        get
        {
            return _rigidBody.velocity.magnitude * ProjectConstants.UnityToRealSpeedCoeeficient;
        }

    }
    public float SteerAngle { get; private set; }



    private void Awake()
    {
        Cashing();
        Settings();
    }

    private void Settings()
    {
        SteerCurveInnit();
    }

    private void SteerCurveInnit()
    {

        _steerCurve.AddKey(_settings.SPEED_FOR_MAX_STEER_ANGLE.value, _settings.MAX_STEER_ANGLE.value);

        var medianSteerAngle = _settings.MIN_STEER_ANGLE.value + (_settings.MAX_STEER_ANGLE.value - _settings.MIN_STEER_ANGLE.value) / 2;

        _steerCurve.AddKey(_settings.SPEED_FOR_MEDIAN_STEER_ANGLE.value, medianSteerAngle);

        _steerCurve.AddKey(_settings.SPEED_FOR_MIN_STEER_ANGLE.value, _settings.MIN_STEER_ANGLE.value);
    }

    private void Cashing()
    {
        _rigidBody = GetComponentInParent<Rigidbody>();
        _settings = SettingsLoader.Instance.Settings;
        _brakeSystem = GetComponent<MotoBrakeSystem>();
        _brakeSystem.BrakeSystemInnit(_forwardWheel, _rearWheel);
    }

    public void ApplySteer(float steer)
    {
        steerInput = steer;
    }

    public void ApplyBrake(float brake)
    {
        Brake = brake; 
    }

    public void SetSteerAngle(float angle)
    {
        SteerAngle = angle;
    }


    void FixedUpdate()
    {
        WheelSettings();
        Steering();
        Braking();
        SteeringSettings();
    }
    private void WheelSettings()
    {
        _highSpeedSteerAngle = SettingsLoader.Instance.Settings.MIN_STEER_ANGLE.value;
        _defsteerAngle = SteerAngle;
    }
    private void Steering()
    {
        _forwardWheel.steerAngle = SteerAngle * steerInput;
    }

    private void Braking()
    {
        _brakeSystem.BrakeInput = Brake;
    }
    private void SteeringSettings()
    {
        var steerAngle = _steerCurve.Evaluate(Speed);
        SetSteerAngle(steerAngle);
    }
}

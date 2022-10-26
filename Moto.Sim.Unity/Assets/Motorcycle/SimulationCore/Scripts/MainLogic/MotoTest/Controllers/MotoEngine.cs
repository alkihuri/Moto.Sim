using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoEngine : VehicleEngineBase, IVehicleEngine
{

    public WheelCollider _forwardWheel;
    public WheelCollider _rearWheel;
    private Rigidbody _rigidBody;
    private float _maxSpeed;
    private bool _reversing;
    private float _motorInput;
    [Header("Engine  power settings:")] 
    [SerializeField] AnimationCurve _enginePowerCurve;
    [SerializeField] private int _currentGear;
    private float _gearShiftRate;
    private float _clutch;

    [SerializeField] AnimationCurve _enginePowerOverLifeTime;
    public float EngineRPM { get; private set; }
    public float Speed
    {
        get
        {
            return _rigidBody.velocity.magnitude * ProjectConstants.UnityToRealSpeedCoeeficient;
        }

    }
    public float EngineTorque { get; private set; }
    public float MinEngineRPM { get; private set; }
    public float MaxEngineRPM { get; private set; }
    public int CurrentGear { get => _currentGear; set => _currentGear = value; }
    public float GearShiftRate { get => _gearShiftRate; set => _gearShiftRate = value; }
    public float MotorInput { get => _motorInput; set => _motorInput = value; }
    public float MaxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    public bool Reversing { get => _reversing; set => _reversing = value; }
    public float Clutch { get => _clutch; set => _clutch = value; }

    public void BrakeSystemInnit(WheelCollider f, WheelCollider r)
    {
        _forwardWheel = f;
        _rearWheel = r;
    }
    private void Awake()
    {
        Settings();
        Cashing();
    }

    private void Cashing()
    {
        _rigidBody = GetComponentInParent<Rigidbody>();
    }

    private void Settings()
    {
        EngineTorque = 5500f;
        MaxEngineRPM = 6000f;
        MinEngineRPM = 1000f;
        MaxSpeed = SettingsLoader.Instance.Settings.MAX_SPEED.value; 
    }

   

    public void ApplyTorque(float torque)
    {
        _motorInput = torque;
    }

    void Engine()
    { 
        EngineRPM = Mathf.Clamp((((Mathf.Abs((_forwardWheel.rpm + _rearWheel.rpm))) + MinEngineRPM)) / (CurrentGear + 1), MinEngineRPM, MaxEngineRPM);
        _enginePowerOverLifeTime.AddKey(Time.frameCount, EngineRPM);

        if (Speed > MaxSpeed)
        {
            _rearWheel.motorTorque = 0;
        }
        else
        {
            _rearWheel.motorTorque = EngineTorque * Mathf.Clamp(_motorInput, 0f, 1f) * CurrentGear;
        }

        Reversing = _motorInput < 0;

        if (Reversing)
        {
            if (Speed < 10)
            {
                _rearWheel.motorTorque = ((EngineTorque * _motorInput) / 3f);
            }
            else
            {
                _rearWheel.motorTorque = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        Engine();
    }

    public void SetGear(int gear)
    {
        CurrentGear = gear;
    }

    public float GetRPM()
    {
        return Mathf.Abs((_forwardWheel.rpm + _rearWheel.rpm)) / CurrentGear;
    }

    public int GetGear()
    {
        return CurrentGear;
    }
}

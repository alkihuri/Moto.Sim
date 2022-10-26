using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoController : VehicleControllerBase, IVehicleController
{

    private IVehicleEngine _engine;
    private IVehicleSteeringSystem _steer;
    private IVehicleGearBox _gearBox;

    private void Awake()
    {
        Cashing();
        Settings();
    }

    private void Settings()
    { 
        _gearBox.SetGear(1);
    }

    private void Cashing()
    {
        _engine = GetComponent<IVehicleEngine>();
        _steer = GetComponent<IVehicleSteeringSystem>();
        _gearBox = GetComponent<IVehicleGearBox>();
    }

    public void ApplyInputs(float torque, float steer, float brake)
    {
        _engine.ApplyTorque(torque);
        _steer.ApplySteer(steer);
        _steer.ApplyBrake(brake);
    }


}

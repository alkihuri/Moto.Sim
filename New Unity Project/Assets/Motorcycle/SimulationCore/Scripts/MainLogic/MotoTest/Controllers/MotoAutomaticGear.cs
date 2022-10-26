using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotoAutomaticGear : VehicleGerBoxSystem, IVehicleGearBox
{
    private IVehicleEngine _motoEngine;
    private IVelocityController _velocityController;
    [Header("Gear box settings:")]
    [SerializeField, Range(1, 10)] private float _gearsNum;
    [SerializeField, Range(0, 5000)] private float _gearShiftLimit;
    [SerializeField] private bool _gearChanging;


    public UnityEvent OnNextGear = new UnityEvent();
    public UnityEvent OnPrevGear = new UnityEvent();

    public float Speed
    {
        get
        {
            return _velocityController.GetCurrentSpeed();
        }
    }
    public float RPM
    {
        get
        {
            return _motoEngine.GetRPM();
        }
    }

    public float GearsNum { get => _gearsNum; set => _gearsNum = value; }
    public float GearShiftLimit { get => _gearShiftLimit; set => _gearShiftLimit = value; }
    public bool GearChanging { get => _gearChanging; set => _gearChanging = value; }

    private void Awake()
    {
        Cashing();
        Settings();
    }

    private void Settings()
    {
        if (GearsNum == null)
            GearsNum = 3;
        _motoEngine.SetGear(1);

        OnNextGear.AddListener(NextGear);
        OnPrevGear.AddListener(PrevGear);
    }

    private void Cashing()
    {
        _motoEngine = GetComponent<IVehicleEngine>();
        _velocityController = GetComponent<IVelocityController>();
    }

    public void SetGear(int gear)
    {
        _motoEngine.SetGear(gear);
    }


    private void FixedUpdate()
    {
        GearBox();
    }

    private void GearBox()
    {
        if (GearChanging)
            return;

        if (RPM > GearShiftLimit)
        {
            StartCoroutine(DelayedInvoke(OnNextGear));
        }
        else if (RPM > 10)
        {
            StartCoroutine(DelayedInvoke(OnPrevGear));
        }
    }
    public void NextGear()
    {
        int nextGear = (int)Mathf.Clamp(_motoEngine.GetGear() + 1, 1, GearsNum);
        _motoEngine.SetGear(nextGear);
    }
    public void PrevGear()
    {
        int prevGear = (int)Mathf.Clamp(_motoEngine.GetGear() - 1, 1, GearsNum);
        _motoEngine.SetGear(prevGear);
    }

    IEnumerator DelayedInvoke(UnityEvent gearEvent, float delay = 1)
    {
        GearChanging = true;
        yield return new WaitForSeconds(delay);
        gearEvent.Invoke();
        GearChanging = false;

    }
}


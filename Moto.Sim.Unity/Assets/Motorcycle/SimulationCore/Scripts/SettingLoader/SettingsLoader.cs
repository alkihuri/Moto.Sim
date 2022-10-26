using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsLoader : MonoSinglethon<SettingsLoader>
{
    [SerializeField]
    private VehiclePhysicSetting _settings;

    public VehiclePhysicSetting Settings { get => _settings; set => _settings = value; }

    private void Awake() => SettingsInnit();

    private void SettingsInnit()
    {
        if (_settings == null)
        {
            Debug.LogError("There is no settings!");
        }
    }
}

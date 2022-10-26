using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Moto physics setting", menuName = "Vice physics/Vehicle physics", order = 1)]
public class VehiclePhysicSetting : ScriptableObject
{
    public SettingItem ACCELARATION_100_Km_H;
    public SettingItem MAX_SPEED_ACCELEARTION;
    public SettingItem MAX_SPEED;
    public SettingItem MAX_STEER_ANGLE; 
    public SettingItem MIN_STEER_ANGLE;
    public SettingItem SPEED_FOR_MAX_STEER_ANGLE;
    public SettingItem SPEED_FOR_MEDIAN_STEER_ANGLE;
    public SettingItem SPEED_FOR_MIN_STEER_ANGLE;
    public SettingItem TIME_FOR_BURNOUT;
    public bool WHILLIE;
    public SettingItem MOTO_MASS;

}

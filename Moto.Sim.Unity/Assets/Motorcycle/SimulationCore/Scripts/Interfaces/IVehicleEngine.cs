public interface IVehicleEngine
{
    public void ApplyTorque(float torque);
    public void SetGear(int gear);
    public int GetGear();
    public float GetRPM();
}

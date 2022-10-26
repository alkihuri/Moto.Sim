using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public float _maxSteering = 0;
    private IVelocityController _velocityController;

    public WheelCollider _forwardWheel;
    public WheelCollider _rearWheel;
    public Transform _forwardWheelTransform;
    public Transform _rearWheelTransform;

    public float RotationValue2 { get; private set; }
    public float RotationValue1 { get; private set; }
    public float Speed { get; private set; }
    public float Stifnes { get; private set; }

    private void Awake()
    {
        Cashing();
    }

    private void Cashing()
    {
        _velocityController = GetComponent<IVelocityController>();
    }

    private void FixedUpdate()
    {
        WheelFriction();
        WheelAlign();
    }

    private void WheelFriction()
    {
        Speed = _velocityController.GetCurrentSpeed();
        Stifnes = Mathf.Clamp(Speed / 10, 1, 8);
        ApplyFriction(Stifnes, _forwardWheel);
        Stifnes = Mathf.Clamp(Speed / 10, 1, 8);
        ApplyFriction(Stifnes, _rearWheel);
    }

    public void ApplyFriction(float stifness, WheelCollider wheel)
    {
        var sidewayFriction = wheel.sidewaysFriction;
        sidewayFriction.stiffness = stifness;
        wheel.sidewaysFriction = sidewayFriction;
        var forwardFriction = wheel.forwardFriction;
        forwardFriction.stiffness = stifness;
        wheel.forwardFriction = forwardFriction;
    }
    void WheelAlign()
    {

        RaycastHit hit;
        WheelHit CorrespondingGroundHit;
        float extension_F;
        float extension_R;

        Vector3 ColliderCenterPointFL = _forwardWheel.transform.TransformPoint(_forwardWheel.center);
        _forwardWheel.GetGroundHit(out CorrespondingGroundHit);

        if (Physics.Raycast(ColliderCenterPointFL, -_forwardWheel.transform.up, out hit, (_forwardWheel.suspensionDistance + _forwardWheel.radius) * transform.localScale.y))
        {
            _forwardWheelTransform.transform.position = hit.point + (_forwardWheel.transform.up * _forwardWheel.radius) * transform.localScale.y;
            extension_F = (-_forwardWheel.transform.InverseTransformPoint(CorrespondingGroundHit.point).y - _forwardWheel.radius) / _forwardWheel.suspensionDistance;
            Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point + _forwardWheel.transform.up * (CorrespondingGroundHit.force / 8000), extension_F <= 0.0f ? Color.magenta : Color.white);
            Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - _forwardWheel.transform.forward * CorrespondingGroundHit.forwardSlip, Color.green);
            Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - _forwardWheel.transform.right * CorrespondingGroundHit.sidewaysSlip, Color.red); 
        }
        else
        {
            _forwardWheelTransform.transform.position = ColliderCenterPointFL - (_forwardWheel.transform.up * _forwardWheel.suspensionDistance) * transform.localScale.y;
        }
        RotationValue1 += _forwardWheel.rpm * (6) * Time.deltaTime;
        _forwardWheelTransform.transform.rotation = _forwardWheel.transform.rotation * Quaternion.Euler(RotationValue1, _forwardWheel.steerAngle, _forwardWheel.transform.rotation.z);

        Vector3 ColliderCenterPointRL = _rearWheel.transform.TransformPoint(_rearWheel.center);
        _rearWheel.GetGroundHit(out CorrespondingGroundHit);

        if (Physics.Raycast(ColliderCenterPointRL, -_rearWheel.transform.up, out hit, (_rearWheel.suspensionDistance + _rearWheel.radius) * transform.localScale.y))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Bike"))
            {
                _rearWheelTransform.transform.position = hit.point + (_rearWheel.transform.up * _rearWheel.radius) * transform.localScale.y;
                extension_R = (-_rearWheel.transform.InverseTransformPoint(CorrespondingGroundHit.point).y - _rearWheel.radius) / _rearWheel.suspensionDistance;
                Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point + _rearWheel.transform.up * (CorrespondingGroundHit.force / 8000), extension_R <= 0.0f ? Color.magenta : Color.white);
                Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - _rearWheel.transform.forward * CorrespondingGroundHit.forwardSlip, Color.green);
                Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - _rearWheel.transform.right * CorrespondingGroundHit.sidewaysSlip, Color.red);
            }
        }
        else
        {
            _rearWheelTransform.transform.position = ColliderCenterPointRL - (_rearWheel.transform.up * _rearWheel.suspensionDistance) * transform.localScale.y;
        }
        RotationValue2 += _rearWheel.rpm * (6) * Time.deltaTime;
        _rearWheelTransform.transform.rotation = _rearWheel.transform.rotation * Quaternion.Euler(RotationValue2, _rearWheel.steerAngle, _rearWheel.transform.rotation.z);

    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoForce : VehicleForce, IVehicleForces
{
    private Rigidbody _rigidBody;
    private float _wheelieTrigger;
    private Vector3 _torqueForce;
    private Vector3 _startEuler;
    private Vector3 _wheelieEuler;
    private Vector3 _calculatedValue;
    private WheelCollider _forwardWheelCollider;
    private WheelCollider _rearWheelCollider;
    [SerializeField] Transform _chasis;
    [SerializeField] Transform _rearWheel;
    [SerializeField] Transform _forwardWheel;
    [SerializeField, Range(-30, 30)] private float _differnce;
    private bool _gravity;
    private bool _isWhellie;
    private Vector3 _com;
    private float _wheelieToqrue;

    public bool IsWhellie { get => _isWhellie; set => _isWhellie = value; }

    public void ApplyForce(Vector3 force)
    {
        throw new System.NotImplementedException();
    }
    private void Awake()
    {
        Cashing();
        LoadSettings();
    }

    private void Cashing()
    {
        _rigidBody = GetComponentInParent<Rigidbody>();
        _forwardWheelCollider = _forwardWheel.gameObject.GetComponent<WheelCollider>();
        _rearWheelCollider = _forwardWheel.gameObject.GetComponent<WheelCollider>();
        _startEuler = transform.localEulerAngles;
        _wheelieEuler = new Vector3(30, 0, 0);
    }

    private void LoadSettings()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _com = _rigidBody.centerOfMass;
    }
    private void Gravity()
    {
        if (IsWhellie)
            return;

        var value = _rigidBody.velocity.magnitude / 25;
        WheelHit wheelHit;
        _forwardWheelCollider.GetGroundHit(out wheelHit);
        if (wheelHit.collider == null)
            value--;
        _rearWheelCollider.GetGroundHit(out wheelHit);
        if (wheelHit.collider == null)
            value--;

        value = Mathf.Clamp(value, 1, 5);

        _rigidBody.AddForce(Physics.gravity * value, ForceMode.Acceleration);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Gravity(); 
    }

    private void Wheelie()
    {
        IsWhellie = _wheelieTrigger != 0 && Input.GetKey(KeyCode.G);
        _wheelieTrigger = Input.GetAxis("Vertical");
        _torqueForce = new Vector3(-_wheelieTrigger / 5, 0, 0);
        _rigidBody.centerOfMass = IsWhellie ? _wheelieTrigger >= 0 ? _forwardWheel.localPosition : _rearWheel.localPosition : _com;
        _rigidBody.angularDrag = IsWhellie ? 4 : 0.1f;
        _rigidBody.drag = IsWhellie ? 1 : 0.1f;
        var angle = Mathf.Abs(transform.parent.transform.eulerAngles.x);
        angle = angle > 180 ? angle - 360 : angle;
        _differnce = Mathf.Clamp(angle, -30, 30);
        _wheelieToqrue = -_wheelieTrigger / 10;
        if (IsWhellie)
        {
            Vector3 force = new Vector3(-_differnce, 0, 0); 
            _rigidBody.AddRelativeTorque(force, ForceMode.VelocityChange);
        }
    }

    private IEnumerator DoWheelie()
    {
        while (!Input.GetKeyUp(KeyCode.G))
        {
            Vector3 force = new Vector3(-_differnce, 0, 0);
            yield return new WaitForFixedUpdate();
            _rigidBody.AddRelativeTorque(force, ForceMode.VelocityChange);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_rigidBody != null)
        {

            Gizmos.DrawSphere(transform.TransformPoint(_rigidBody.centerOfMass), 1f);
            Debug.DrawLine(transform.position, _rigidBody.centerOfMass);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MotoShapeView : MonoBehaviour
{
    [SerializeField] Transform _forwardWheelView;
    [SerializeField] Transform _rearWheelView;
    [SerializeField] Transform _bodyView;
    [SerializeField] Transform _body;
    [SerializeField] WheelCollider _forwardWheel;
    [SerializeField] WheelCollider _rearWheel;

    [SerializeField] TextMeshProUGUI _steer;
    [SerializeField] TextMeshProUGUI _bodyAngle;
    // Update is called once per frame
    void Update()
    {
        _forwardWheelView.localEulerAngles = new Vector3(0, 0, -_forwardWheel.steerAngle);
        _bodyView.localEulerAngles = new Vector3(0, _body.localEulerAngles.z, _body.localEulerAngles.y);

        _steer.text = ((int)_forwardWheel.steerAngle).ToString();
        var angle = -(int)_body.localEulerAngles.z;
        _bodyAngle.text = (angle < -180 ? angle + 360 : angle).ToString();
        _bodyAngle.gameObject.transform.localEulerAngles = new Vector3(0, 0, _body.localEulerAngles.y);
    }
}

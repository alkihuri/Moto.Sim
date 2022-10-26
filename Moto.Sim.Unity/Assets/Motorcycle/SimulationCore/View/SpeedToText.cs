using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpeedToText : MonoBehaviour
{

    [SerializeField]
    private Rigidbody _rigidBody;
    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "speed : " + (_rigidBody.velocity.magnitude * ProjectConstants.UnityToRealSpeedCoeeficient).ToString("#.");
    }
}

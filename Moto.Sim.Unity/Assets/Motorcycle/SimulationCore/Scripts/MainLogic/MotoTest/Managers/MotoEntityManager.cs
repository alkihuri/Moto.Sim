using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotoEntityManager : MonoBehaviour
{
    [SerializeField] private MotoController _motoController;
    [SerializeField] private MotoAutomaticGear _motoAutomaticGearBox;
    [SerializeField] private RigidBodyController _rigidBodyController;
    [SerializeField] private WheelController _wheelController;
    [SerializeField] private MotoSteeringSystem _motoSteeringSystem;
    [SerializeField] private MotoEngine _motoEngine;
    [SerializeField] private BodyViewController _bodyVieCOntroller;

    public UnityEvent OnCOmponentsTurnedOff = new UnityEvent(); 
    
    public MotoController MotoController { get => _motoController; set => _motoController = value; }
    public MotoAutomaticGear MotoAutomaticGearBox { get => _motoAutomaticGearBox; set => _motoAutomaticGearBox = value; }
    public RigidBodyController RigidBodyController { get => _rigidBodyController; set => _rigidBodyController = value; }
    public WheelController WheelController { get => _wheelController; set => _wheelController = value; }
    public MotoSteeringSystem MotoSteeringSystem { get => _motoSteeringSystem; set => _motoSteeringSystem = value; }
    public MotoEngine MotoEngine { get => _motoEngine; set => _motoEngine = value; }
    public BodyViewController BodyVieCOntroller { get => _bodyVieCOntroller; set => _bodyVieCOntroller = value; }
    private void Awake()
    {
        Cashing();
    }
    private void Cashing()
    {
        MotoController = GetComponent<MotoController>(); ;
        MotoAutomaticGearBox = GetComponent<MotoAutomaticGear>();  
        RigidBodyController = GetComponent<RigidBodyController>();
        WheelController = GetComponent<WheelController>();
        MotoSteeringSystem = GetComponent<MotoSteeringSystem>();
        MotoEngine = GetComponent<MotoEngine>();
        BodyVieCOntroller = GetComponent<BodyViewController>();
    }
}

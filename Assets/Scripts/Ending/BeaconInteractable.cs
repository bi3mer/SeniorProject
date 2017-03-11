using UnityEngine;
using System.Collections;
using DG.Tweening;


public class BeaconInteractable : MonoBehaviour
{
    [Tooltip("The light that comes off of the beacon.")]
    [SerializeField]
    private GameObject lightPlanes;

    [Tooltip("The length the light extends once it's turned on.")]
    [SerializeField]
    private float lightPlaneLength;

    [Tooltip("The beacon's rotation once it is turned on.")]
    [SerializeField]
    private Vector3 onRotation;

    [Tooltip("Beacon rotating bit")]
    [SerializeField]
    private GameObject RotatingBeacon;

    [Tooltip("The time it takes for the turn on animation to play")]
    [SerializeField]
    private float turnOnTime;

    [Tooltip("The beacon's rotation once it is turned on.")]
    [SerializeField]
    private Material beaconLightMaterial;

    [Tooltip("The beacon's color once it is turned on.")]
    [SerializeField]
    private Color beaconOnColor;

    [Tooltip("The beacon's color once it is turned off.")]
    [SerializeField]
    private Color beaconOffColor;

    [SerializeField]
    private EndingController endingController;

    private const string materialEmissionColor = "_EmissionColor";

    private bool isOn = false;

    // Use this for initialization
    void Start ()
    {
        beaconLightMaterial.SetColor(materialEmissionColor, beaconOffColor);
	}

    /// <summary>
    /// Called to turn the light on and point it up in the air.
    /// </summary>
    public void TurnLightOn()
    {
        if(!isOn)
        {
            isOn = true;
            // Tween the rotations
            RotatingBeacon.transform.DORotate(onRotation, turnOnTime);
            // Set emission color
            beaconLightMaterial.SetColor(materialEmissionColor, beaconOnColor);

            // Tween the light shaft
            lightPlanes.transform.DOScaleZ(lightPlaneLength, turnOnTime);

            GetComponent<InteractableObject>().enabled = false;

            endingController.BeaconTurnedOn();
        }
    } 
}

using UnityEngine;
using System.Collections;
using DG.Tweening;

// Contains functions that can be called to animate aspects of the radio with tweens.
public class InteractableRadioModel : MonoBehaviour
{
    [Header ("Radio Take Out and Putaway Variables")]
    [SerializeField]
    [Tooltip("Position of the radio when it's active and on screen.")]
    private Vector3 activePosition;
    [SerializeField]
    [Tooltip("Position of the radio when it's first activated, but not on screen yet.")]
    private Vector3 offscreenPosition;
    [SerializeField]
    [Tooltip("Rotation of the radio when it's activated")]
    private Vector3 activeRotation;
    [SerializeField]
    [Tooltip("Rotation of the radio when it's first activated, but not on screen yet.")]
    private Vector3 offscreenRotation;
    [SerializeField]
    [Tooltip("The length of the activation/deactivation animation")]
    private float takeOutTime;

    [Header("Radio Button and Knob Game Objects")]
    [SerializeField]
    [Tooltip("The radio models power button game object")]
    private GameObject powerButton;
    [SerializeField]
    [Tooltip("The radio models stationKnob game object")]
    private GameObject stationKnob;
    [SerializeField]
    [Tooltip("The radio models StationSlider game object")]
    private GameObject stationSlider;
    [SerializeField]
    [Tooltip("The radio models Volume Up game object")]
    private GameObject volumeUp;
    [SerializeField]
    [Tooltip("The radio models Volume Down button game object")]
    private GameObject volumeDown;

    [Header("Button Animation Variables")]
    [SerializeField]
    [Tooltip("How far should the buttons move down when they are pressed?")]
    private float buttonDown = 0.0002f;
    [SerializeField]
    [Tooltip("How long does a button press take?")]
    private float buttonDownTime = 0.25f;

    [Header("Slider Animation Variables")]
    [SerializeField]
    [Tooltip("How far left can the slider move?")]
    private float stationSliderLeftMax = -0.003f;
    [SerializeField]
    [Tooltip("How far right can the slider move?")]
    private float stationSliderRightMax = 0.0002f;
    [SerializeField]
    [Tooltip("How fast should the knob rotate?")]
    private float knobSpeed = 400f;

    [Header("Other")]
    [SerializeField]
    private Canvas radioCanvas;
    [SerializeField]
    private GameViewBehavior gameViewBehavior;

    // Unexposed Variables
    private Vector3 powerButtonStartPosition;
    private Vector3 volumeUpButtonStartPosition;
    private Vector3 volumeDownButtonStartPosition;
    private MeshRenderer radioMesh;

    private float sliderLength;

    [SerializeField]
    // Because of the tiny gamescale we're using DoTween tends to start up tweens so fast that it looks like the object jumps.
    // This is a curve that has a slow start up to it to prevent the jumpiness.
    private AnimationCurve slowStartCurve;

    /// <summary>
    /// Set up all the button start positions.
    /// </summary>
    void Start()
    {
        powerButtonStartPosition = powerButton.transform.localPosition;
        volumeUpButtonStartPosition = volumeUp.transform.localPosition;
        volumeDownButtonStartPosition = volumeDown.transform.localPosition;
        radioMesh = GetComponent<MeshRenderer>();
        radioMesh.enabled = false;

        // find length of slider
        sliderLength = (stationSliderLeftMax - stationSliderRightMax);
    }


    /// <summary>
    /// Activates the radio mesh's and moves the radio from the offscreen position to the onscreen position.
    /// </summary>
    [ContextMenu("Activate Radio")]
    public void ActivateRadio()
    {
        transform.gameObject.SetActive(true);
        transform.localPosition = offscreenPosition;
        transform.localEulerAngles = offscreenRotation;
        radioMesh.enabled = true;
        radioCanvas.gameObject.SetActive(true);

        transform.DOLocalMove(activePosition, takeOutTime).SetEase(slowStartCurve);
        transform.DOLocalRotate(activeRotation, takeOutTime).SetEase(slowStartCurve);
    }

    /// <summary>
    /// Calls the deactivate coroutine
    /// </summary>
    [ContextMenu("Deactivate Radio")]
    public void DeactivateRadio()
    {
        transform.localPosition = activePosition;
        transform.localEulerAngles = activeRotation;
        radioMesh.enabled = true;

        transform.gameObject.SetActive(true);

        StartCoroutine(DeactivateCoroutine());
    }

    /// <summary>
    /// Deactivates the radio mesh's and moves the radio from the onscreen position to the offscreen position.
    /// </summary>
    private IEnumerator DeactivateCoroutine()
    {
        Tween myTween = transform.DOLocalMove(offscreenPosition, takeOutTime).SetEase(slowStartCurve);
        transform.DOLocalRotate(offscreenRotation, takeOutTime).SetEase(slowStartCurve);

        yield return myTween.WaitForCompletion();

        radioMesh.enabled = false;
        radioCanvas.gameObject.SetActive(false);
        gameViewBehavior.OnResumeClick();
    }


    /// <summary>
    /// Play the power button push animation
    /// </summary>
    [ContextMenu("Push Power Button")]
    public void PushPowerButton()
    {
        StartCoroutine(ButtonPressCoroutine(powerButton, powerButtonStartPosition));
    }

    /// <summary>
    /// Play the volume up button push animation
    /// </summary>
    [ContextMenu("Push Volume Up Button")]
    public void PushVolumeUpButton()
    {
        StartCoroutine(ButtonPressCoroutine(volumeUp, volumeUpButtonStartPosition));
    }

    /// <summary>
    /// Play the volume down button push animation
    /// </summary>
    [ContextMenu("Push Volume Down Button")]
    public void PushVolumeDownButton()
    {
        StartCoroutine(ButtonPressCoroutine(volumeDown, volumeDownButtonStartPosition));
    }

    /// <summary>
    /// The coroutine called for all button pushes
    /// </summary>
    /// <param name="button">The button game object to push</param>
    /// <param name="startposition">The starting position of the button</param>
    /// <returns></returns>
    private IEnumerator ButtonPressCoroutine(GameObject button, Vector3 startposition)
    {
        button.transform.localPosition = startposition;

        Sequence buttonSequence = DOTween.Sequence();

        // Devide button down time by 2 because we need to half it since we have two tweens.
        buttonSequence.Append(button.transform.DOLocalMove(startposition - new Vector3(0f, buttonDown, 0f), buttonDownTime/2f).SetEase(slowStartCurve));
        buttonSequence.Append(button.transform.DOLocalMove(startposition, buttonDownTime/2f).SetEase(slowStartCurve));
        buttonSequence.Play();
        yield return buttonSequence.WaitForCompletion();
    }

    /// <summary>
    /// Sets the rotation of the knob, and sets the position of the slider based on a value between 0-1
    /// </summary>
    /// <param name="value"> A number between 0-1 anything greater or less than will be clamped.</param>
    public void SetKnobRotation(float value)
    {
        value = Mathf.Clamp01(value);

        stationKnob.transform.localEulerAngles = new Vector3(0f, value * knobSpeed, 0f);
        
        // Change the value's range to the slider's range.
        value = (value * (stationSliderRightMax - stationSliderLeftMax)) + stationSliderLeftMax;
        stationSlider.transform.localPosition = new Vector3(value, stationSlider.transform.localPosition.y, stationSlider.transform.localPosition.z);
    }

    /// <summary>
    /// Slide the slider based on the degree of which the knob is turned.
    /// </summary>
    /// <param name="degree"></param>
    public void SetSlider(float degree)
    {
        // convert circle degree to slider length
        float xPosition = (degree * sliderLength) / 360;
        
        // divide the slider length by 2 to produce the positive half of the slider. 
        // subtract the xposition to get whether it's negative or positive. 
        xPosition = sliderLength / 2 - xPosition;
        stationSlider.transform.localPosition = new Vector3(xPosition, 
                                                stationSlider.transform.localPosition.y, 
                                                stationSlider.transform.localPosition.z);
    }
}

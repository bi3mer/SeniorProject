using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Dial : MonoBehaviour
{
    [SerializeField]
    private Vector3 knobRotation;

    public Slider DialSlider;
    [SerializeField]
    private GameObject knob;

    private bool isRotating;
    private Vector3 mouseRef;
    private Vector3 mouseOffset;

    /// <summary>
    /// Check for if the knob is rotating, then rotate the knob.
    /// </summary>
    void Update()
    {
        // Move knob if is rotating is true
        if (isRotating == true)
        {
            // Stop the button from turning when first clicked
            if (mouseRef != Vector3.zero)
            {
                // Get the difference b/w mouse position and previous position
                mouseOffset = Input.mousePosition - mouseRef;

                // Use the difference's x value to determine rotation increase
                knobRotation.z = -mouseOffset.x;
                        
                knob.transform.Rotate(knobRotation);

                // Change the dial - if knob rotation is negative make it positive
                if (knob.transform.eulerAngles.z > 0)
                {
                    // Subtract from 360 (a circle's total degrees) since numbers are reversed
                    DialSlider.value = 360 - knob.transform.eulerAngles.z;
                }
                else
                {
                    DialSlider.value = -(knob.transform.eulerAngles.z);
                }
            }

            // Update previous mouse position
            mouseRef = Input.mousePosition;
        }
    }

    /// <summary>
    /// Start rotating the knob.
    /// </summary>
    public void StartRotation()
    {
        isRotating = true;
    }

    /// <summary>
    /// Stop rotating the knob.
    /// </summary>
    public void StopRotation()
    {
        isRotating = false;

        // Reset mouseRef to keep from turning when knob is clicked again
        mouseRef = Vector2.zero;
    }
}

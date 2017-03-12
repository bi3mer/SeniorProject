using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Dial : MonoBehaviour
{
    [SerializeField]
    private Vector3 knobRotation;

    [SerializeField]
    private InteractableRadioModel radioModelAnimation;

    [SerializeField]
    private GameObject knob;

    private bool isRotating;
    private Vector3 mouseRef;
    private Vector3 mouseOffset;

    public float knobDegree;

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
                knobRotation.y = -mouseOffset.x;

                knob.transform.Rotate(knobRotation);

                // Add 180 to the knob angle to convert value to regular circle degrees, 
                // then mod by 360 to keep within 360 degrees.
                knobDegree = (knob.transform.localRotation.eulerAngles.y + 180) % 360;

                radioModelAnimation.SetSlider(knobDegree);
            }
                      
            // Update previous mouse position
            mouseRef = Input.mousePosition;
        }
    }

    /// <summary>
    /// Start rotating the knob.
    /// </summary>
    void OnMouseDown()
    {
        isRotating = true;
    }

    /// <summary>
    /// Stop rotating the knob.
    /// </summary>
    void OnMouseUp()
    {
        isRotating = false;

        // Reset mouseRef to keep from turning when knob is clicked again
        mouseRef = Vector2.zero;
    }
}

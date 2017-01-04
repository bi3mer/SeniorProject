using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Dial : MonoBehaviour
{
	//[SerializeField]
	//private Radio currentRadio;
    [SerializeField]
    private Vector3 knobRotation;

    public Slider dialSlider;
    private GameObject dial;
    private GameObject knob;

    private bool isRotating;
    private Vector3 mouseRef;
    private Vector3 mouseOffset;

    // Use this for initialization
    void Start ()
    {
        //set up radio with game instance
        //currentRadio = Game.Instance.RadioInstance;

        knob = GameObject.FindGameObjectWithTag("Knob");
        dial = GameObject.FindGameObjectWithTag("Dial");
        dialSlider = dial.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move knob if is rotating is true
        if (isRotating == true)
        {
            //stop the button from turning when first clicked
            if (mouseRef != new Vector3(0,0,0))
            {
                //get the difference b/w mouse position and previous position
                mouseOffset = Input.mousePosition - mouseRef;

                //use the difference's x value to determine rotation increase
                knobRotation.z = -mouseOffset.x;
                        
                knob.transform.Rotate(knobRotation);

                //change the dial - if knob rotation is negative make it positive
                if (knob.transform.eulerAngles.z > 0)
                {
                    //subtract from 360 (a circle's total degrees) since numbers are reversed
                    dialSlider.value = 360 - knob.transform.eulerAngles.z;
                }
                else
                {
                    dialSlider.value = -(knob.transform.eulerAngles.z);
                }
            }

            //update previous mouse position
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

        //reset mouseRef to keep from turning when knob is clicked again
        mouseRef = new Vector3(0, 0, 0);
    }

    //public void ChangeChannel(float knobRotation)
    //{
    //    if ((knobRotation > 175 && knobRotation < 180) || (knobRotation < -5 && knobRotation >= 0))
    //    {
    //        currentRadio.SetChannel(RadioChannel.Music);
    //    }
    //    else
    //    {
    //        currentRadio.SetChannel(RadioChannel.Null);
    //    }
    //}
}

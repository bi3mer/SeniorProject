using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum ButtonType {VolumeUp, VolumeDown, Power};

public class RadioButton : MonoBehaviour
{ 
    [SerializeField]
    private ButtonType buttonType;
    [SerializeField]
    private Radio radio;

    [SerializeField]
    InteractableRadioModel radioModelAnimation;

    /// <summary>
    /// When clicked, do button animation based on button type.
    /// </summary>
    void OnMouseDown()
    {
        if (buttonType == ButtonType.VolumeUp)
        {
            radioModelAnimation.PushVolumeUpButton();
        }
        else if (buttonType == ButtonType.VolumeDown)
        {
            radioModelAnimation.PushVolumeDownButton();
        }
        else if (buttonType == ButtonType.Power)
        {
            radioModelAnimation.PushPowerButton();
            radio.Power();
        }
    }
}

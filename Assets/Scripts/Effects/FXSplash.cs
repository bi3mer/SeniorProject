using UnityEngine;
using System.Collections;

public class FXSplash : MonoBehaviour
{
    /// <summary>
    /// An Animation Event, called at the end of the animation to hide the effect.
    /// </summary>
    void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Hide immediately when created!
    /// </summary>
    void Start()
    {
        Hide();
    }
}

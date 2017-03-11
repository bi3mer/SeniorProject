using UnityEngine;
using System.Collections;

public class FXSplash : MonoBehaviour
{
    public Animator SplashAnimator;

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
        if(SplashAnimator == null)
        {
            SplashAnimator = GetComponent<Animator>();
        }
        Hide();
    }

    /// <summary>
    /// Set the animation speed of the splash
    /// </summary>
    /// <param name="speed">How fast the animation should play, default is 1</param>
    public void setAnimationSpeed(float speed)
    {
        SplashAnimator.speed = speed;
    }
}

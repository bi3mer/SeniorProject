using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EndingController : MonoBehaviour
{
    [Tooltip("The trigger the player enters for the ending to start.")]
    [SerializeField]
    private Collider endingTrigger;

    [Tooltip("Time for the camera to tween to the ending position")]
    [SerializeField]
    private Collider deathTrigger;

    [Tooltip("Gameobjects to set inactive when the ending starts")]
    [SerializeField]
    public GameObject[] hideThese;

    [Tooltip("Scripts to disable when the ending starts.")]
    [SerializeField]
    public  MonoBehaviour[] disableThese;

    [Tooltip("Time to wait before the ladder drops")]
    [SerializeField]
    private float timeToWait;

    [Tooltip("Position for the camera during the ending cutscene")]
    [SerializeField]
    private Transform cameraPosition;

    [Tooltip("Time for the camera to tween to the ending position")]
    [SerializeField]
    private float cameraTweenTime;


    [Tooltip("The ladder gameobject")]
    [SerializeField]
    private GameObject ladder;

    [Tooltip("Position for the ladder to fall to")]
    [SerializeField]
    private Transform ladderPosition;

    [Tooltip("Time for the ladder to fall")]
    [SerializeField]
    private float ladderDropTime;

    [Tooltip("Time after the ladder falls or the player jumps off to wait to fade out")]
    [SerializeField]
    private float waitBeforeEndingFadeOut;

    [Tooltip("The fader that lets the game fade out to white")]
    [SerializeField]
    private FaderManager fader;

    [Tooltip("Time the ending fade takes")]
    [SerializeField]
    private float endingFadeTime;

    private bool endingActivated = false;

    private const string playerTag = "Player";

    /// <summary>
    /// Called to trigger the ending
    /// </summary>
    /// <param name="other">The collider that entered one of the ending triggers, should be the player</param>
    /// /// <param name="deathrigger">Did the player enter the death trigger?</param>
    public void EndingTrigger(Collider other, bool deathTrigger)
    {
        if (other.CompareTag(playerTag) && !endingActivated && !deathTrigger)
        {
            endingActivated = true;

            // Disable everything we want to be disabled.
            for (int i = 0; i < hideThese.Length; ++i)
            {
                hideThese[i].SetActive(false);
                 
            }
            for (int i = 0; i < disableThese.Length; ++i)
            {
                disableThese[i].enabled = false;
            }

            // Rotate the camera, and move it to the ending zone
            Camera.main.transform.DOMove(cameraPosition.position, cameraTweenTime);
            Camera.main.transform.DORotate(cameraPosition.eulerAngles, cameraTweenTime);
            Camera.main.GetComponent<CameraController>().EndingTriggered = true;
        }
        else if (other.CompareTag(playerTag) && deathTrigger)
        {
            StartCoroutine(SuicideCoroutine());
        }
    }

    /// <summary>
    /// Called by the beacon script to signify that the ending has begun, and the player needs to wait for rescue.
    /// </summary>
    public void BeaconTurnedOn()
    {
        StartCoroutine(EndingCoroutine());
    }

    /// <summary>
    /// The coroutine for the ending if the player doesn't kill themselves.
    /// </summary>
    /// <returns></returns>
    IEnumerator EndingCoroutine()
    {
        // Don't let player be kill
        // TODO: Change all instances of this line to be handled by calling a function that stops the players health from updating at all.
        Game.Instance.PlayerInstance.Health = 100;
        
        // Wait for rescue.
        yield return new WaitForSeconds(timeToWait);

        // Lower ladder
        ladder.transform.DOMove(ladderPosition.position, ladderDropTime);

        // Don't let player be kill
        Game.Instance.PlayerInstance.Health = 100;

        // Wait for the ladder to drop
        yield return new WaitForSeconds(ladderDropTime + waitBeforeEndingFadeOut);

        fader.EndGameFade(endingFadeTime);

        // Wait for the fade to finish
        yield return new WaitForSeconds(endingFadeTime + waitBeforeEndingFadeOut);

        // Show the main menu button
        fader.ShowMainMenuButton();
    }

    /// <summary>
    /// The coroutine for the ending if the player decides to kill themselves.
    /// </summary>
    /// <returns></returns>
    IEnumerator SuicideCoroutine()
    {
        StopCoroutine(EndingCoroutine());

        fader.EndGameFade(endingFadeTime);

        // Wait for the fade to finish
        yield return new WaitForSeconds(endingFadeTime + waitBeforeEndingFadeOut);

        // Show the main menu button
        fader.ShowMainMenuButton();
    }
}

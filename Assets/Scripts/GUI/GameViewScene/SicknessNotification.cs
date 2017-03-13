using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class SicknessNotification : MonoBehaviour
{
    [SerializeField]
    private GameObject notificationBar;
    [SerializeField]
    private Text text; 

    [SerializeField]
    private float endingPosition;

    [SerializeField]
    [Tooltip ("How long it takes for the notification bar to get to it's final position.")]
    private float enterTime;
    [SerializeField]
    [Tooltip("How long it takes for the notification bar to get to it's exit the screen.")]
    private float exitTime;

    [SerializeField]
    private string foodPoisoningText;
    [SerializeField]
    private string pneumoniaText;
    [SerializeField]
    private string noSicknessText;

    // for testing purposes
    [SerializeField]
    private PlayerHealthStatus tester;

    private Vector3 startingPosition;
    private Player player;
    private PlayerHealthStatus prevStatus;

    /// <summary>
    /// Set starting position of the notification bar.
    /// </summary>
    void Start ()
    {
        startingPosition = notificationBar.transform.localPosition;
        player = Game.Player;
	}
	
	/// <summary>
    /// Drop down notification bar if player becomes sick.
    /// </summary>
	void Update ()
    {
        player.HealthStatus = tester;

        // check for if there is a status change
        if (prevStatus != player.HealthStatus)
        {
            // close the notification bar before changing text
            if (notificationBar.transform.localPosition.y == endingPosition)
            {
                CloseNotification();
            }

            if (player.HealthStatus == PlayerHealthStatus.FoodPoisoning)
            {
                text.text = foodPoisoningText;
            }
            else if (player.HealthStatus == PlayerHealthStatus.Pneumonia)
            {
                text.text = pneumoniaText;
            }
            else
            {
                text.text = noSicknessText;
            }
            notificationBar.transform.DOLocalMoveY(endingPosition, enterTime);
        }

        prevStatus = player.HealthStatus;
    }

    /// <summary>
    /// Closes out notification bar.
    /// </summary>
    public void CloseNotification()
    {
        notificationBar.transform.DOLocalMoveY(startingPosition.y, exitTime);
    }
}

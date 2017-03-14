using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class SicknessNotification : MonoBehaviour
{
    [Header("Game Object Settings")]
    [SerializeField]
    private GameObject notificationBar;
    [SerializeField]
    private Text text;

    [Header("Animation Settings")]
    [SerializeField]
    [Tooltip ("How long it takes for the notification bar to get to it's final position.")]
    private float enterTime;
    [SerializeField]
    [Tooltip("How long it takes for the notification bar to get to it's exit the screen.")]
    private float exitTime;

    [Header("Anchor Settings")]
    [SerializeField]
    private float maxAnchorX;
    [SerializeField]
    private float maxAnchorY;
    [SerializeField]
    private float minAnchorX;
    [SerializeField]
    private float minAnchorY;

    [Header("Text Settings")]
    [SerializeField]
    private string foodPoisoningText;
    [SerializeField]
    private string pneumoniaText;
    [SerializeField]
    private string noSicknessText;

    private Vector2 endingAnchorMax;
    private Vector2 endingAnchorMin;

    private Vector3 startingPosition;
    private Vector3 endingPosition;
    private Player player;
    private PlayerHealthStatus prevStatus;

    /// <summary>
    /// Set starting position of the notification bar.
    /// </summary>
    void Start ()
    {
        startingPosition = notificationBar.transform.localPosition;

        // define ending anchors
        endingAnchorMax = new Vector2(maxAnchorX, maxAnchorY);
        endingAnchorMin = new Vector2(minAnchorX, minAnchorY);

        // set up ending position by placing the notification bar based on the ending anchors
        notificationBar.GetComponent<RectTransform>().anchorMax = endingAnchorMax;
        notificationBar.GetComponent<RectTransform>().anchorMin = endingAnchorMin;
        endingPosition = notificationBar.transform.localPosition;

        // remove the notification bar before the loading screen finishes
        CloseNotification();

        player = Game.Player;
	}
	
	/// <summary>
    /// Drop down notification bar if player becomes sick.
    /// </summary>
	void Update ()
    {
        // check for if there is a status change
        if (prevStatus != player.HealthStatus)
        {
            // close the notification bar before changing text
            if (notificationBar.transform.localPosition == endingPosition)
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
       
            notificationBar.transform.DOLocalMove(endingPosition, enterTime);
        }

        prevStatus = player.HealthStatus;
    }

    /// <summary>
    /// Closes out notification bar.
    /// </summary>
    public void CloseNotification()
    {
        notificationBar.transform.DOLocalMove(startingPosition, exitTime);
    }
}

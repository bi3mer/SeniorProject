using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class PlayerNotification : MonoBehaviour
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
    [SerializeField]
    private string inventoryFullText;
    [SerializeField]
    private string rodBreakText;
    [SerializeField]
    private string caughtFishText;

    private Vector2 endingAnchorMax;
    private Vector2 endingAnchorMin;

    private Vector3 startingPosition;
    private Vector3 endingPosition;

    Dictionary<NotificationType, string> notificationsToTypes;

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

		notificationsToTypes = new Dictionary<NotificationType, string>();

		notificationsToTypes.Add(NotificationType.STOMACH, foodPoisoningText);
		notificationsToTypes.Add(NotificationType.PNEUMONIA, pneumoniaText);
		notificationsToTypes.Add(NotificationType.CURE, noSicknessText);
		notificationsToTypes.Add(NotificationType.INVENTORYFULL, inventoryFullText);
        notificationsToTypes.Add(NotificationType.BREAK, rodBreakText);
        notificationsToTypes.Add(NotificationType.CAUGHTFISH, caughtFishText);

        // remove the notification bar before the loading screen finishes
        CloseNotification();

        GuiInstanceManager.PlayerNotificationInstance = this;
	}
	
	/// <summary>
	/// Shows the notification.
	/// </summary>
	/// <param name="notification">Type of notification.</param>
	public void ShowNotification (NotificationType notification)
    {
        // close the notification bar before changing text
        if (notificationBar.transform.localPosition == endingPosition)
        {
            CloseNotification();
        }

		text.text = notificationsToTypes[notification];
   
        notificationBar.transform.DOLocalMove(endingPosition, enterTime);
    }

    /// <summary>
    /// Closes out notification bar.
    /// </summary>
    public void CloseNotification()
    {
        notificationBar.transform.DOLocalMove(startingPosition, exitTime);
    }
}

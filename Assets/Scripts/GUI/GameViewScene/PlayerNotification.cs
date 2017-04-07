using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

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

    private Vector2 endingAnchorMax;
    private Vector2 endingAnchorMin;

    private Vector3 startingPosition;
    private Vector3 endingPosition;
    private Player player;

    public enum NotificationType { STOMACH, PNEUMONIA, CURE, INVENTORYFULL };
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

        GuiInstanceManager.PlayerNotificationInsance = this;
	}
	
	/// <summary>
    /// Drop down notification bar if player becomes sick.
    /// </summary>
	public void ShowNotification (NotificationType notification)
    {
        // close the notification bar before changing text
        if (notificationBar.transform.localPosition == endingPosition)
        {
            CloseNotification();
        }

		if (notification == NotificationType.STOMACH)
        {
            text.text = foodPoisoningText;
        }
        else if (notification == NotificationType.PNEUMONIA)
        {
            text.text = pneumoniaText;
        }
        else if(notification == NotificationType.CURE)
        {
            text.text = noSicknessText;
        }
        else if(notification == NotificationType.INVENTORYFULL)
        {
        	text.text = inventoryFullText;
        }
   
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

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class SicknessNotification : MonoBehaviour
{
    [SerializeField]
    private bool playerSick;

    [SerializeField]
    private GameObject notifBar;
    [SerializeField]
    private Text text; 

    [SerializeField]
    private float endingPosition;

    [SerializeField]
    private float enterTime;
    [SerializeField]
    private float exitTime;

    private Vector3 startingPosition;
    private Player player;

    private const string foodPoisoningText = "Oh no! You have food poisioning!";
    private const string pneumoniaText = "Oh no! You have pneumonia!";

    // Use this for initialization
    void Start ()
    {
        startingPosition = notifBar.transform.localPosition;

        player = Game.Player;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (playerSick)
        {
            if (player.HealthStatus == PlayerHealthStatus.FoodPoisoning)
            {
                text.text = foodPoisoningText;
            }
            else if (player.HealthStatus == PlayerHealthStatus.Pneumonia)
            {
                text.text = pneumoniaText;
            }

            notifBar.transform.DOLocalMoveY(endingPosition, enterTime);
        }
	}

    public void CloseNotification()
    {
        notifBar.transform.DOLocalMoveY(startingPosition.y, exitTime);
        playerSick = false;
    }
}

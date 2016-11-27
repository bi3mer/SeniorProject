using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndCreditsBehavior : MonoBehaviour 
{
	[SerializeField]
	private Text credits;
	[SerializeField]
	private GameObject scrollContentPanel;
	private bool scroll = true;
	private const int SPEED = 40;
	private const int MIN_Y_POSITION = -263;

	/// <summary>
	/// Update the text scroll.
	/// </summary>
	void Update () 
	{
		if (!scroll) 
		{
			return;
		}

		scrollContentPanel.transform.Translate (Vector3.up * Time.deltaTime * SPEED);
		if (scrollContentPanel.transform.position.y < MIN_Y_POSITION) 
		{
			scroll = false;
		}
	}
}

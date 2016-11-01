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
	private int speed = 40;
	private int minPanelPositionY = -263;

	/// <summary>
	/// Update the text scroll.
	/// </summary>
	void Update () 
	{
		if (!scroll) 
		{
			return;
		}

		scrollContentPanel.transform.Translate (Vector3.up * Time.deltaTime * speed);
		if (scrollContentPanel.transform.position.y < minPanelPositionY) 
		{
			scroll = false;
		}
	}
}

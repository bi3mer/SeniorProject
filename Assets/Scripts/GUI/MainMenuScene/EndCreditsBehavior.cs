using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndCreditsBehavior : MonoBehaviour 
{
	[SerializeField]
	private int speed = 2;
	[SerializeField]
	private Text credits;
	private bool scroll = false;

	/// <summary>
	/// Initialize text component.
	/// </summary>
	void Start () 
	{
		string creditsText = "jdofjoijgiofdjg   jfjdogj s jdfjd\n";
		creditsText += "jdisojfoisdjfg jiojfodg\n";
		credits.text = creditsText;
	}
	
	/// <summary>
	/// Update the text scroll.
	/// </summary>
	void Update () 
	{
		if (!scroll)
			return;
		transform.Translate (Vector3.up * Time.deltaTime * speed);
		if (gameObject.transform.position.y > .8) 
		{
			scroll = false;
		}
	}
}

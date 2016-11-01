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
	/// Initialize text component.
	/// </summary>
	void Start () 
	{
		//string creditsText = "jdofjoijgiofdjg   jfjdogj s jdfjd\n";
		//creditsText += "jdisojfoisdjfg jiojfodg\n";
		credits.text = "\n\nONELorem ipsum dolor sit amet, consectetur adipiscing elit. " +
		"TWOSed convallis neque non justo dapibus suscipit. Fusce fermentum non velit nec tristique. " +
		"THREEPellentesque pulvinar aliquam nibh, sed iaculis tortor semper id. Vivamus bibendum erat nisi," +
		"FOURvitae aliquet eros luctus ac. Donec pulvinar nisl ut risus elementum bibendum. Donec feugiat, " +
		"diam eu fermentum fringilla, purus enim vestibulum sapien, sit amet tempus ante nisi fringilla " +
		"magna. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. " +
		"Morbi viverra imperdiet mauris in venenatis. Ut luctus neque in faucibus sodales.\n\nNulla auctor nec " +
		"justo posuere vulputate. Proin finibus eget arcu vitae consectetur. Praesent feugiat consequat mauris, " +
		"FIVEac posuere augue tincidunt nec. Morbi ullamcorper est auctor nunc fermentum vulputate. Integer quis justo " +
		"fringilla, tempus dui eu, gravida mi. Suspendisse placerat sagittis sollicitudin. Pellentesque " +
		"habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Cras quis " +
		"tincidunt massa. Fusce lobortis nulla ut risus dignissim, ac pellentesque leo faucibus. Sed " +
		"consequat ex sed ligula sodales fermentum.\n\nAliquam pellentesque nisl luctus lacus iaculis " +
		"ultricies. In hac habitasse platea dictumst. Nullam eleifend eros quam. Sed consequat sollicitudin " +
		"SIXnisl, efficitur tempor elit eleifend id. Phasellus vehicula, urna et facilisis hendrerit, " +
		"justo elit elementum odio, et ultrices mi massa id orci. Vestibulum sed cursus ex. Curabitur " +
		"quis viverra erat.\n\nVivamus fermentum sapien neque, nec consequat nisl bibendum non. Duis " +
		"hendrerit lacus sit amet hendrerit euismod. Nullam at mattis erat. Suspendisse ac metus in elit " +
		"egestas vulputate in pellentesque sapien. Morbi nec sodales eros, eget posuere erat. Integer et" +
		" ipsum non neque aliquet commodo. Cras quis condimentum enim. Etiam fringilla malesuada magna. " +
		"Pellentesque elementum urna elit, nec malesuada dolor vestibulum sed. Nulla porta erat sed " +
		"volutpat iaculis.\n\nNunc massa orci, rutrum et feugiat in, sollicitudin vitae purus. Curabitur " +
		"semper posuere elementum. Cras tempus diam interdum tellus fringilla, sed ullamcorper eros vehicula. ";
	}

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

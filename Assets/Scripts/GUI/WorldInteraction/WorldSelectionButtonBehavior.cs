using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class WorldSelectionButtonBehavior : MonoBehaviour 
{
	[SerializeField]
	[Tooltip("The text display")]
	private Text nameDisplay;

	/// <summary>
	/// Gets or sets the name of the item.
	/// </summary>
	/// <value>The name of the item.</value>
	public string ItemName
	{
		get
		{
			return nameDisplay.text;
		}
		set
		{
			nameDisplay.text = value;
		}
	}
}

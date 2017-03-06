using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class WorldSelectionButtonBehavior : MonoBehaviour 
{
	[SerializeField]
	[Tooltip("The text display")]
	private Text nameDisplay;

	[SerializeField]
	[Tooltip("The button handler")]
	private Button associatedButton;

	/// <summary>
	/// Gets or sets the name of the item.
	/// </summary>
	/// <value>The name of the item.</value>
	public string ButtonName
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

	/// <summary>
	/// Sets the action to the button's onclick listener.
	/// </summary>
	/// <param name="action">Action.</param>
	public void SetAction(UnityAction action)
	{
		associatedButton.onClick.RemoveAllListeners();
		associatedButton.onClick.AddListener(action);
	}
}

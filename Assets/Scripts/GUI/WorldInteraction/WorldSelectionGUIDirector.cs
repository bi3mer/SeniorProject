using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WorldSelectionGUIDirector : MonoBehaviour 
{
	[SerializeField]
	[Tooltip("Entire panel display")]
	private GameObject displayPanel;

	[SerializeField]
	[Tooltip("The panel in which the selection ui objects should go")]
	private GameObject worldSelectionPanel;

	[SerializeField]
	[Tooltip("The template for selection ui objects")]
	private GameObject worldSelectionTemplate;

	[SerializeField]
	[Tooltip("UI Text item that appears when there is no valid item")]
	private Text noValidItemText;

	private List<WorldSelectionButtonBehavior> options;

	private HasSelectionInterface requester;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake () 
	{
		GuiInstanceManager.WorldSelectionGuiInstance = this;
	}

	/// <summary>
	/// Displays the item options.
	/// </summary>
	/// <param name="itemType">Item type.</param>
	/// <param name="requestingItem">Requesting item.</param>
	public void DisplayItemOptions(string itemType, HasSelectionInterface requestingItem)
	{
		List<string> itemNames = Game.Instance.PlayerInstance.Inventory.GetItemsByType(itemType);
		options = new List<WorldSelectionButtonBehavior>();

		for(int i = 0; i < itemNames.Count; ++i)
		{
			WorldSelectionButtonBehavior option = GameObject.Instantiate(worldSelectionTemplate).GetComponent<WorldSelectionButtonBehavior>();
			option.ItemName = itemNames[i];
			option.transform.SetParent(worldSelectionPanel.transform, false);
			option.gameObject.SetActive(true);
			options.Add(option);
		}

		if(options.Count <= 0)
		{
			noValidItemText.gameObject.SetActive(true);
		}

		displayPanel.SetActive(true);

		requester = requestingItem;
	}

	/// <summary>
	/// Finishs the selection.
	/// </summary>
	/// <param name="selected">Selected.</param>
	public void FinishSelection(WorldSelectionButtonBehavior selected)
	{
		requester.ItemSelectedCallbackAction(selected.ItemName);
		CloseSelection();
	}

	/// <summary>
	/// Closes the selection panel. Clears options.
	/// </summary>
	public void CloseSelection()
	{
		for(int i = 0; i < options.Count; ++i)
		{
			GameObject.Destroy(options[i].gameObject);
		}

		options.Clear();
		displayPanel.SetActive(false);
		noValidItemText.gameObject.SetActive(false);
	}
}

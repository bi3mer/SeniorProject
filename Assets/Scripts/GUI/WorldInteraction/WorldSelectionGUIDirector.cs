using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

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
	[Tooltip("UI Text item that appears when there is no valid option")]
	private Text noValidOptionText;

	[SerializeField]
	[Tooltip("Text displayed when there are not valid actions to be selected")]
	private string noValidActionText = "No valid actions";

	private List<WorldSelectionButtonBehavior> options;

	private OverworldItemOptionSelection requester;

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
	public void DisplayItemOptions(string itemType, OverworldItemOptionSelection requestingItem)
	{
		List<string> itemNames = Game.Instance.PlayerInstance.Inventory.GetItemsByType(itemType);
		options = new List<WorldSelectionButtonBehavior>();

		for(int i = 0; i < itemNames.Count; ++i)
		{
			string item = itemNames[i];
			options.Add(createWorldSelectionButton(itemNames[i], new UnityAction(delegate {FinishSelection(item); })));
		}

		if(options.Count <= 0)
		{
			noValidOptionText.text = noValidActionText;
			noValidOptionText.gameObject.SetActive(true);
		}

		displayPanel.SetActive(true);

		requester = requestingItem;
	}

	/// <summary>
	/// Displaies the actions.
	/// </summary>
	/// <param name="actions">Actions.</param>
	/// <param name="requestingItem">Requesting item.</param>
	public void DisplayActions(List<ItemAction> actions, OverworldItemOptionSelection requestingItem)
	{
		bool conditionsFulfilled = true;
		options = new List<WorldSelectionButtonBehavior>();

		for(int i = 0; i < actions.Count; ++i)
		{
			conditionsFulfilled = true;

			for(int j = 0; j < actions[i].Conditions.Count; ++j)
			{
				ItemCondition condition = actions [i].Conditions [j];

				if(!condition.CheckCondition())
				{
					conditionsFulfilled = false;
				}
			}

			if(conditionsFulfilled)
			{
				string name = actions[i].ActionName;
				options.Add(createWorldSelectionButton(name, new UnityAction(delegate {FinishSelection(name); })));
			}
		}

		if(options.Count <= 0)
		{
			noValidOptionText.text = noValidActionText;
			noValidOptionText.gameObject.SetActive(true);
		}

		displayPanel.SetActive(true);
		requester = requestingItem;
	}

	/// <summary>
	/// Creates a world selection button and assigns the action to it's onclick.
	/// </summary>
	/// <returns>The world selection button.</returns>
	/// <param name="buttonName">Button name.</param>
	/// <param name="desiredAction">Desired action.</param>
	private WorldSelectionButtonBehavior createWorldSelectionButton(string buttonName, UnityAction desiredAction)
	{
		WorldSelectionButtonBehavior option = GameObject.Instantiate(worldSelectionTemplate).GetComponent<WorldSelectionButtonBehavior>();
		option.ButtonName = buttonName;
		option.transform.SetParent(worldSelectionPanel.transform, false);
		option.gameObject.SetActive(true);
		option.SetAction(desiredAction);

		return option;
	}

	/// <summary>
	/// Finishs the selection.
	/// </summary>
	/// <param name="selected">Selected.</param>
	public void FinishSelection(string selectedItem)
	{
		requester.OptionSelectedCallbackAction(selectedItem);
	}

	/// <summary>
	/// Closes the selection panel. Clears options.
	/// </summary>
	public void CloseSelection()
	{
		ClearOptions();
		displayPanel.SetActive(false);
		noValidOptionText.gameObject.SetActive(false);
	}

	/// <summary>
	/// Clears the options.
	/// </summary>
	public void ClearOptions()
	{
		for(int i = 0; i < options.Count; ++i)
		{
			GameObject.Destroy(options[i].gameObject);
		}

		options.Clear();
	}
}

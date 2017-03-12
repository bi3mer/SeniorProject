using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class WorldSelectionGUIDirector : MonoBehaviour 
{
	[SerializeField]
	[Tooltip("Entire panel display that is used to display items on the center of the screen")]
	private GameObject screenDisplayPanel;

	[SerializeField]
	[Tooltip("The panel in the center of the screen in which the selection ui objects should go")]
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
	/// <param name="itemTypes">Items of types that should be displayed.</param>
	/// <param name="requestingItem">Requesting item that stores selected options.</param>
	/// <param name="targetPanel">Target panel where the options will appear.</param>
	public void DisplayItemOptions(List<string> itemTypes, OverworldItemOptionSelection requestingItem, GameObject targetPanel = null)
	{
		if(targetPanel == null)
		{
			targetPanel = worldSelectionPanel;
			screenDisplayPanel.SetActive(true);
		}

		List<string> itemNames = Game.Instance.PlayerInstance.Inventory.GetItemsByType(itemTypes);

		for(int i = 0; i < itemNames.Count; ++i)
		{
			string item = itemNames[i];
			createWorldSelectionButton(itemNames[i], new UnityAction(delegate {FinishSelection(item); }), targetPanel);
		}

		if(targetPanel.transform.childCount <= 0)
		{
			noValidOptionText.text = noValidActionText;
			noValidOptionText.gameObject.SetActive(true);
		}

		requester = requestingItem;
	}

	/// <summary>
	/// Displays the items possible actions.
	/// </summary>
	/// <param name="actions">Actions that should be displayed.</param>
	/// <param name="requestingItem">Requesting item that stores the selected options.</param>
	/// <param name="targetPanel">Target panel where the options will appear.</param>
	public void DisplayActions(List<ItemAction> actions, OverworldItemOptionSelection requestingItem, GameObject targetPanel = null)
	{
		bool conditionsFulfilled = true;

		if(targetPanel == null)
		{
			targetPanel = worldSelectionPanel;
			screenDisplayPanel.SetActive(true);
		}

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
				createWorldSelectionButton(name, new UnityAction(delegate {FinishSelection(name); }), targetPanel);
			}
		}

		if(targetPanel.transform.childCount <= 0)
		{
			noValidOptionText.text = noValidActionText;
			noValidOptionText.gameObject.SetActive(true);
		}

		requester = requestingItem;
	}

	/// <summary>
	/// Creates a world selection button and assigns the action to it's onclick.
	/// </summary>
	/// <returns>The world selection button.</returns>
	/// <param name="buttonName">Text that should appear on button.</param>
	/// <param name="desiredAction">Desired action that occurs onclick.</param>
	/// <param name="targetPanel">Target panel where button should appear.</param>
	private WorldSelectionButtonBehavior createWorldSelectionButton(string buttonName, UnityAction desiredAction, GameObject targetPanel)
	{
		WorldSelectionButtonBehavior option = GameObject.Instantiate(worldSelectionTemplate).GetComponent<WorldSelectionButtonBehavior>();
		option.ButtonName = buttonName;
		option.transform.SetParent(targetPanel.transform, false);
		option.gameObject.SetActive(true);
		option.SetAction(desiredAction);

		return option;
	}

	/// <summary>
	/// Finishs the selection.
	/// </summary>
	/// <param name="selectedItem">Selected.</param>
	public void FinishSelection(string selectedItem)
	{
		requester.OptionSelectedCallbackAction(selectedItem);
	}

	/// <summary>
	/// Closes the selection.
	/// </summary>
	/// <param name="targetContent">Target content panel that contains options.</param>
	/// <param name="targetHolder">Target holder panel that contains the display panel.</param>
	public void CloseSelection(GameObject targetContent, GameObject targetHolder)
	{
		noValidOptionText.gameObject.SetActive(false);
		ClearOptions(targetContent);

		if(targetHolder != null)
		{
			targetHolder.SetActive(false);
		}
		else
		{
			screenDisplayPanel.SetActive(false);
		}
	}

	/// <summary>
	/// Clears the options from the target panel.
	/// </summary>
	/// <param name="targetPanel">Target panel.</param>
	public void ClearOptions(GameObject targetPanel)
	{
		GameObject target = targetPanel;

		if(target == null)
		{
			targetPanel = worldSelectionPanel;
		}

		for(int i = 0; i < targetPanel.transform.childCount; ++i)
		{
			GameObject.Destroy(targetPanel.transform.GetChild(i).gameObject);
		}
	}
}

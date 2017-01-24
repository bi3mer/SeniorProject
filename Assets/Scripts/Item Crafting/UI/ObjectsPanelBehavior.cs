using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ObjectsPanelBehavior : MonoBehaviour 
{
	private int currentAnimationState = 1;

	private int closeState = 2;
	private int openState = 1;
	private string animatorStateVariable = "state";

	private Animator anim;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		anim = GetComponent<Animator>();
	}

	/// <summary>
	/// Open the item info panel for the specified item.
	/// </summary>
	/// <param name="item">Item to view.</param>
	///  <param name="numToModify">Number to modify.</param>
	public void OpenPanel()
	{
		// when the animation state is 1, the panel will open
		currentAnimationState = openState;

		anim.SetInteger (animatorStateVariable, currentAnimationState);
	}

	/// <summary>
	/// Closes the item info panel.
	/// </summary>
	public void ClosePanel()
	{
		currentAnimationState = closeState;
		anim.SetInteger (animatorStateVariable, currentAnimationState);

		ItemDiscarder discarder = new ItemDiscarder();
		discarder.DiscardItems(GuiInstanceManager.InventoryUiInstance.ItemsToDiscard);

		GuiInstanceManager.InventoryUiInstance.ItemsToDiscard.Clear();

		// deselects all ui components to prevent space bar from firing them off
		EventSystem.current.SetSelectedGameObject(null);
	}
}

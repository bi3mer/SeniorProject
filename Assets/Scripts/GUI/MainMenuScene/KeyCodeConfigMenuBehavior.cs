using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class KeyCodeConfigMenuBehavior : MonoBehaviour 
{
	// key buttons for each user input
	[SerializeField]
	private Button forwardKeyButton;
	[SerializeField]
	private Button backKeyButton;
	[SerializeField]
	private Button rightKeyButton;
	[SerializeField]
	private Button leftKeyButton;
	[SerializeField]
	private Button forwardSecKeyButton;
	[SerializeField]
	private Button backSecKeyButton;
	[SerializeField]
	private Button rightSecKeyButton;
	[SerializeField]
	private Button leftSecKeyButton;
	[SerializeField]
	private Button sprintKeyButton;
	[SerializeField]
	private Button jumpKeyButton;
	[SerializeField]
	private Button actionKeyButton;
	[SerializeField]
	private Button useToolKeyButton;
	[SerializeField]
	private Button camRightKeyButton;
	[SerializeField]
	private Button camLeftKeyButton;
	[SerializeField]
	private Button pauseKeyButton;
	[SerializeField]
	private Button inventoryKeyButton;
	[SerializeField]
	private Button radioKeyButton;
	[SerializeField]
	private Button craftingKeyButton;

	// Text Components of each button
	private Text forwardText;
	private Text backText;
	private Text rightText;
	private Text leftText;
	private Text forwardSecText;
	private Text backSecText;
	private Text rightSecText;
	private Text leftSecText;
	private Text sprintText;
	private Text jumpText;
	private Text actionText;
	private Text useToolText;
	private Text camRightText;
	private Text camLeftText;
	private Text pauseText;
	private Text inventoryText;
	private Text radioText;
	private Text craftingText;

	private ControlScheme controlSchemeInstance;
	private bool waitingForUserInputKey;
	private Event keyEvent;
	private KeyCode newKeyCode;

	/// <summary>
	/// Start instance of key code config panel.
	/// </summary>
	void Start()
	{
		// set text elements for button labels
		controlSchemeInstance = Game.Instance.GameSettingsInstance.Scheme;
		forwardText = forwardKeyButton.GetComponentInChildren<Text>();
		backText = backKeyButton.GetComponentInChildren<Text> ();
		rightText = rightKeyButton.GetComponentInChildren<Text> ();
		leftText = leftKeyButton.GetComponentInChildren<Text> ();
		forwardSecText = forwardSecKeyButton.GetComponentInChildren<Text>();
		backSecText = backSecKeyButton.GetComponentInChildren<Text> ();
		rightSecText = rightSecKeyButton.GetComponentInChildren<Text> ();
		leftSecText = leftSecKeyButton.GetComponentInChildren<Text> ();
		sprintText = sprintKeyButton.GetComponentInChildren<Text>();
		jumpText = jumpKeyButton.GetComponentInChildren<Text> ();
		actionText = actionKeyButton.GetComponentInChildren<Text> ();
		useToolText = useToolKeyButton.GetComponentInChildren<Text> ();
		camRightText = camRightKeyButton.GetComponentInChildren<Text> ();
		camLeftText = camLeftKeyButton.GetComponentInChildren<Text> ();
		pauseText = pauseKeyButton.GetComponentInChildren<Text> ();
		inventoryText = inventoryKeyButton.GetComponentInChildren<Text> ();
		radioText = radioKeyButton.GetComponentInChildren<Text> ();
		craftingText = craftingKeyButton.GetComponentInChildren<Text> ();

		// set initial button labels from set keycodes
		forwardText.text = controlSchemeInstance.Forward.ToString();
		backText.text = controlSchemeInstance.Back.ToString();
		rightText.text = controlSchemeInstance.Right.ToString();
		leftText.text = controlSchemeInstance.Left.ToString();
		forwardSecText.text = controlSchemeInstance.ForwardSecondary.ToString();
		backSecText.text = controlSchemeInstance.BackSecondary.ToString();
		rightSecText.text = controlSchemeInstance.RightSecondary.ToString();
		leftSecText.text = controlSchemeInstance.LeftSecondary.ToString();
		sprintText.text = controlSchemeInstance.Sprint.ToString();
		jumpText.text = controlSchemeInstance.Jump.ToString();
		actionText.text = controlSchemeInstance.Action.ToString();
		useToolText.text = controlSchemeInstance.UseTool.ToString();
		camRightText.text = controlSchemeInstance.CameraRight.ToString();
		camLeftText.text = controlSchemeInstance.CameraLeft.ToString();
		pauseText.text = controlSchemeInstance.Pause.ToString();
		inventoryText.text = controlSchemeInstance.Inventory.ToString();
		radioText.text = controlSchemeInstance.Radio.ToString();
		craftingText.text = controlSchemeInstance.Crafting.ToString();
	}

	/// <summary>
	/// Get user input everytime a GUI event is raised.
	/// </summary>
	void OnGUI()
	{
		keyEvent = Event.current;
		if(keyEvent.isKey && waitingForUserInputKey)
		{
			newKeyCode = keyEvent.keyCode;
			waitingForUserInputKey = false;
		}
	}

	/// <summary>
	/// Waits for input.
	/// </summary>
	/// <returns>The for input.</returns>
	IEnumerator WaitForInput()
	{
		while (!keyEvent.isKey)
		{
			yield return null;
		}
	}

	/// <summary>
	/// Sets the key.
	/// </summary>
	/// <returns>The key.</returns>
	/// <param name="keyButtonPressed">Key button pressed.</param>
	public IEnumerator SetKey(GameObject keyButtonPressed)
	{
		waitingForUserInputKey = true;
		string name = keyButtonPressed.name;
	
		yield return WaitForInput ();

		// update the button labels with user input here
		switch (name) 
		{
		case "ForwardKeyButton":
			forwardText.text = newKeyCode.ToString ();
			controlSchemeInstance.Forward = newKeyCode;
			break;
		case "BackKeyButton":
			backText.text = newKeyCode.ToString ();
			controlSchemeInstance.Back = newKeyCode;
			break;
		case "RightKeyButton":
			rightText.text = newKeyCode.ToString ();
			controlSchemeInstance.Right = newKeyCode;
			break;
		case "LeftKeyButton":
			leftText.text = newKeyCode.ToString ();
			controlSchemeInstance.Left = newKeyCode;
			break;
		case "ForwardSecKeyButton":
			forwardSecText.text = newKeyCode.ToString ();
			controlSchemeInstance.ForwardSecondary = newKeyCode;
			break;
		case "BackSecKeyButton":
			backSecText.text = newKeyCode.ToString ();
			controlSchemeInstance.BackSecondary = newKeyCode;
			break;
		case "RightSecKeyButton":
			rightSecText.text = newKeyCode.ToString ();
			controlSchemeInstance.RightSecondary = newKeyCode;
			break;
		case "LeftSecKeyButton":
			leftSecText.text = newKeyCode.ToString ();
			controlSchemeInstance.LeftSecondary = newKeyCode;
			break;
		case "SprintKeyButton":
			sprintText.text = newKeyCode.ToString ();
			controlSchemeInstance.Sprint = newKeyCode;
			break;
		case "JumpKeyButton":
			jumpText.text = newKeyCode.ToString ();
			controlSchemeInstance.Jump = newKeyCode;
			break;
		case "ActionKeyButton":
			actionText.text = newKeyCode.ToString ();
			controlSchemeInstance.Action = newKeyCode;
			break;
		case "UseToolKeyButton":
			useToolText.text = newKeyCode.ToString ();
			controlSchemeInstance.UseTool = newKeyCode;
			break;
		case "CamRightKeyButton":
			camRightText.text = newKeyCode.ToString ();
			controlSchemeInstance.CameraRight = newKeyCode;
			break;
		case "CamLeftKeyButton":
			camLeftText.text = newKeyCode.ToString ();
			controlSchemeInstance.CameraLeft = newKeyCode;
			break;
		case "PauseKeyButton":
			pauseText.text = newKeyCode.ToString ();
			controlSchemeInstance.Pause = newKeyCode;
			break;
		case "InventoryKeyButton":
			inventoryText.text = newKeyCode.ToString ();
			controlSchemeInstance.Inventory = newKeyCode;
			break;
		case "RadioKeyButton":
			radioText.text = newKeyCode.ToString ();
			controlSchemeInstance.Radio = newKeyCode;
			break;
		case "CraftingKeyButton":
			craftingText.text = newKeyCode.ToString ();
			controlSchemeInstance.Crafting = newKeyCode;
			break;
		}

		yield return null;
	}

	/// <summary>
	/// Sets the button labels.
	/// </summary>
	public void OnKeyButtonClick()
	{
		if (!waitingForUserInputKey) 
		{
			StartCoroutine (SetKey (EventSystem.current.currentSelectedGameObject));
		}
	}

	/// <summary>
	/// Saves user input key codes and closes key code configuration panel.
	/// </summary>
	public void OnCloseClick()
	{
		this.gameObject.SetActive(false);
	}
}

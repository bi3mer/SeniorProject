using UnityEngine;
using System.Collections;

public class InteractableSetUp : MonoBehaviour 
{
	/// <summary>
	/// The name of the item.
	/// </summary>
	[Tooltip("Name of the item. Used to get baseItem.")]
	public string ItemName;

	/// <summary>
	/// The location of the prefab that contains the trigger collider and text mesh needed for interactable objects
	/// </summary>
	private const string triggerObjectLocation = "ItemGeneration/InteractableText";

	/// <summary>
	/// the prefab that contains the trigger collider and text mesh needed for interactable objects
	/// </summary>
	private GameObject triggerObjectPrefab;

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start () 
	{
		triggerObjectPrefab = (GameObject) Resources.Load(triggerObjectLocation);
		setUpItem();
	}

	/// <summary>
	/// Sets up item in world to be a pickup item.
	/// </summary>
	private void setUpItem()
	{
		// create the object with the model
		BaseItem baseItem = Game.Instance.ItemFactoryInstance.GetBaseItem(ItemName);

		// creates the trigger object that will handle interaction with player
		GameObject triggerObject = GameObject.Instantiate(triggerObjectPrefab);
		triggerObject.transform.SetParent(transform);
		triggerObject.transform.localPosition = Vector3.zero;

		PickUpItem pickup = gameObject.AddComponent<PickUpItem>();
        pickup.SetUp();
		pickup.Item = baseItem;
		pickup.Show = false;

		// TODO: Make the amount found in one stack to be a variable number
		pickup.Amount = 1;

		gameObject.name = baseItem.ItemName;

		StartCoroutine(addToWorld());
	}

	private IEnumerator addToWorld()
	{
		while(Game.Instance.ItemPoolInstance == null)
		{
			yield return null;
		}

		Game.Instance.ItemPoolInstance.AddItemFromWorld(this.gameObject);
	}
}

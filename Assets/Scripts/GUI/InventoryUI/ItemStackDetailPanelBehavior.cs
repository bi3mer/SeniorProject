using UnityEngine;
using System.Collections;

public class ItemStackDetailPanelBehavior : MonoBehaviour 
{
	public static ItemStackDetailPanelBehavior instance;

	void Start()
	{
		instance = this;
	}
}

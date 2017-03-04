using UnityEngine;
using System.Collections;

public class Lantern : Tool 
{
	[SerializeField]
    [BaseItemPopup]
    private string toolName;

    [SerializeField]
    private float brightness;

    private Light lightSource;
    private bool on;

	// Use this for initialization
	void Start () 
	{
		on = false;
		lightSource = GetComponentInChildren<Light>();
		ToolName = toolName;
		lightSource.intensity = brightness;
	}

	public bool On
	{
		get
		{
			return on;
		}
		set
		{
			on = value;
			lightSource.enabled = value;
		}
	}

	public override void Equip()
	{
	}

	public override void Unequip()
	{
	}
	
	public override void Use()
	{
		On = !On;
	}
}

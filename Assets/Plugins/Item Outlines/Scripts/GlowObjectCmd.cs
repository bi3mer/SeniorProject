using UnityEngine;
using System.Collections.Generic;

public class GlowObjectCmd : MonoBehaviour
{
	public Color GlowColor;
	public float LerpFactor = 10;

	public Renderer[] Renderers
	{
		get;
		private set;
	}

	public Color CurrentColor
	{
		get { return currentColor; }
	}

	private Color currentColor;
	private Color targetColor;

	void Start()
	{
		Renderers = GetComponentsInChildren<Renderer>();
		GlowController.RegisterObject(this);
	}


	/// <summary>
	/// Changes the glow color so that it is visible to the player.
	/// </summary>
	public void InViewColor()
	{
        targetColor = Color.red;
		enabled = true;
	}

	/// <summary>
	/// Changes the glow color so that it is invisible to the player.
	/// </summary>
	public void OutOfViewColor()
	{
		targetColor = Color.black;
		enabled = true;
	}

	/// <summary>
	/// Update color, disable self if we reach our target color.
	/// </summary>
	private void Update()
	{
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * LerpFactor);

        if (currentColor.Equals(targetColor))
        {
            enabled = false;
        }
    }
}

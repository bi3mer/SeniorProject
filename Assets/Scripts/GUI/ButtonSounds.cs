using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ButtonSounds : MonoBehaviour 
{
	private static ButtonSounds instance;

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static ButtonSounds Instance
	{
		get
		{
			if(ButtonSounds.instance == null)
			{
				ButtonSounds.instance = GameObject.FindObjectOfType<ButtonSounds>();
			}

			return ButtonSounds.instance;
		}
	}

	private AudioSource source;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		this.source = this.GetComponent<AudioSource>();
	}

	/// <summary>
	/// Plays the button sound.
	/// </summary>
	public void PlayButtonSound()
	{
		this.source.Play();
	}
}

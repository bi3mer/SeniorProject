using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenuBehavior : MonoBehaviour 
{
	[SerializeField]
	private Toggle soundToggle;
	[SerializeField]
	private Slider volumeSlider;
	[SerializeField]
	private Button configureKeysButton;
	[SerializeField]
	private InputField procCityGenerationSeedInputField;
	[SerializeField]
	private GameObject inputConfigPanel;
	private GameSettings settings = Game.Instance.GameSettingsInstance;

	/// <summary>
	/// Turns the sound on or off based on toggle value.
	/// </summary>
	/// <param name="isChecked">If set to <c>true</c> is sound on.</param>
	public void turnSoundOnOff()
	{
		if (soundToggle.isOn) 
		{
			settings.SoundOn = true;
		} 
		if (!soundToggle.isOn)
		{
			settings.SoundOn = false;
		}
	}

	/// <summary>
	/// Changes the volume.
	/// </summary>
	/// <param name="value">Value.</param>
	public void changeVolume(float value)
	{
		settings.VolumeValue = value;
	}

	/// <summary>
	/// Validates seed input filed and sets the procedural city generation seed.
	/// </summary>
	public void OnSetSeedClick()
	{
		procCityGenerationSeedInputField.characterValidation = InputField.CharacterValidation.Integer;
		int seed;
		bool inputConvertedToInt = int.TryParse(procCityGenerationSeedInputField.text, out seed);
		if (inputConvertedToInt) 
		{
			settings.ProceduralCityGenerationSeed = seed;
		}
	}

	/// <summary>
	/// Opens the user input configuration panel.
	/// </summary>
	public void OnConfigKeysClick()
	{
		inputConfigPanel.SetActive (true);
	}
}

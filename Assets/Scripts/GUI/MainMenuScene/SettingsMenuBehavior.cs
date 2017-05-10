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

	[Header("Camera Effects Settings")]
	[SerializeField]
	private Toggle depthOfFieldToggle;
	[SerializeField]
	private Toggle chromaticAberrationToggle;
	[SerializeField]
	private Toggle motionBlurToggle;
	[SerializeField]
	private Toggle bloomToggle;

	private GameObject inputConfigPanel;
	private GameSettings settings;
	private string previousSeed;

	/// <summary>
	/// Start this instance of the SettingsMenuBehavior.
	/// </summary>
	void Start()
	{
		settings = Game.Instance.GameSettingsInstance;
		procCityGenerationSeedInputField.onValueChanged.AddListener (OnSeedValueChanged);
		volumeSlider.onValueChanged.AddListener (changeVolume);

		// Add functionality to camera setting toggles
		depthOfFieldToggle.onValueChanged.AddListener(() => {
				settings.Camera.DepthOfFieldEnabled = depthOfFieldToggle.isOn;
			});
		chromaticAberrationToggle.onValueChanged.AddListener(() => {
				settings.Camera.ChromaticAberrationEnabled = chromaticAberrationToggle.isOn;
			});
		motionBlurToggle.onValueChanged.AddListener(() => {
				settings.Camera.MotionBlurEnabled = motionBlurToggle.isOn;
			});
		bloomToggle.onValueChanged.AddListener(() => {
				settings.Camera.BloomEnabled = bloomToggle.isOn;
			});
	}

	/// <summary>
	/// Turns the sound on or off based on toggle value.
	/// </summary>
	/// <param name="isChecked">If set to <c>true</c> is sound on.</param>
	public void turnSoundOnOff()
	{
		settings.SoundOn = soundToggle.isOn;
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
	public void OnSeedValueChanged(string inputSeed)
	{
		int seed;
		bool inputConvertedToInt = int.TryParse(inputSeed, out seed);
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

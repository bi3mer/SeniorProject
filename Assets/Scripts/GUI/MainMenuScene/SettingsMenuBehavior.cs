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
		
		setupCameraToggles();
	}

	///	<summary>
	/// Add functionality to camera setting toggles.
	/// </summary>
	private void setupCameraToggles ()
	{
		depthOfFieldToggle.isOn = settings.Camera.DepthOfFieldEnabled;
		depthOfFieldToggle.onValueChanged.AddListener((bool value) => {
				settings.Camera.DepthOfFieldEnabled = value;
			});

		chromaticAberrationToggle.isOn = settings.Camera.ChromaticAberrationEnabled;
		chromaticAberrationToggle.onValueChanged.AddListener((bool value) => {
				settings.Camera.ChromaticAberrationEnabled = value;
			});

		motionBlurToggle.isOn = settings.Camera.MotionBlurEnabled;
		motionBlurToggle.onValueChanged.AddListener((bool value) => {
				settings.Camera.MotionBlurEnabled = value;
			});

		bloomToggle.isOn = settings.Camera.BloomEnabled;
		bloomToggle.onValueChanged.AddListener((bool value) => {
				settings.Camera.BloomEnabled = value;
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

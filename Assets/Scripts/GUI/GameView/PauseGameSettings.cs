using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///	<summary>
/// Controls settings panel in pause menu
/// </summary>
public class PauseGameSettings : MonoBehaviour 
{
	[SerializeField]
	private GameObject panel;

	[SerializeField]
	private Button closeButton;

	[Header("Sound Settings")]
	[SerializeField]
	private Toggle soundToggle;
	[SerializeField]
	private Slider volumeSlider;

	[Header("Camera Effects Settings")]
	[SerializeField]
	private Toggle depthOfFieldToggle;
	[SerializeField]
	private Toggle chromaticAberrationToggle;
	[SerializeField]
	private Toggle motionBlurToggle;
	[SerializeField]
	private Toggle bloomToggle;

	private GameSettings settings;

	/// <summary>
	/// Set up menu components.
	/// </summary>
	void Start () 
	{
		settings = Game.Instance.GameSettingsInstance;

		closeButton.onClick.AddListener(() => {
				panel.SetActive(false);
			});

		setupSoundSettings();
		setupCameraSettings();
	}

	///	<summary>
	/// Add functionality to sound setting.
	/// </summary>
	private void setupSoundSettings ()
	{
		soundToggle.isOn = settings.SoundOn;
		soundToggle.onValueChanged.AddListener((bool value) => {
				settings.SoundOn = value;
			});

		volumeSlider.value = settings.VolumeValue;
		volumeSlider.onValueChanged.AddListener((float value) => {
				settings.VolumeValue = value;
			});
	}

	///	<summary>
	/// Add functionality to camera setting toggles.
	/// </summary>
	private void setupCameraSettings ()
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
}

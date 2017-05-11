using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.PostProcessing
{
	[ExecuteInEditMode]
	public class PostProcessingSettings : MonoBehaviour 
	{
		private DepthOfFieldModel depthOfField;
		private ChromaticAberrationModel chromaticAberration;
		private MotionBlurModel motionBlur;
		private BloomModel bloom;
		private static PostProcessingSettings instance;

		/// <summary>
		/// Get each of the post-processing effects we're using. These aren't traditional Unity components, but they're just as easy to get to.
		/// </summary>
		void Start () 
		{
			depthOfField = GetComponent<PostProcessingBehaviour> ().profile.depthOfField;
			chromaticAberration = GetComponent<PostProcessingBehaviour> ().profile.chromaticAberration;
			motionBlur = GetComponent<PostProcessingBehaviour> ().profile.motionBlur;
			bloom = GetComponent<PostProcessingBehaviour> ().profile.bloom;

			applyCurrentSettings();

			instance = this;
		}

		/// <summary>
		/// Applies the currently configured camera settings.
		/// </summary>
		private void applyCurrentSettings ()
		{
			GameSettings.CameraSettings settings = Game.Instance.GameSettingsInstance.Camera;
			DepthOfFieldEnabled = settings.DepthOfFieldEnabled;
			ChromaticAberrationEnabled = settings.ChromaticAberrationEnabled;
			MotionBlurEnabled = settings.MotionBlurEnabled;
			BloomEnabled = settings.BloomEnabled;
		}

		/// <summary>
		/// Gets the instance that controls the post processing settings in the scene.
		/// </summary>
		public static PostProcessingSettings Instance
		{
			get 
			{
				return instance;
			}
		}

		/// <summary>
		/// Enables camera depth of field effect.
		/// </summary>
		public bool DepthOfFieldEnabled
		{
			get 
			{
				return depthOfField.enabled;
			}
			set 
			{
				depthOfField.enabled = value;
			}
		}

		/// <summary>
		/// Enables camera chromatic aberration effect.
		/// </summary>
		public bool ChromaticAberrationEnabled
		{
			get 
			{
				return chromaticAberration.enabled;
			}
			set
			{
				chromaticAberration.enabled = value;
			}
		}

		/// <summary>
		/// Enables camera motion blur effect.
		/// </summary>
		public bool MotionBlurEnabled
		{
			get
			{
				return motionBlur.enabled;
			}
			set
			{
				motionBlur.enabled = value;
			}
		}

		/// <summary>
		/// Enables camera bloom effect.
		/// </summary>
		public bool BloomEnabled
		{
			get
			{
				return bloom.enabled;
			}
			set
			{
				bloom.enabled = value;
			}
		}
	}
}

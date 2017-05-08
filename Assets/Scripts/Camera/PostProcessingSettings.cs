using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.PostProcessing{
	[ExecuteInEditMode]
	public class PostProcessingSettings : MonoBehaviour {

		public bool DepthOfField;
		public bool ChromaticAberration;
		public bool MotionBlur;

		/// Naming this effect "Bloom" breaks the actual effect in the post-processing profile, so it is named as such
		public bool BloomEffect;

		private DepthOfFieldModel depthOfField;
		private ChromaticAberrationModel chromaticAberration;
		private MotionBlurModel motionBlur;
		private BloomModel bloom;

		/// <summary>
		/// Get each of the post-processing effects we're using. These aren't traditional Unity components, but they're just as easy to get to.
		/// </summary>
		void Start () {
			depthOfField = GetComponent<PostProcessingBehaviour> ().profile.depthOfField;
			chromaticAberration = GetComponent<PostProcessingBehaviour> ().profile.chromaticAberration;
			motionBlur = GetComponent<PostProcessingBehaviour> ().profile.motionBlur;
			bloom = GetComponent<PostProcessingBehaviour> ().profile.bloom;
		}
			
		/// <summary>
		/// Have each individual setting check if it's on or not. Doing this in Update lets us change settings mid-game if we wanted to.
		/// </summary>
		void Update () {
			
			if (DepthOfField == true) {
				depthOfField.enabled = true;
			} else if (DepthOfField == false) {
				depthOfField.enabled = false;
			}

			if (ChromaticAberration == true) {
				chromaticAberration.enabled = true;
			} else if (ChromaticAberration == false) {
				chromaticAberration.enabled = false;
			}

			if (MotionBlur == true) {
				motionBlur.enabled = true;
			} else if (MotionBlur == false) {
				motionBlur.enabled = false;
			}

			if (BloomEffect == true) {
				bloom.enabled = true;
			} else if (BloomEffect == false) {
				bloom.enabled = false;
			}
		}

	}
}

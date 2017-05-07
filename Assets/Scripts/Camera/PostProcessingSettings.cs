using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.PostProcessing{
	[ExecuteInEditMode]
	public class PostProcessingSettings : MonoBehaviour {

		/// <summary>
		///  
		/// </summary>
		public bool DepthOfField;
		public bool ChromaticAberration;
		public bool MotionBlur;
		public bool BloomEffect;

		private DepthOfFieldModel depthOfField;
		private ChromaticAberrationModel chromaticAberration;
		private MotionBlurModel motionBlur;
		private BloomModel bloom;

		void Start () {
			depthOfField = GetComponent<PostProcessingBehaviour> ().profile.depthOfField;
			chromaticAberration = GetComponent<PostProcessingBehaviour> ().profile.chromaticAberration;
			motionBlur = GetComponent<PostProcessingBehaviour> ().profile.motionBlur;
			bloom = GetComponent<PostProcessingBehaviour> ().profile.bloom;
		}
			
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

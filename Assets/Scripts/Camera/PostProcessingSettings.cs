using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.PostProcessing{
	[ExecuteInEditMode]
	public class PostProcessingSettings : MonoBehaviour {

		public bool DepthOfField;
		public bool ChromaticAberration;
		public bool MotionBlur;

		private DepthOfFieldModel depthOfField;
		private ChromaticAberrationModel chromaticAberration;
		private MotionBlurModel motionBlur;

		// Use this for initialization
		void Start () {
			depthOfField = GetComponent<PostProcessingBehaviour> ().profile.depthOfField;
			chromaticAberration = GetComponent<PostProcessingBehaviour> ().profile.chromaticAberration;
			motionBlur = GetComponent<PostProcessingBehaviour> ().profile.motionBlur;
		}
		
		// Update is called once per frame
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
		}

	}
}

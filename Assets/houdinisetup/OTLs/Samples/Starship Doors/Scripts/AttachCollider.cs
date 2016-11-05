#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif // UNITY_EDITOR
public class AttachCollider : MonoBehaviour
{
#if UNITY_EDITOR
	public bool isTrigger = true;
	public Vector3 size = new Vector3( 1.0f, 1.0f, 1.0f );
	public Vector3 center = new Vector3( 0.0f, 0.0f, 0.0f );
	private Component[] rends;
	private GameObject myRoot;
	private GameObject doorControl;

	void Start()
	{
		// The myRoot assignment is pretty shaky. What I really want to do
		// is use a HAPI function to get the asset's root node, but I can't
		// find it at the moment.
		myRoot = gameObject.transform.parent.gameObject.transform.parent.gameObject;

		// Once we've got the root, we need to look back down and find the
		// door control object. Go find all the HAPI_GeoControl components
		// in the child objects...
		HoudiniGeoControl[] gcs = myRoot.GetComponentsInChildren< HoudiniGeoControl >();
		for ( int i = 0; i < gcs.Length; ++i )
		{
			// ...and if the geo name is 'door_anim,'  set the doorControl
			// GameObject to be the geo's parent. Confused yet? Me too.
			if ( gcs[ i ].prGeoName == "door_anim" )
				doorControl = gcs[ i ].gameObject.transform.parent.gameObject;
		}
	}

	void OnRenderObject()
	{
		// For some reason, I can't just add a BoxCollider component
		// directly via the UnityScriptAttach asset. It will add the
		// component, but it won't set the parameters. This is why I have
		// to do it from within the CS script here, using the values of the
		// public variables which were set by parsing the JSON string created
		// as a string attribute by UnityScriptAttach. Of course, this script
		// is useful for other things as well, such as trigger events.
		if ( !GetComponent< Collider >() )
		{
			BoxCollider bc = gameObject.AddComponent< BoxCollider >();
			bc.isTrigger = isTrigger;
			bc.size = size;
			bc.center = center;
		}

		// OK, now let's get rid of all the mesh filters and renderers in
		// all children of this object. because you don't want to render
		// the colliders.
		rends = gameObject.GetComponentsInChildren< MeshRenderer >();
		foreach ( MeshRenderer r in rends )
			r.enabled = false;
	}

	// This is where we send trigger messages to other script components. 
	// It makes more sense to do it this way than to use a ton of code in
	// the trigger events to animate the doors.  
	void OnTriggerEnter( Collider other )
	{
		// Typical Unity trick here: go fetch the animation script of the
		// doorControl object we set up earlier and execute a function.
		doorAnim comp = doorControl.GetComponent< doorAnim >();
		comp.Open();

		// Execute the open routine.
	}

	void OnTriggerExit( Collider other )
	{
		doorAnim comp = doorControl.GetComponent< doorAnim >();
		comp.Close();

		// Execute the open routine.
	}

#endif // UNITY_EDITOR
}

using System;
using UnityEngine;

// adds linerenderer when connected to object, don't delete the line renderer or you'll be SOL
[RequireComponent(typeof(LineRenderer))]
public class DebugDistricts : MonoBehaviour
{
	private LineRenderer lines;
	private Vector2[] verts;

	void Start()
	{
		lines = GetComponent<LineRenderer> ();
	}

	public Vector2[] Verts
	{ 
		get
		{ 
			return verts; 
		} 
		set 
		{
			verts = value;
			UpdateLines ();
		}
	}

	private void UpdateLines()
	{
		lines.SetVertexCount (verts.Length);
		for (int i = 0; i < verts.Length; ++i) 
		{
			Vector3 newVec = new Vector3 (verts [i].x, 0, verts [i].y); // put the y in the z because we want a flat plane
			lines.SetPosition (i, newVec);
		}
	}


}

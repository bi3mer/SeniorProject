using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform[] cameraPositions;
    public float smoothness;

    private int currentView;

	void Start () {
        currentView = 1;
	}
	
	void Update () {
        transform.position = Vector3.Lerp(transform.position, 
            cameraPositions[currentView].position, smoothness);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            cameraPositions[currentView].rotation, smoothness);
	}

    public Transform CurrentView
    {
        get { return cameraPositions[currentView]; }
    }

    /// <summary>
    /// Rotates the camera to the next right position.
    /// </summary>
    public void RotateRight()
    {
        ++currentView;
        currentView %= cameraPositions.Length;
    }

    /// <summary>
    /// Rotates the camera to the next left position.
    /// </summary>
    public void RotateLeft()
    {
        --currentView;
        if (currentView < 0)
        {
            currentView = cameraPositions.Length - 1;
        }
    }
}

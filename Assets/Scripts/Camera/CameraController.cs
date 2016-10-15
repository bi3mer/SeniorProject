using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public Transform[] CameraPositions;

    [SerializeField]
    private int startingView;
    [SerializeField]
    private float smoothness;

    private int currentView;

    /// <summary>
    /// Initialize camera based on starting view specified.
    /// </summary>
	void Start () {
        // in case a starting position is entered that is out of the range
        currentView = startingView % CameraPositions.Length;
	}
	
    /// <summary>
    /// Translates the camera smoothly betwteen camera positions.
    /// </summary>
	void Update () {
        transform.position = Vector3.Lerp(transform.position, 
            CameraPositions[currentView].position, smoothness);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            CameraPositions[currentView].rotation, smoothness);
	}

    /// <summary>
    /// Gets the transform of the camera's current target view.
    /// </summary>
    public Transform CurrentView
    {
        get { return CameraPositions[currentView]; }
    }

    /// <summary>
    /// Rotates the camera to the next right position.
    /// </summary>
    public void RotateRight()
    {
        ++currentView;
        currentView %= CameraPositions.Length;
    }

    /// <summary>
    /// Rotates the camera to the next left position.
    /// </summary>
    public void RotateLeft()
    {
        --currentView;
        if (currentView < 0)
        {
            currentView = CameraPositions.Length - 1;
        }
    }
}

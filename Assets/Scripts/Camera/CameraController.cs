using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public Transform[] CameraPositions;

    [Header("Rotation Control")]
    [SerializeField]
    private int startingView;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float cameraTransitionSpeed;
    [SerializeField]
    private Transform target;

    [Header("Zoom Controls")]
    [SerializeField]
    private float zoomingSpeed;
    [SerializeField]
    private float minZoomLevel;
    [SerializeField]
    private float maxZoomLevel;
    [SerializeField]
    private float initialZoomLevel;
    [SerializeField]
    private AnimationCurve zoomYPositionCurve;
    [SerializeField]
    private AnimationCurve zoomZPositionCurve;
    [SerializeField]
    private AnimationCurve zoomXRotationCurve;

    private int currentView;
    private Camera targetCamera;
    public bool EndingTriggered = false;

    /// <summary>
    /// Initialize camera based on starting view specified.
    /// </summary>
	void Start ()
    {
        // in case a starting position is entered that is out of the range
        currentView = startingView % CameraPositions.Length;
        // grab the camera
        targetCamera = GetComponent<Camera>();

        ZoomLevel = initialZoomLevel;
	}
	
    /// <summary>
    /// Translates the camera smoothly betwteen camera positions.
    /// </summary>
	void Update ()
    {
        float curveVal = scaleRangeForAnimationCurve(ZoomLevel, minZoomLevel, maxZoomLevel);
        Vector3 targetPosition = updateTargetPosition(curveVal);
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraTransitionSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(zoomXRotationCurve.Evaluate(curveVal),
            CameraPositions[currentView].eulerAngles.y, CameraPositions[currentView].eulerAngles.z), cameraTransitionSpeed);
	}

    /// <summary>
    /// Returns the target position for the camera to move to.
    /// </summary>
    private Vector3 updateTargetPosition(float curveVal)
    {
       return Vector3.LerpUnclamped(target.position + CameraPositions[currentView].localPosition + CameraPositions[currentView].transform.forward * zoomZPositionCurve.Evaluate(curveVal) +
            new Vector3(0f, zoomYPositionCurve.Evaluate(curveVal), 0f), target.position, ZoomLevel);
    }

    /// <summary>
    /// Gets the transform of the camera's current target view.
    /// </summary>
    public Transform CurrentView
    {
        get
        {
            if(!EndingTriggered)
            { 
               return CameraPositions[currentView];

            }
            else
            {
                return Camera.main.transform;
            }
        }
    }


    /// <summary>
    /// Gets the current level of zoom.
    /// </summary>
    public float ZoomLevel
    {
        get;
        private set;
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

    /// <summary>
    /// Zoom in the camera by the amount.
    /// </summary>
    /// <param name="amount">Amount to zoom.</param>
    public void Zoom (float amount)
    {
        ZoomLevel += amount * zoomingSpeed;

        if (ZoomLevel > maxZoomLevel)
        {
            ZoomLevel = maxZoomLevel;
        }
        else if (ZoomLevel < minZoomLevel)
        {
            ZoomLevel = minZoomLevel;
        }
    }

    /// <summary>
    /// Normalizes the zoom level to values between 0-1 which is used for Animation curves
    /// </summary>
    private float scaleRangeForAnimationCurve(float rawValue, float min, float max)
    {
        return (rawValue - min) / (max - min);
    }
}

using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierLine : MonoBehaviour
{
    [SerializeField]
    private Vector3 startPoint;
    [SerializeField]
    private Vector3 endPoint;
    [SerializeField]
    private Vector3 controlPoint;
    [SerializeField]
    private int nodes;

    /// <summary>
    /// Gets the line renderer component.
    /// </summary>
    public LineRenderer RendererComponent
    {
        get;
        private set;
    }
    
    /// <summary>
    /// Sets up line paramters and sets the positions of the initial line.
    /// </summary>
    void Start () 
    {
        if (nodes < 2)
        {
            Debug.LogError("BezierLine must have at least 2 nodes.");
        }
        RendererComponent = GetComponent<LineRenderer>();
        RendererComponent.SetVertexCount(nodes);
        RedrawLine();
    }

    /// <summary>
    /// Starting point of the bezier curve.
    /// </summary>
    public Vector3 StartPoint
    {
        get 
        { 
            return startPoint;
        }
        set
        {
            startPoint = value;
            RedrawLine();
        }
    }

    /// <summary>
    /// End point of the bezier curve.
    /// </summary>
    public Vector3 EndPoint
    {
        get
        {
            return endPoint;
        }
        set
        {
            endPoint = value;
            RedrawLine();
        }
    }

    /// <summary>
    /// Control point which the curve is interpolated towards.
    /// </summary>
    public Vector3 ControlPoint
    {
        get
        {
            return controlPoint;
        }
        set
        {
            controlPoint = value;
            RedrawLine();
        }
    }

    private void RedrawLine()
    {
        for (int i = 0; i < nodes; i++)
        {
            RendererComponent.SetPosition(i, CalculatePosition(i/(float)nodes));
        }
    }

    private Vector3 CalculatePosition(float t)
    {
        return ((1.0f - t) * ((1.0f - t) * StartPoint + t * ControlPoint)) 
            + (t * ((1.0f - t) * ControlPoint + t * EndPoint));
    }
}

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
        redrawLine();
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
            redrawLine();
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
            redrawLine();
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
            redrawLine();
        }
    }

    /// <summary>
    /// Updates each position in the lines
    /// </summary>
    private void redrawLine()
    {
        for (int i = 0; i < nodes; ++i)
        {
            RendererComponent.SetPosition(i, calculatePosition(i/(float)nodes));
        }
    }

    /// <summary>
    /// Calculate the position of the line at parametric position t.
    /// See https://en.wikipedia.org/wiki/B%C3%A9zier_curve
    /// </summary>
    /// <param name="t">Parametric position t along the line ranging from 0 to 1</param>
    /// <returns></returns>
    private Vector3 calculatePosition(float t)
    {
        return ((1.0f - t) * ((1.0f - t) * StartPoint + t * ControlPoint)) 
            + (t * ((1.0f - t) * ControlPoint + t * EndPoint));
    }
}

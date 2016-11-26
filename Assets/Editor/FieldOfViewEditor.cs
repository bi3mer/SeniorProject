using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class FieldOfViewEditor : Editor
{
    /// <summary>
    /// Draws how the field of view will look
    /// </summary>
    void OnSceneGUI()
    {
        PlayerController fov = (PlayerController)target;
        Handles.color = Color.white;

        // draw a circle by drawing the arc 360 degrees
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.ViewRadius);

        // define the angle of the lines that outline the field of view; since the top of a circle is 0 degrees getting 
        // the negative and positive halves of the viewAngle will get the outlining angles
        Vector3 viewAngleA = fov.DirFromAngle(-fov.ViewAngle / 2);
        Vector3 viewAngleB = fov.DirFromAngle(fov.ViewAngle / 2);

        //draw the lines using the outlining angles
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.ViewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.ViewRadius);
    }

}
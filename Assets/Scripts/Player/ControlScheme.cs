using UnityEngine;
using System.Collections;

[System.Serializable]
public class ControlScheme 
{
    public KeyCode Forward;
    public KeyCode Back;
    public KeyCode Right;
    public KeyCode Left;

    public KeyCode ForwardSecondary;
    public KeyCode BackSecondary;
    public KeyCode RightSecondary;
    public KeyCode LeftSecondary;

    public KeyCode Sprint;
    public KeyCode Jump;
    public KeyCode Action;
    public KeyCode UseTool;

    public KeyCode CameraRight;
    public KeyCode CameraLeft;
    public string CameraZoomAxis;

    public KeyCode Pause;
    public KeyCode Inventory;
    public KeyCode Radio;
	public KeyCode Crafting;
}
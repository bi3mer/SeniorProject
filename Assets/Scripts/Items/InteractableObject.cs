using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class InteractableObject : MonoBehaviour 
{
    [SerializeField]
    private string text;
    [SerializeField]
    private UnityEvent action;

    private TextMesh display;

    /// <summary>
    /// Initializes text and hides it.
    /// </summary>
    void Start()
    {
        SetUp();
    }

    /// <summary>
    /// Update the text to face the camera if shown.
    /// </summary>
    void Update()
    {
        // face text so it's readable
        if (Show)
        {
            Transform transform = display.gameObject.transform;
            transform.forward = Camera.main.transform.forward;
            transform.Rotate(30, 0, -45);
        }
    }

    /// <summary>
    /// Sets up the InteractableObject.
    /// </summary>
    public virtual void SetUp()
    {
    	if(display == null)
    	{
			display = GetComponentInChildren<TextMesh>();
		}

        Show = false;
    }

    /// <summary>
    /// Shows or hieds text describing the interactable object action to the player.
    /// </summary>
    public bool Show
    {
        get 
        { 
            return display.gameObject.activeInHierarchy;
        }
        set 
        {
            display.gameObject.SetActive(value);
            display.text = text;
        }
    }

    /// <summary>
    /// Text describing the interactive object's action.
    /// </summary>
    public string Text
    {
        get 
        { 
            return text;
        }
        set
        {
            text = value;
            if (Show)
            {
                display.text = text;
            }
        }
    }

    /// <summary>
    /// Have the player perform the object's action.
    /// </summary>
    public void PerformAction()
    {
    	if(action != null)
    	{
        	action.Invoke();
        }
    }

    /// <summary>
    /// Set the object's action which the player may activate.
    /// </summary>
    /// <param name="newAction">The action to be performed.</param>
    public void SetAction(UnityAction newAction)
    {
    	if(action == null)
    	{
    		action = new UnityEvent();
    	}

        action.RemoveAllListeners();
        action.AddListener(newAction);
    }
}
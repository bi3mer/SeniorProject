using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class Interactable : MonoBehaviour {

    public string text;
    public UnityEvent action;

    private TextMesh display;

    void Start()
    {
        display = GetComponentInChildren<TextMesh>();
        Show = false;
    }

    void Update()
    {
        // face text so it's readable
        Transform transform = display.gameObject.transform;
        transform.forward = Camera.main.transform.forward;
        transform.Rotate(30, 0, -45);
    }

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

    public void PerformAction()
    {
        action.Invoke();
    }

    public void SetAction(UnityAction newAction)
    {
        action.RemoveAllListeners();
        action.AddListener(newAction);
    }
}

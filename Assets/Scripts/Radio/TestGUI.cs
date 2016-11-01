using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour {
    public GameObject[] buttons;
    public Radio radio;

    void Awake()
    {
        buttons = GameObject.FindGameObjectsWithTag("RadioButtons");
        for (int i = 0; i < buttons.Length; ++i)
        {
            buttons[i].SetActive(false);
        }
    }

    void Start()
    {
        
    }


    public void ShowRadioButtons ()
    {
        for (int i = 0; i < buttons.Length; ++i)
        {
            if (buttons[i].activeSelf)
            {
                buttons[i].SetActive(false);
            }
            else if (!buttons[i].activeSelf)
            {
                buttons[i].SetActive(true);
            }
        }
    }

	public void Weather_Clicked()
    {
        radio.SetChannel(RadioChannel.Weather);
    }

    public void Music_Clicked()
    {
        radio.SetChannel(RadioChannel.Music);
    }
}

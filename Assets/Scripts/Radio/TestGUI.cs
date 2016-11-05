using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour {
    public GameObject[] buttons;
    public Radio radio;
    public AudioSource[] audios;
    public AudioSource soundA;
    public AudioSource soundB;
    public AudioClip newClip;


    void Awake()
    {
        buttons = GameObject.FindGameObjectsWithTag("RadioButtons");
        for (int i = 0; i < buttons.Length; ++i)
        {
            buttons[i].SetActive(false);
        }
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

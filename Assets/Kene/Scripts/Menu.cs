using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuMusicToPlay;

    private void Start()
    {
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        AudioManager.instance.PlayMusic(GetComponent<AudioSource>(), menuMusicToPlay);
    }

    public void OpenURL(string person)
    {
        switch (person)
        {
            case "odis":
                Application.OpenURL("https://odispixel.carrd.co");
                break;
            case "ash":
                Application.OpenURL("https://rizosha.itch.io");
                break;
            case "kene":
                Application.OpenURL("https://kene991.itch.io/");
                break;

        }
    }
}

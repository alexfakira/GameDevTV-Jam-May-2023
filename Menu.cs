using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject main_menu;
    public GameObject options_menu;

    public Slider music_slider;
    public Slider sfx_slider;

    private void Awake(){
        Screen.SetResolution(1280, 720, true);

        options_menu.SetActive(false);

        music_slider.value = AudioManager.instance.music_volume;
        sfx_slider.value = AudioManager.instance.sfx_volume;
    }

    public void PlayGame(){
        SceneManager.LoadScene("Level_1");
    }

    public void OptionsMenu(){
        main_menu.SetActive(false);
        options_menu.SetActive(true);
    }

    public void QuitGame(){
        #if UNITY_STANDALONE
        Application.Quit();
        #endif
 
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OptionsMenuBack(){
        options_menu.SetActive(false);
        main_menu.SetActive(true);
    }

    public void SetSFXVolume(){
        AudioManager.instance.SFXVolume(sfx_slider.value);
    }

    public void SetMusicVolume(){
        AudioManager.instance.MusicVolume(music_slider.value);
    }

}
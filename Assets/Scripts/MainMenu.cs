using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool FadeOutFinished = false;

    // Start is called before the first frame update
    public void PlayGame()
    {
        StartCoroutine(FadeOut(GameObject.Find("MainMenu_theme").GetComponent<AudioSource>(), 3));
    }

    public void Update(){
        if(FadeOutFinished){
            SceneManager.LoadScene(1);
            FadeOutFinished = false;
        }
    }

    public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop ();
        audioSource.volume = startVolume;
        FadeOutFinished = true;
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();   
    }
}

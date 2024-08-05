using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    VideoPlayer videoPlayer;
    [SerializeField] GameObject Panel;
    Audio audioManager;


    private void Start()
    {
     
      audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
       
    }


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
       audioManager.PlaySFX(audioManager.ButtonClick);
        
    }

    public void Setting()
    {
        Panel.SetActive(true);
        audioManager.PlaySFX(audioManager.IconClick);


    }

    public void BackToMenu()
    {
        Panel.SetActive(false);
        audioManager.PlaySFX(audioManager.ButtonClick);


    }

    public void PauseGame()
    {
        Panel.SetActive(true);
        audioManager.PlaySFX(audioManager.IconClick);
        Time.timeScale = 0f;
       
    }

    public void ResumeGame()
    {
        Panel.SetActive(false);
        audioManager.PlaySFX(audioManager.ButtonClick);
        Time.timeScale = 1f; 
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        audioManager.PlaySFX(audioManager.ButtonClick);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        audioManager.PlaySFX(audioManager.ButtonClick);
        Time.timeScale = 1f;
    }
}

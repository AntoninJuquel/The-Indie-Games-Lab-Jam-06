using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    public static ButtonManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Track func
    public void StartTrack()
    {
        if ((GameManager.Instance.GetTrackStatus() == GameManager.TrackStatus.building || GameManager.Instance.GetTrackStatus() == GameManager.TrackStatus.paused) && GameManager.Instance.GetTrackStatus() != GameManager.TrackStatus.ended)
        {
            if (PathManager.Instance.StartPath())
                GameManager.Instance.StartTrack();
            else
            {
                Debug.Log("Build a path first // Todo display on screen");
            }
        }
    }
    public void PauseTrack()
    {
        if (GameManager.Instance.GetTrackStatus() == GameManager.TrackStatus.started)
        {
            PathManager.Instance.PausePath();
            GameManager.Instance.PauseTrack();
        }
    }
    public void ResetTrack()
    {
        BuildManager.Instance.ResetNodes();
        PathManager.Instance.ResetPath();
        UIManager.Instance.DisplayInstruction(0);
        GameManager.Instance.ResetTrack();
    }
    // Game func
    public void ResumeGame()
    {
        GameManager.Instance.ToggleGameStatus();
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void RealyQuitTheGame()
    {
        Application.Quit();
    }

    // Settings func
    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetVolume(float percent)
    {
        mixer.SetFloat("Volume", Mathf.Log10(Mathf.Clamp(percent / 100, 0.001f, 1f)) * 20);
    }
    public void NextLevel()
    {
        if (GameManager.Instance.LevelValidated())
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            Debug.Log("You didnt deliver all item // TODO display on screen");
    }
    public IEnumerator LoadLevel(int levelNumber)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelNumber);
    }
    public void ButtonConfirmation()
    {
        AudioManager.Instance.Play("confirmation_" + Random.Range(0, 2));
    }
    public void ButtonClose()
    {
        AudioManager.Instance.Play("close_" + Random.Range(0, 5));
    }
    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
}

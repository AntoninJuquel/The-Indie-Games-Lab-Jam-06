using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CameraController viewCamera;
    [SerializeField] ParticleSystem fireworks;
    [Header("Status")]
    [SerializeField] int levelToUnlock;
    [SerializeField] TrackStatus trackStatus;
    [SerializeField] GameStatus gameStatus;



    [SerializeField] int coins;
    [Header("Objectives")]
    [SerializeField] int fruitsToDeliver;
    [SerializeField] int humansToDeliver;
    [Header("Progress")]
    [SerializeField] int fruitsDelivered;
    [SerializeField] int humansDelivered;
    [Header("Timers")]
    [SerializeField] float totalTime;
    [SerializeField] float reflexionTime;
    [SerializeField] float travelTime;

    
    public static GameManager Instance;
    public enum TrackStatus { building, started, paused, ended }
    public enum GameStatus { playing, paused }
    bool paused;
    private void Awake()
    {
        Instance = this;

        Invoke("ResetTrack", .5f);
    }
    private void Start()
    {
        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (trackStatus != TrackStatus.ended)
        {
            totalTime += Time.deltaTime;
        }
        if (trackStatus == TrackStatus.started)
        {
            travelTime += Time.deltaTime;
        }
        if (trackStatus == TrackStatus.ended)
        {
            reflexionTime = totalTime - travelTime;
        }
        UIManager.Instance.SetTimerText(reflexionTime, travelTime, totalTime); ;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGameStatus();
        }
    }

    public void StartTrack()
    {
        trackStatus = TrackStatus.started;

    }
    public void PauseTrack()
    {
        trackStatus = TrackStatus.paused;
    }

    public void ResetTrack()
    {
        trackStatus = TrackStatus.building;
        fruitsDelivered = 0;
        humansDelivered = 0;
        travelTime = 0;
        UIManager.Instance.SetTimerText(totalTime, travelTime, reflexionTime);
        viewCamera.GoToPlayground();
        fireworks.Stop();
    }
    public void EndTrack()
    {
        trackStatus = TrackStatus.ended;
        viewCamera.GoToEndScreen();
        if (LevelValidated())
        {
            fireworks.Play();
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
        }
        return;
    }
    public float GetTimer()
    {
        return travelTime;
    }
    public void ToggleGameStatus()
    {
        gameStatus = (gameStatus == GameStatus.playing ? GameStatus.paused : GameStatus.playing);
        if (gameStatus == GameStatus.playing)
        {
            Time.timeScale = 1;
            if (trackStatus == TrackStatus.ended)
                viewCamera.GoToEndScreen();
            else
                viewCamera.GoToPlayground();
        }
        else
        {
            Time.timeScale = 0;
            viewCamera.GoToPauseMenu();
        }
    }
    public GameStatus GetGameStatus()
    {
        return gameStatus;
    }
    public TrackStatus GetTrackStatus()
    {
        return trackStatus;
    }
    public void AddFruitDelivery()
    {
        fruitsDelivered++;
    }
    public void AddHumanDelivered()
    {
        humansDelivered++;
    }
    public bool LevelValidated()
    {
        bool validation = (humansDelivered == humansToDeliver && fruitsDelivered == fruitsToDeliver);
        return validation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    int currentIndex = 0;
    [SerializeField] TextMeshProUGUI[] instructionTexts;
    [Header("Speed")]
    [SerializeField] TextMeshProUGUI speedText;
    [Header("Timers")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI timerMsText;
    [Header("Rails")]
    [SerializeField] TextMeshProUGUI railsCounter;
    [Header("Sound")]
    [SerializeField] TextMeshProUGUI soundPercentText;
    [Header("Time")]
    [SerializeField] TextMeshProUGUI reflexionTimeText;
    [SerializeField] TextMeshProUGUI travelTimeText;
    [SerializeField] TextMeshProUGUI totalTimeText;
 
   
    [Header("Stats")]
    [SerializeField] TextMeshProUGUI railsUsedText;
    [SerializeField] TextMeshProUGUI railsRemainingText;
    
    

    bool isFading;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
        SetTimerText(0,0,0);
        SetSpeedText(50);
    }

    public void SetSpeedText(float percent)
    {
        float speed = percent * 3f;
        speedText.text = speed.ToString("f1") + "km/h";
    }
    float[] FloatToTime(float time)
    {
        float minutes = (int)(time / 60f);
        float seconds = (int)(time % 60f);
        float ms = (int)((time - seconds) * 100f);

        float[] myArray = new float[3] { minutes, seconds, ms };

        return myArray;
    }
    public void SetTimerText(float reflexionTime,float travelTime,float totalTime)
    {
        /*float minutes = (int)(time / 60f);
        float seconds = (int)(time % 60f);
        float ms = (int)((time - seconds) * 100f);*/

        timerText.text = FloatToTime(travelTime)[0].ToString("00") + ":" + FloatToTime(travelTime)[1].ToString("00");
        timerMsText.text = FloatToTime(travelTime)[2].ToString("00");
        travelTimeText.text = "Travel Time : " + FloatToTime(travelTime)[0].ToString("00") + ":" + FloatToTime(travelTime)[1].ToString("00") + ":" + FloatToTime(travelTime)[2].ToString("0");

        reflexionTimeText.text = "Reflexion Time : " + FloatToTime(reflexionTime)[0].ToString("00") + ":" + FloatToTime(reflexionTime)[1].ToString("00") + ":" + FloatToTime(reflexionTime)[2].ToString("0");
        totalTimeText.text = "Total Time : " + FloatToTime(totalTime)[0].ToString("00") + ":" + FloatToTime(totalTime)[1].ToString("00") + ":" + FloatToTime(totalTime)[2].ToString("0");
    }

    public void DisplayInstruction(int index)
    {
        if (index < instructionTexts.Length)
        {
            StartCoroutine(Fade(instructionTexts[currentIndex],1,0));
            currentIndex = index;
            StartCoroutine(Fade(instructionTexts[currentIndex], 0, 1));
        }
    }
    IEnumerator Fade(TextMeshProUGUI text,float start, float end, float lerpTime = 0.5f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            Color c = text.color;
            c.a = currentValue;
            text.color = c;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
    public void SetRailsCounter(int _number, int railsUsed)
    {
        
        railsCounter.text = "Rails remaining " + _number;
        railsRemainingText.text = "Rails remaining : " + _number;
        railsUsedText.text = "Rails used : " + railsUsed;
    }
    public void SetSoundPercentText(float percent)
    {
        soundPercentText.text = percent.ToString("00.0") + "%";
    }
}

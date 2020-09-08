using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playPosition;
    [SerializeField] Transform pausePosition;
    [SerializeField] Transform settingsPosititon;
    [SerializeField] Transform endScreenPosition;
    [SerializeField] float speed;

    Transform start, end;

    [SerializeField]
    [Range(0f, 1f)]
    float lerpPct = 0f;
    // Update is called once per frame
    void Update()
    {
        if (start && end)
        {
            lerpPct = Mathf.Clamp(lerpPct + (Time.unscaledDeltaTime * speed), 0, 1);
            transform.position = Vector3.Lerp(start.position, end.position, lerpPct);
            transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, lerpPct);
        }

    }
    public void GoToPlayground()
    {
        lerpPct = 0;
        start = transform;
        end = playPosition;
    }
    public void GoToPauseMenu()
    {
        lerpPct = 0;
        start = transform;
        end = pausePosition;
    }
    public void GoToSettingsMenu()
    {
        lerpPct = 0;
        start = transform;
        end = settingsPosititon;
    }
    public void GoToEndScreen()
    {
        lerpPct = 0;
        start = transform;
        end = endScreenPosition;
    }
}

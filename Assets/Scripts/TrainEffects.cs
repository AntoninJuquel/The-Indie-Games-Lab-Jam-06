using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainEffects : MonoBehaviour
{
    [SerializeField] SlidingNode slider;
    [Header("Speed")]
    [SerializeField] float fasterSpeed;
    [SerializeField] float slowerSpeed;
    [SerializeField] float timeEffect;
    public enum Effect
    {
        basic,
        fast,
        slow
    }
    public Effect effect;
    Train train;
    private void Awake()
    {
        train = GetComponent<Train>();
    }
    public IEnumerator Faster()
    {
        float previousSpeed = train.GetSpeed();
        train.SetSpeed(fasterSpeed);
        slider.ToggleLock();
        effect = Effect.fast;

        yield return new WaitForSeconds(timeEffect);

        effect = Effect.basic;
        train.SetSpeed(previousSpeed);
        slider.ToggleLock();

    }
    public IEnumerator Slower()
    {
        float previousSpeed = train.GetSpeed();
        train.SetSpeed(slowerSpeed);
        slider.ToggleLock();
        effect = Effect.slow;

        yield return new WaitForSeconds(timeEffect);

        effect = Effect.basic;
        train.SetSpeed(previousSpeed);
        slider.ToggleLock();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float pitchBufferMultiplier;
    private float playerMinPitch;
    private float playerMaxPitch;

    [SerializeField] float speed;

    private enum FlightSpeedMethod
    {
        Constant,
        Accelerating
    }
    [SerializeField] FlightSpeedMethod flightSpeedMethod;

    [SerializeField] internal float minY;
    [SerializeField] internal float maxY;
    internal float heightT;

    private enum HeightInterpolationMethod
    {
        Linear,
        Exponential
    }
    [SerializeField] HeightInterpolationMethod heightInterpolationMethod;

    private enum ZeroHzFlight
    {
        MaintainHeight
    }
    [SerializeField] ZeroHzFlight zeroHzFlight;

    [SerializeField] PitchTracker pitchTracker;

    void Start()
    {
        playerMinPitch = ExponentialInterpolation(pitchTracker.pitchRangeMin, pitchTracker.pitchRangeMax, pitchBufferMultiplier / 2);
        playerMaxPitch = ExponentialInterpolation(pitchTracker.pitchRangeMin, pitchTracker.pitchRangeMax, 1 - pitchBufferMultiplier / 2);
    }

    void LateUpdate()
    {
        if (pitchTracker.pitchValue == 0)
        {
            return;
        }
        float t;
        if (heightInterpolationMethod == HeightInterpolationMethod.Linear)
        {
            t = Mathf.InverseLerp(playerMinPitch, playerMaxPitch, pitchTracker.pitchValue);
        }
        else
        {
            t = 0.5f;  // Implement later
        }
        if (flightSpeedMethod == FlightSpeedMethod.Constant)
        {
            heightT = Mathf.Lerp(minY, maxY, t);
            if (heightT < transform.position.y)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
            }
            else if (heightT > transform.position.y)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
            }
        }
    }

    public float ExponentialInterpolation(float a, float b, float t)
    {
        return a * Mathf.Pow((b / a), t);
    }
}

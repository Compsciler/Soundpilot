using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float pitchBufferMultiplier;
    private float playerMinPitch;
    private float playerMaxPitch;

    [SerializeField] float horizontalStartSpeed;
    [SerializeField] float horizontalSpeedIncrementPerSecond;
    private float horizontalSpeed;
    [SerializeField] float verticalSpeed;

    private enum VerticalSpeedMethod
    {
        Constant,
        Accelerating
    }
    [SerializeField] VerticalSpeedMethod verticalSpeedMethod;

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

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        playerMinPitch = ExponentialInterpolation(pitchTracker.pitchRangeMin, pitchTracker.pitchRangeMax, pitchBufferMultiplier / 2);
        playerMaxPitch = ExponentialInterpolation(pitchTracker.pitchRangeMin, pitchTracker.pitchRangeMax, 1 - pitchBufferMultiplier / 2);

        horizontalSpeed = horizontalStartSpeed;
    }

    void LateUpdate()
    {
        transform.Translate(horizontalSpeed * Time.deltaTime, 0, 0);
        mainCamera.transform.Translate(horizontalSpeed * Time.deltaTime, 0, 0);
        MoveVertically();

        horizontalSpeed += horizontalSpeedIncrementPerSecond * Time.deltaTime;
    }

    public void MoveVertically()
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
        if (verticalSpeedMethod == VerticalSpeedMethod.Constant)
        {
            heightT = Mathf.Lerp(minY, maxY, t);
            if (heightT < transform.position.y)
            {
                transform.Translate(0, -verticalSpeed * Time.deltaTime, 0);
            }
            else if (heightT > transform.position.y)
            {
                transform.Translate(0, verticalSpeed * Time.deltaTime, 0);
            }
        }
    }

    public float ExponentialInterpolation(float a, float b, float t)
    {
        return a * Mathf.Pow((b / a), t);
    }
}

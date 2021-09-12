using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using TMPro;

// Source: https://forum.unity.com/threads/detecting-musical-notes-from-vocal-input.316698/
[RequireComponent(typeof(AudioSource))]
class Peak
{
    public float amplitude;
    public int index;

    public Peak()
    {
        amplitude = 0f;
        index = -1;
    }

    public Peak(float _frequency, int _index)
    {
        amplitude = _frequency;
        index = _index;
    }
}

class AmpComparer : IComparer<Peak>
{
    public int Compare(Peak a, Peak b)
    {
        return 0 - a.amplitude.CompareTo(b.amplitude);
    }
}

class IndexComparer : IComparer<Peak>
{
    public int Compare(Peak a, Peak b)
    {
        return a.index.CompareTo(b.index);
    }
}

public class PitchTracker : MonoBehaviour
{

    public float rmsValue;
    public float dbValue;
    public float pitchValue;

    public int qSamples = 1024;
    public int binSize = 1024; // you can change this up, I originally used 8192 for better resolution, but I stuck with 1024 because it was slow-performing on the phone
    public float refValue = 0.1f;
    public float threshold = 0.01f;


    private List<Peak> peaks = new List<Peak>();
    float[] samples;
    float[] spectrum;
    int samplerate;

    public TMP_Text display; // drag a Text object here to display values  //| Changed from Text to TMP_Text
    public bool mute = true;
    public AudioMixer masterMixer; // drag an Audio Mixer here in the inspector

    [SerializeField] int minPeaksForPitch = 5;  //|
    [SerializeField] bool isUsingMinPitch = false;

    [SerializeField] internal float pitchRangeMin;
    [SerializeField] internal float pitchRangeMax;

    void Start()
    {
        samples = new float[qSamples];
        spectrum = new float[binSize];
        samplerate = AudioSettings.outputSampleRate;

        // starts the Microphone and attaches it to the AudioSource
        GetComponent<AudioSource>().clip = Microphone.Start(null, true, 10, samplerate);
        GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
        while (!(Microphone.GetPosition(null) > 0)) { } // Wait until the recording has started
        GetComponent<AudioSource>().Play();

        // Mutes the mixer. You have to expose the Volume element of your mixer for this to work. I named mine "masterVolume".
        //| masterMixer.SetFloat("masterVolume", -80f);
    }

    void Update()
    {
        AnalyzeSound();
        if (display != null)
        {
            display.text = "RMS: " + rmsValue.ToString("F2") +
                " (" + dbValue.ToString("F1") + " dB)\n" +
                "Pitch: " + pitchValue.ToString("F0") + " Hz";
        }
    }

    void AnalyzeSound()
    {
        float[] samples = new float[qSamples];
        GetComponent<AudioSource>().GetOutputData(samples, 0); // fill array with samples
        int i = 0;
        float sum = 0f;
        for (i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
        dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min

        // get sound spectrum
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0f;
        for (i = 0; i < binSize; i++)
        { // find max
            if (spectrum[i] > maxV && spectrum[i] > threshold)
            {
                peaks.Add(new Peak(spectrum[i], i));
                if (peaks.Count > minPeaksForPitch)
                { // get the 5 peaks in the sample with the highest amplitudes
                    peaks.Sort(new AmpComparer()); // sort peak amplitudes from highest to lowest
                    string s = "";
                    for (int j = 0; j < peaks.Count; j++)
                    {
                        s += GetPitch(peaks[j]) + " ";
                    }
                    // Debug.Log(s);  //|
                    // Debug.Log(GetPitch(peaks[0]) + " " + GetPitch(peaks[1]) + " " + GetPitch(peaks[2]) + " " + GetPitch(peaks[3]) + " " + GetPitch(peaks[4]));  //|
                    // Debug.Log(peaks[0].amplitude + " " + peaks[1].amplitude + " " + peaks[2].amplitude + " " + peaks[3].amplitude + " " + peaks[4].amplitude);  //|
                    // Debug.Log(peaks[0].index + " " + peaks[1].index + " " + peaks[2].index + " " + peaks[3].index + " " + peaks[4].index);  //|
                    //peaks.Remove (peaks [5]); // remove peak with the lowest amplitude
                }
            }
        }
        if (peaks.Count > 0)
        {
            if (isUsingMinPitch)
            {
                pitchValue = GetMinPitchWithinRange();
                // Debug.Log(pitchValue);
            }
            else
            {
                pitchValue = GetPitch(peaks[0]); // convert index to frequency
            }
        }
        else
        {
            pitchValue = 0;
        }
        peaks.Clear();
    }

    private float GetPitch(Peak peak)  //| Moved from AnalyzeSound()
    {
        float freqN = 0f;
        if (peaks.Count > 0)
        {
            float maxV = peak.amplitude;
            int maxN = peak.index;
            freqN = maxN; // pass the index to a float variable
            if (maxN > 0 && maxN < binSize - 1)
            { // interpolate index using neighbours
                var dL = spectrum[maxN - 1] / spectrum[maxN];
                var dR = spectrum[maxN + 1] / spectrum[maxN];
                freqN += 0.5f * (dR * dR - dL * dL);
            }
        }
        return freqN * (samplerate / 2f) / binSize;
    }

    private float GetMinPitchWithinRange()  //| Should be fundamental frequency
    {
        float minPitch = float.MaxValue;
        foreach (Peak peak in peaks)
        {
            float pitch = GetPitch(peak);
            if (pitch < minPitch && (pitch >= pitchRangeMin && pitch <= pitchRangeMax))
            {
                minPitch = pitch;
            }
        }
        if (minPitch == float.MaxValue)
        {
            return 0;
        }
        return minPitch;
    }
}
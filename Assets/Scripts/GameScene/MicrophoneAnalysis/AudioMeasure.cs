using TMPro;
using UnityEngine;

// Source: https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html?childToView=158800#answer-158800
public class AudioMeasure : MonoBehaviour
{
    public float RmsValue;
    public float DbValue;
    public float PitchValue;

    private const int QSamples = 1024;
    private const float RefValue = 0.1f;
    private const float Threshold = 0.02f;

    float[] _samples;
    private float[] _spectrum;
    private float _fSample;

    [SerializeField] TMP_Text pitchText;

    void Start()
    {
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;
    }

    void Update()
    {
        AnalyzeSound();
    }

    void AnalyzeSound()
    {
        GetComponent<AudioSource>().GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }
        RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
        if (DbValue < -160) DbValue = -160; // clamp it to -160dB min
                                            // get sound spectrum
        GetComponent<AudioSource>().GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < QSamples; i++)
        { // find max 
            if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
                continue;

            maxV = _spectrum[i];
            maxN = i; // maxN is the index of max
            // Debug.Log(i + " " + GetPitch(maxN));  //|
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < QSamples - 1)
        { // interpolate index using neighbours
            var dL = _spectrum[maxN - 1] / _spectrum[maxN];
            var dR = _spectrum[maxN + 1] / _spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
            // Debug.Log((maxN - 1) * (_fSample / 2) / QSamples + " " + maxN * (_fSample / 2) / QSamples + " " + (maxN + 1) * (_fSample / 2) / QSamples);  //|
        }
        PitchValue = freqN * (_fSample / 2) / QSamples; // convert index to frequency

        pitchText.text = $"Pitch: {Mathf.Round(PitchValue)} Hz";
        if (PitchValue != 0)  //|
        {
            // Debug.Log("freqN: " + freqN);
            // Debug.Log(Mathf.Round(PitchValue));
            // Debug.Log("FF: " + GetComponent<MicrophoneInput>().GetFundamentalFrequency());
        }
    }
}
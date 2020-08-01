using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;
    public static float[] _samples = new float[512]; //all 512 raw samples
    public static float[] freqBand = new float[8];  //512 samples compressed into 8 raw samples 
    public static float[] bandBuffer = new float[8]; //8 raw samples with buffering.
    float[] bufferChange = new float[8]; //buffer decrease valuse for exponential downwards only

    [Tooltip("Replaces the audio clip on the attached audio source with mic input")]
    public bool useMic = false;
    public int micIndex = 0;
    [Tooltip("uses the static audio Listener class instead of the audio source")]
    public bool GlobalListen = false;
   
    public enum BufferType { ExponetialDownwards, goToFrequencyBand }
    public BufferType _bufferType; //enum to choose buffer type.
    [Range(1, 30)]
    public float BufferDownSpeedDivider = 8;//controlls speed that the buffer goes down at
    [Range(1, 30)]
    public float BufferUpSpeedDivider = 1;//controlls speed that buffer goes up at.

    public float[] freqBandHighest = new float[8]; //highest recorded amplitude in audio samples
    public static float[] audioBandRange = new float[8]; //creates a value from 0 to 1 to represent amplitute levels.
    public static float[] audioBandRangeBuffer = new float[8];

    public static float AmplitudeAvg, AmplitudeAvgBuffer;
    float AmplitudeHighest;

    public float Intensity;
    bool BufferingMusicAvgAmp = true;
    // Use this for initialization
    void Start()
    {
        Array.Clear(audioBandRange, 0, audioBandRange.Length);
        _audioSource = gameObject.GetComponent<AudioSource>();
        SetupMic(micIndex);
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBandRange();
        StartCoroutine(MusicAvgAmpBuffer());
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBandRange();
        GetAvgAmplitude();
    }

    private string micName;
    void SetupMic(int deviceIndex)
    {
        if (useMic)
        {
            foreach (string device in Microphone.devices)
            {
                Debug.Log(device);
            }
            if (Microphone.devices.Length > 0)
            {
                if (deviceIndex < 0 || deviceIndex > Microphone.devices.Length)
                {
                    Debug.LogWarning("invalidMic");
                    return;
                }
                micName = Microphone.devices[deviceIndex].ToString();                
                _audioSource.clip = Microphone.Start(micName, true, 10, AudioSettings.outputSampleRate);
                while (Microphone.GetPosition(micName) < 20) { }
                _audioSource.PlayDelayed(0.002f);
            }
            else
            {
                Debug.LogWarning("no microphone detected");
            }
        }
    }

    void GetAvgAmplitude()
    {
        float currentAmpAvg = 0;
        float CurrentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            currentAmpAvg += audioBandRange[i];
            CurrentAmplitudeBuffer += audioBandRangeBuffer[i];
        }
        if (currentAmpAvg > AmplitudeHighest)
        {
            AmplitudeHighest = currentAmpAvg;
        }
        if (float.IsNaN(currentAmpAvg) || float.IsNaN(AmplitudeHighest))
        {
            AmplitudeAvg = 0;
            AmplitudeAvgBuffer = 0;
        }
        else
        {
            AmplitudeAvg = currentAmpAvg / AmplitudeHighest;
            AmplitudeAvgBuffer = CurrentAmplitudeBuffer / AmplitudeHighest;
        }
    }

    void CreateAudioBandRange()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];
            }

            audioBandRange[i] = (freqBand[i] / freqBandHighest[i]);
            audioBandRangeBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);

        }
    }

    void GetSpectrumAudioSource()
    {
        float[] tempSpamples1 = new float[512];
        float[] tempSpamples2 = new float[512];
        if (!GlobalListen)
        {
            AudioListener.GetSpectrumData(tempSpamples1, 0, FFTWindow.Blackman);
            AudioListener.GetSpectrumData(tempSpamples2, 1, FFTWindow.Blackman);
        }
        else
        {
            _audioSource.GetSpectrumData(tempSpamples1, 0, FFTWindow.Blackman);
            _audioSource.GetSpectrumData(tempSpamples2, 1, FFTWindow.Blackman);
        }

        for (int i = 0; i < _samples.Length; i++)
        {
            _samples[i] = (tempSpamples1[i] + tempSpamples2[i]) / 2;
        }
    }

    void BandBuffer()
    {

        for (int g = 0; g < 8; g++)
        {
            switch (_bufferType)
            {
                case BufferType.ExponetialDownwards:
                    if (freqBand[g] > bandBuffer[g])
                    {
                        bandBuffer[g] = freqBand[g];
                        bufferChange[g] = 0.08f;
                    }

                    if (freqBand[g] < bandBuffer[g])
                    {
                        bandBuffer[g] -= bufferChange[g];
                        bufferChange[g] *= 1.15f;
                    }
                    break;

                case BufferType.goToFrequencyBand:
                    if (freqBand[g] > bandBuffer[g])
                    {
                        //bandBuffer[g] = freqBand[g];
                        bufferChange[g] = (freqBand[g] - bandBuffer[g]) / BufferUpSpeedDivider;
                        bandBuffer[g] += bufferChange[g];
                    }

                    if (freqBand[g] < bandBuffer[g])
                    {
                        bufferChange[g] = (bandBuffer[g] - freqBand[g]) / BufferDownSpeedDivider;
                        bandBuffer[g] -= bufferChange[g];
                    }
                    break;
            }

        }
    }

    private void MakeFrequencyBands()
    {
        //creates 8 frequency bands that average out the 512 bands.
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)//adds 2 extra sample channels to cover entire spectrum
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
                //for each channel the amount of samples taken gets higher and count keeps rising
            }

            average /= count;
            freqBand[i] = average * 10f;
        }
    }
    public void AnyalizeSong()
    {
        for (int i = 0; i < freqBandHighest.Length; i++)
        {
            freqBandHighest[i] = 0;
        }
        _audioSource.Pause();
        int clipLength = (int)_audioSource.clip.length;
        StartCoroutine(AnyalizeSongCo(clipLength));
    }

    IEnumerator AnyalizeSongCo(int _clipLength)
    {
        for (float i = 0; i < _clipLength; i += 0.5f)
        {
            _audioSource.Pause();
            _audioSource.time = i;
            _audioSource.Play();
            yield return new WaitForSeconds(0.001f);
        }
    }

    //UI Functions
    public void EditBufferUp(float _BufferUp)
    {
        BufferUpSpeedDivider = _BufferUp;
    }

    public void EditBufferDown(float _BufferDown)
    {
        BufferDownSpeedDivider = _BufferDown;
    }

    static float AverageBufferAmp;
    List<float> AvgAmpBuffer = new List<float>();
    IEnumerator MusicAvgAmpBuffer()
    {
        for (int i = 0; i < 100; i++)
        {
            AvgAmpBuffer.Add(0);
        }
        while (BufferingMusicAvgAmp)
        {
            //MusicAVGBuffer.Insert(0, AudioPeer.audioBandRangeBuffer[0]*2 + 0.5f);
            AvgAmpBuffer.Insert(0, AmplitudeAvg);
            try
            {
                AvgAmpBuffer.RemoveAt(101);
            }
            catch (Exception)
            {
                Debug.Log("ListNotFull");
            }
            float tempAvg = 0;
            foreach (float buffer in AvgAmpBuffer) // calculates the average amplitude in a separate variable to avoid calling while calculations are being made.
            {
                tempAvg = +buffer;
            }
            AverageBufferAmp = tempAvg / AvgAmpBuffer.Count;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public static float GetAmpBufferAvg()
    {
        return AverageBufferAmp;
    }

}

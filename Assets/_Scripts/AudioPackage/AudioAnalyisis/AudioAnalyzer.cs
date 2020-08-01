using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyzer : MonoBehaviour
{
    public string audioTag; //used to identify in array of AudioAnalyizers
    public AudioSource _audioSource;
    private float[] _samples = new float[512]; //all 512 raw samples
    private float[] freqBand = new float[8];  //512 samples compressed into 8 raw samples 
    private float[] bandBuffer = new float[8]; //8 raw samples with buffering.
    private float[] bufferChange = new float[8]; //buffer decrease valuse for exponential downwards only

    public enum BufferType { ExponetialDownwards, goToFrequencyBand }
    public BufferType _bufferType; //enum to choose buffer type.
    [Range(0, 1)]
    public float BufferDownMultiplier = 1;//controlls speed that the buffer goes down at
    [Range(0, 1)]
    public float BufferUpMultipler = 1;//controlls speed that buffer goes up at.

    public float NormValDecreaseSpeed = 0; //controlls how quickly the highest recorded values used for normalization decrease

    public float[] freqBandHighest = new float[8]; //highest recorded amplitude in audio samples
    public float[] audioBandRange = new float[8]; //creates a value from 0 to 1 to represent amplitute levels.
    public float[] audioBandRangeBuffer = new float[8];

    public float AmplitudeAvg, AmplitudeAvgBuffer;
    float AmplitudeHighest;



    bool BufferingMusicAvgAmp = true;
    // Use this for initialization
    void Start()
    {
        Array.Clear(audioBandRange, 0, audioBandRange.Length);
        _audioSource = gameObject.GetComponent<AudioSource>();
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
        //DecreaseMaxVals();
    }

    //decreases the freqHighest values acording to NormValueDecreased
    void DecreaseMaxVals()
    {
        for (int i = 0; i < freqBandHighest.Length; i++)
        {
            if(freqBandHighest[i] - NormValDecreaseSpeed > 0)
            {
                freqBandHighest[i] -= NormValDecreaseSpeed;
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
        _audioSource.GetSpectrumData(tempSpamples1, 0, FFTWindow.Blackman);
        float[] tempSpamples2 = new float[512];
        _audioSource.GetSpectrumData(tempSpamples2, 1, FFTWindow.Blackman);

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
                        bufferChange[g] = (freqBand[g] - bandBuffer[g]) * BufferUpMultipler;
                        bandBuffer[g] += bufferChange[g];
                    }

                    if (freqBand[g] < bandBuffer[g])
                    {
                        bufferChange[g] = (bandBuffer[g] - freqBand[g]) * BufferDownMultiplier;
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
            freqBand[i] = average * 10f + 0.00001f; //adds small amount to prevent divide by 0
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

    float AverageBufferAmp;
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

    public float GetAmpBufferAvg()
    {
        return AverageBufferAmp;
    }
}

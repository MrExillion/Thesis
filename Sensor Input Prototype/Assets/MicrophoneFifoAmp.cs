
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;

using System;

public static class MicrophoneFifoAmp
{



    private static ConditionalWeakTable<MMicrophoneFifoAmp, Fields> table;
    
    static MicrophoneFifoAmp()
    {
        table = new ConditionalWeakTable<MMicrophoneFifoAmp, Fields>();
    }
    private sealed class Fields : MonoBehaviour, MTransition
    {
        internal Queue<AudioClip> samplesQueue = new Queue<AudioClip>();
        internal float currentAccretedAudioSampleVolume = 0.0f;
        internal AudioSource audioSource;
        internal int NumberOfFrequencyBands = 32;
        internal int samplingRate;
        internal float[] frequencyBands;
        internal float[] bufferedSamples;
        internal int bufferSize;

    }
    public static void MicrophonoeFifoAmpInitializer(this MMicrophoneFifoAmp map, AudioSource audioSource, int freqBandNums, int samplingRate, int bufferSize)
    {
        if (Mathf.IsPowerOfTwo(freqBandNums) == false)
        {
            throw new ArgumentException(
           "The Number of Frequency Bands MUST be a power of 2", nameof(freqBandNums));
        }
        if (Mathf.IsPowerOfTwo(samplingRate) == false)
        {
            throw new ArgumentException(
           "Sampling rate MUST be a power of 2", nameof(samplingRate));
        }
        if (samplingRate > 8192 || samplingRate < 64)
        {
            throw new ArgumentException(
           "Sampling rate must be between 64 and 8192", nameof(samplingRate));
        }

        table.GetOrCreateValue(map).audioSource = audioSource;
        table.GetOrCreateValue(map).NumberOfFrequencyBands = freqBandNums;
        table.GetOrCreateValue(map).frequencyBands = new float[freqBandNums];
        table.GetOrCreateValue(map).samplingRate = samplingRate;
        table.GetOrCreateValue(map).bufferedSamples = new float[samplingRate];
        table.GetOrCreateValue(map).bufferSize = bufferSize;
    }
    public static void Enqueue(this MMicrophoneFifoAmp map, AudioClip soundSample,int position)
    {
        //float[] samples = new float[soundSample.samples];
        //soundSample.GetData(samples, position);
        //AudioClip audioClip = AudioClip.Create("tempclip", samples.Length, soundSample.channels, soundSample.frequency, false);
        table.GetOrCreateValue(map).samplesQueue.Enqueue(soundSample);


    }
    public static void BufferUpdate(this MMicrophoneFifoAmp map, AudioClip soundSample, int position)
    {
        Queue<AudioClip> audioClips = new(table.GetOrCreateValue(map).samplesQueue);
        if (audioClips.Count < table.GetOrCreateValue(map).bufferSize)
        {
            // just enqueue but dont analyse yet.
            Enqueue(map, soundSample, position);

        }
        else
        {


            float[] spectrumData = new float[table.GetOrCreateValue(map).samplingRate];
            //float[] perRateSpectrumData = new float[table.GetOrCreateValue(map).samplingRate];
            for (int i = 0; i < table.GetOrCreateValue(map).samplesQueue.Count; i++)
            {
                table.GetOrCreateValue(map).audioSource.clip = audioClips.Dequeue();
                table.GetOrCreateValue(map).audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Hanning);

                for (int j = 0; j < spectrumData.Length; j++)
                { table.GetOrCreateValue(map).bufferedSamples[j] += spectrumData[j]; }


            }


            UpdateAvgLoudness(map);
            MakeFrequencyBands(map, table.GetOrCreateValue(map).bufferedSamples);
            
            Enqueue(map, soundSample, position);

        }
        if(table.GetOrCreateValue(map).samplesQueue.Count > table.GetOrCreateValue(map).bufferSize)
        { Dequeue(map); }









    }

    private static void MakeFrequencyBands(MMicrophoneFifoAmp map, float[] _samples)
    {
        int count = 0;
        int bandnumbers = table.GetOrCreateValue(map).NumberOfFrequencyBands;

        for (int i = 0; i < bandnumbers; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            float avg = 0;


            if (i == bandnumbers - 1)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                avg += _samples[count] * (count + 1);
                count++;
            }
            avg /= count;

            table.GetOrCreateValue(map).frequencyBands[i] = avg * 10;

        }




    }



    public static void Dequeue(this MMicrophoneFifoAmp map)
    {
        table.GetOrCreateValue(map).samplesQueue.Dequeue();
        

    }
    public static void UpdateAvgLoudness(MMicrophoneFifoAmp map)
    {

        
        Queue<AudioClip> audioClips = new(table.GetOrCreateValue(map).samplesQueue);


        float accumulatedClipLoudness = 0;


        for (int i = 0; i < audioClips.Count; i++)
        {

            AudioClip audioClip = audioClips.Dequeue();
            AudioSource audioSource = table.GetOrCreateValue(map).audioSource;

           
            float clipLoudness = 0f;
            audioSource.clip = audioClip;
            float[] sampleData = new float[audioClip.samples];
            audioClip.GetData(sampleData, audioSource.timeSamples);
            
           
            foreach(var sample in sampleData)
            {
                // Volume based on accumulated amplitude forced positive
                clipLoudness += Mathf.Abs(sample);
                
            }
            


            accumulatedClipLoudness += clipLoudness / audioClip.samples;




        }
        table.GetOrCreateValue(map).currentAccretedAudioSampleVolume = accumulatedClipLoudness;





        // += audioClip.frequency;
        //table.GetOrCreateValue( map).currentAccretedAudioSampleFrequency += audioClip.frequency;
    }



    public static float GetAvgQueueAmplitude(this MMicrophoneFifoAmp map)
    {

        float valueout = table.GetOrCreateValue(map).currentAccretedAudioSampleVolume;


        return valueout;
    }
    public static float[] GetAvgQueueFrequencyDistribution(this MMicrophoneFifoAmp map)
    {

        float[] valueout = table.GetOrCreateValue(map).frequencyBands;


        return valueout;
    }
    public static void BufferFlush(this MMicrophoneFifoAmp map)
    {

        table.GetOrCreateValue(map).frequencyBands = new float[table.GetOrCreateValue(map).NumberOfFrequencyBands];
        table.GetOrCreateValue(map).samplesQueue.Clear();        
        table.GetOrCreateValue(map).bufferedSamples = new float[table.GetOrCreateValue(map).samplingRate];

    }

}

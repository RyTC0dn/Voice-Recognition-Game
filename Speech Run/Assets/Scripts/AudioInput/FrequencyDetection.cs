using UnityEngine;

/// <summary>This class is a practice script for detecting the loudness of an audio clip, specifically from microphone input. It captures audio data from the microphone and calculates the average loudness over a specified sample window. The loudness is determined by averaging the absolute values of the audio samples within that window, providing a measure of how loud the input is at any given moment.</summary>

public class FrequencyDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        MicrophoneToAudioClip();

        if (Microphone.devices.Length > 0)
        {
            Debug.Log("Microphone connected: " + Microphone.devices[0]);
            // You can also list all devices:
            // foreach (string device in Microphone.devices) { Debug.Log("Device: " + device); }
        }
        else
        {
            Debug.LogWarning("No microphone connected!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void MicrophoneToAudioClip()
    {
        //Get the first microphone in device list
        string microphoneName = Microphone.devices[0];

        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    /// <summary>
    /// Calculates the average loudness of an audio clip within a specified sample window at the given position.
    /// </summary>
    /// <remarks>The method analyzes a window of samples ending at the specified position. If the position is
    /// near the start of the clip, ensure that the sample window does not exceed the clip's bounds. The loudness is
    /// calculated as the mean of the absolute values of the samples in the window.</remarks>
    /// <param name="clipPosition">The position, in samples, within the audio clip from which to start calculating loudness. Must be greater than
    /// or equal to the sample window size.</param>
    /// <param name="clip">The audio clip to analyze. Cannot be null.</param>
    /// <returns>A floating-point value representing the average loudness of the audio segment. Higher values indicate greater
    /// loudness.</returns>
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class AudioInputController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Audio Settings")]
    private AudioClip audioClip;

    public int sampleWindow = 64;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    [Space(20)]
    [SerializeField]
    private bool isStop, moveBack, isGround;

    private bool input = false;
    private bool wordRecognized = false;

    [Header("Character Settings")]
    [SerializeField]
    private float characterSpeed = 1.0f;

    [SerializeField]
    private float jumpForce = 2.0f;

    [SerializeField]
    [Range(0, 100)] private float detectionRange = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        MicrophoneAudio();

        #region Keyword Dictionary

        //Player movement commands
        keywordActions.Add("turn right", TurnRight);
        keywordActions.Add("turn left", TurnLeft);
        keywordActions.Add("move", Forward);
        keywordActions.Add("stop", Stop);
        keywordActions.Add("jump", Jump);

        //Menu commands
        keywordActions.Add("close", Back);
        keywordActions.Add("open", OpenMenu);

        #endregion Keyword Dictionary

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
        keywordRecognizer.Start();
    }

    private void FixedUpdate()
    {
        InputHandler();
        AirBehaviour();
    }

    private void OnKeywordRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        GameManager.Instance.DisplayCommand(args.text);
        keywordActions[args.text].Invoke();
        wordRecognized = true;
    }

    public void MicrophoneAudio()
    {
        //Grabs the microphone device from input list
        string microphoneName = Microphone.devices[0];

        //Have the microphone start recording commands
        audioClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    private void InputHandler()
    {
        //Handles the movement of the character based on the voice commands
        if (!isStop)
        {
            //Moves the character forward at consistent speed
            rb.linearVelocity = new Vector3(transform.forward.x * characterSpeed, rb.linearVelocity.y, transform.forward.z * characterSpeed);
        }
        else if (moveBack)
        {
            rb.linearVelocity = new Vector3(-transform.forward.x * characterSpeed, rb.linearVelocity.y, -transform.forward.z * characterSpeed);
        }
        if (isStop)
        {
            rb.linearVelocity = new Vector3(transform.position.x * 0, rb.linearVelocity.y, transform.position.z * 0);
        }
    }

    private void AirBehaviour()
    {
        if (input)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            input = false;
        }
    }

    #region Function Actions

    //List of actions for the object

    private void TurnLeft()
    {
        transform.Rotate(0, -45, 0);
    }

    private void TurnRight()
    {
        transform.Rotate(0, 45, 0);
    }

    private void Forward()
    {
        isStop = false;
    }

    private void Back()
    {
        GameManager.Instance.HideMenu();
    }

    private void OpenMenu()
    {
        GameManager.Instance.ShowMenu();
    }

    private void Stop()
    {
        isStop = true;
    }

    private void Jump()
    {
        input = true;
    }

    #endregion Function Actions

    #region Audio Analysis

    /// <summary>
    /// Gets the current loudness level from the microphone input.
    /// </summary>
    /// <remarks>This method retrieves the loudness by accessing the microphone's audio input and processing
    /// it through the specified audio clip. Ensure that the microphone is properly initialized and that audio
    /// permissions are granted before calling this method.</remarks>
    /// <returns>A float representing the loudness level, measured in decibels, based on the audio captured from the microphone.</returns>
    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), audioClip);
    }

    /// <summary>
    /// Calculates the loudness from a given audio clip.
    /// </summary>
    /// <param name="clipPosition">The current position in the audio clip.</param>
    /// <param name="clip">The audio clip to analyze.</param>
    /// <returns>The calculated loudness.</returns>
    private float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);
        float totalLoudness = 0;
        foreach (float sample in waveData)
        {
            totalLoudness += Mathf.Abs(sample);
        }
        return totalLoudness / 128;
    }

    #endregion Audio Analysis
}
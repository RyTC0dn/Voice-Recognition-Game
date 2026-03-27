using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class AudioInputController : MonoBehaviour
{
    private Rigidbody rb;

    private AudioClip audioClip;

    private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    [SerializeField]
    private bool isStop, moveBack, isGround;

    private bool input = false;

    [SerializeField]
    [Range(0, 1)] private float characterSpeed = 1.0f;

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

        keywordActions.Add("turn right", TurnRight);
        keywordActions.Add("turn left", TurnLeft);
        keywordActions.Add("start", Forward);
        keywordActions.Add("back", Back);
        keywordActions.Add("stop", Stop);
        keywordActions.Add("jump", Jump);

        #endregion Keyword Dictionary

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
        keywordRecognizer.Start();
    }

    private void FixedUpdate()
    {
        InputHandler();
        AirBehaviour();
        //isGround = Physics.Raycast(transform.position, Vector3.down, detectionRange, jumpableLayer);
    }

    private void OnKeywordRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywordActions[args.text].Invoke();
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
        if (!isStop)
        {
            rb.AddForce(transform.forward * characterSpeed, ForceMode.Impulse);
        }
        else if (moveBack)
        {
            rb.AddForce(-transform.forward * characterSpeed, ForceMode.Impulse);
        }
        else if (isStop)
        {
            rb.AddForce(transform.position * 0, ForceMode.Impulse);
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
        moveBack = true;
    }

    private void Stop()
    {
        isStop = true;
    }

    private void Jump()
    {
        input = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, Vector3.down * detectionRange);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody characterPrefab;

    private AudioClip audioClip;

    private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    [SerializeField] private bool isStop = true;
    [SerializeField] private float characterSpeed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        MicrophoneAudio();
        keywordActions.Add("turn right", TurnRight);
        keywordActions.Add("turn left", TurnLeft);
        keywordActions.Add("start", Forward);
        keywordActions.Add("back", Back);
        keywordActions.Add("stop", Stop);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
        keywordRecognizer.Start();
    }

    private void Update()
    {
        if (!isStop)
        {
            transform.position += Vector3.forward * characterSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.zero;
        }
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
        transform.position = Vector3.back;
    }

    private void Stop()
    {
        isStop = true;
    }
}
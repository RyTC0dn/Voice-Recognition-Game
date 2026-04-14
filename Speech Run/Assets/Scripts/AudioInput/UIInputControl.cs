using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class UIInputControl : MonoBehaviour
{
    private AudioClip m_AudioClip;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        MicrophoneAudio();

        //Menu commands
        keywords.Add("start", StartGame);
        keywords.Add("menu", LoadMainMenu);
        keywords.Add("quit", CloseGame);

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
        keywordRecognizer.Start();
    }

    private void OnKeywordRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        GameManager.Instance.DisplayCommand(args.text);
        keywords[args.text].Invoke();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void MicrophoneAudio()
    {
        //Grabs the microphone device from input list
        string microphoneName = Microphone.devices[0];

        //Have the microphone start recording commands
        m_AudioClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    #region Menu Methods

    public void StartGame()
    {
        SceneManager.LoadScene("Main Level");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void CloseGame()
    {
        // If we are running in a standalone build of the game
        if (Application.isPlaying)
            Application.Quit();//Close the application
    }

    #endregion Menu Methods
}
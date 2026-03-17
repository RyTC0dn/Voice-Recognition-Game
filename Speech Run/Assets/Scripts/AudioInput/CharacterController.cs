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

    private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        keywordActions.Add("turn right", TurnRight);
        keywordActions.Add("turn left", TurnLeft);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
        keywordRecognizer.Start();
    }

    private void OnKeywordRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywordActions[args.text].Invoke();
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
}
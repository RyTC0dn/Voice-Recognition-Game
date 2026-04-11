using TMPro;
using UnityEngine;

public enum GameStates
{
    None,
    Play,
    Pause
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI commandText;
    public GameObject menuPanel;
    private float timer;

    [Space(20)]
    public GameObject startPos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        menuPanel.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        SetTimer();
    }

    #region Menu Methods

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        ChangeStates(GameStates.Pause);
    }

    public void HideMenu()
    {
        menuPanel.SetActive(false);
        ChangeStates(GameStates.Play);
    }

    #endregion Menu Methods

    // Set the timer and update the time text
    private void SetTimer()
    {
        timer += Time.deltaTime;
        timeText.text = "Time: " + timer.ToString("F2");
    }

    public void DisplayCommand(string command)
    {
        commandText.text = "Command: " + command;
    }

    public void ChangeStates(GameStates state)
    {
        switch (state)
        {
            case GameStates.None:
                break;

            case GameStates.Play:
                Time.timeScale = 1f;
                break;

            case GameStates.Pause:
                Time.timeScale = 0f;
                break;

            default:
                break;
        }
    }
}
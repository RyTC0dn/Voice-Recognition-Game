using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject pauseMenu;
    public GameObject menuUI;
    [HideInInspector] public static float timer;

    public bool isPaused = false;

    [Space(20)]
    public GameObject startPos;

    [HideInInspector] public string sceneName;
    private GameObject player;

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
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "EndScreen")
        {
            timeText = GameObject.Find("EndTimeText").GetComponent<TextMeshProUGUI>();
            EndTimer();
        }

        player = GameObject.Find("Player");
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (sceneName == "Main Level")
            SetTimer();
    }

    #region Menu Methods

    public void ShowMenu()
    {
        if (!isPaused) return;

        menuUI.SetActive(false);
        pauseMenu.SetActive(false);

        menuPanel.SetActive(true);

        ChangeStates(GameStates.Pause);
    }

    public void BackToPause()
    {
        if (!isPaused) return;

        menuPanel.SetActive(false);

        pauseMenu.SetActive(true);

        ChangeStates(GameStates.Play);
    }

    public void Resume()
    {
        menuPanel.SetActive(false);
        pauseMenu.SetActive(false);

        menuUI.SetActive(true);

        isPaused = false;
        ChangeStates(GameStates.Play);
    }

    public void PauseMenu()
    {
        menuUI.SetActive(false);
        menuPanel.SetActive(false);

        pauseMenu.SetActive(true);

        isPaused = true;
        ChangeStates(GameStates.Pause);
    }

    #endregion Menu Methods

    // Set the timer and update the time text
    private void SetTimer()
    {
        timer += Time.deltaTime;
        timeText.text = "Time: " + timer.ToString("F2");
    }

    private void EndTimer()
    {
        timeText.text = "Your Time was: " + GameManager.timer.ToString("F2");
    }

    public void DisplayCommand(string command)
    {
        commandText.text = "Command: " + command;
    }

    public void EndGame()
    {
        SceneManager.LoadScene("EndScreen");
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
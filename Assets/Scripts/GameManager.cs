using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Players")]
    public MovementController player1;
    public MovementController player2;

    [Header("Score UI")]
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI timerText1;
    public TextMeshProUGUI timerText2;

    [Header("Game Settings")]
    public int fishToWin = 5;
    public float matchTime = 300f; 

    [Header("End Game UI")]
    public GameObject endPanel;         
    public TextMeshProUGUI endTitleText; 
    public TextMeshProUGUI endScoreText; 
    public GameObject pausePanel;      
    public KeyCode pauseKey = KeyCode.Escape;

    private int player1Score = 0;
    private int player2Score = 0;
    private float timeLeft;
    private bool gameOver = false;
    private bool paused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeLeft = matchTime;
        endPanel.SetActive(false);
        pausePanel.SetActive(false);
        UpdateUI();
    }

    private void Update()
    {
        if (gameOver) return;

     
        if (Input.GetKeyDown(pauseKey))
            TogglePause();

        if (paused) return;

       
        timeLeft -= Time.deltaTime;
        UpdateTimerUI();

        if (timeLeft <= 0f)
            EndGame(null); 
    }



    public void FishCollected(MovementController player)
    {
        if (gameOver) return;
        //SoundManager.Instance.StopAll();

        if (player == player1)
        {
            player1Score++;
            player1.ApplyFishPenalty(player1Score);
            UpdateUI();
            if (player1Score >= fishToWin) EndGame(player1);
        }
        else if (player == player2)
        {
            player2Score++;
            player2.ApplyFishPenalty(player2Score);
            UpdateUI();
            if (player2Score >= fishToWin) EndGame(player2);
        }
    }



    private void EndGame(MovementController winner)
    {
        gameOver = true;
        Time.timeScale = 0f; 

        endPanel.SetActive(true);

        if (winner == null)
            endTitleText.text = "Time Out!";
        else if (winner == player1)
            endTitleText.text = "Cat 1 Wins!";
        else
            endTitleText.text = "Cat 2 Wins!";

        endScoreText.text = "Cat 1: "+player1Score+ " | " +"Cat 2: "+player2Score;
    }



    public void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
        pausePanel.SetActive(paused);
    }

   

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ResumeGame()
    {
        TogglePause();
    }



    private void UpdateUI()
    {
        player1ScoreText.text = "Cat 1: " + player1Score+"/5";
        player2ScoreText.text = "Cat 2: " + player2Score + "/5";
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timerText1.text = "Time Left "+minutes+":"+seconds;
        timerText2.text = "Time Left " + minutes + ":" + seconds;
        //if (timeLeft <= 10f && timeLeft > 0f)
        //{
        //    if (Mathf.FloorToInt(timeLeft) != Mathf.FloorToInt(timeLeft + Time.deltaTime))
        //        SoundManager.Instance.PlayBeep();
        //}
    }
}


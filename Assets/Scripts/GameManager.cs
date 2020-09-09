using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public enum stateMenu
    {
        Main ,
        Ingame,
        Aftergame , 
        Scores,
        Settings,
        About,
        None
    }

    public static GameManager instance;

    public int width = 10;
    public float speedGame = 0.7f;

    public int points = 0;
    public float pointsPerBlock = 10f;
    public float multiplierPoint = 1.2f;
    public int endBorder = 9;

    public stateMenu currentPosMenu;

    public bool isConnection = false;

    [Space()]
    public RectTransform inGame;
    public Text points_text;
    public Text yourBest_text;
    public Text globalBest_text;

    [Space()]
    public RectTransform afterGame;
    public InputField namePlayer;
    public Text currentScore;


    [Space()]
    public RectTransform menu;


    [Space()]
    public RectTransform scores;
    public Text[] tx_scores;

    [Space()]
    public RectTransform settings;

    [Space()]
    public RectTransform about;


    [Space()]
    public bool endGame = false;

    [HideInInspector]
    public Board board;
    [HideInInspector]
    public ScoresManager scoreManager;
    [HideInInspector]
    public Background background;
    [HideInInspector]
    public SoundManager soundManager;
    private Camera mainCamera;

    public List<Difficulty> diff = new List<Difficulty>();
    public AnimationCurve levelOfHard; 

    public Camera MainCamera
    {
        get
        {
            return mainCamera;
        }
    }

    private void Awake()
    {
        

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(instance);

        mainCamera = Camera.main;
        board = GetComponent<Board>();
        scoreManager = GetComponent<ScoresManager>();
        background = GetComponent<Background>();
        soundManager = GetComponent<SoundManager>();

        scoreManager.TestConnection();

        scoreManager.GetScore();
    }

    public void Init()
    {
        endGame = false;

        currentPosMenu = stateMenu.Ingame;

        float y = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 1f)).y;
        float x = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 1f)).x;


        y = Mathf.Round(y) + 1;
        x = Mathf.Round(x);

        Vector2 c_d = new Vector2(x, y);

        y = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 1f)).y;
        y = Mathf.Round(y) + 1;

        Vector2 c_u = new Vector2(x, y);

        Board._width = width;
        Board.speedGame = speedGame;
        points = 0;

        scoreManager.GetScore();

        board.Init(c_d, c_u);

    }


    private void Update()
    {
        if (points_text != null)
            points_text.text = "Points: " + points;

        Board.speedGame = levelOfHard.Evaluate((float)(Board.step + (points * 0.03f)));

        switch (currentPosMenu)
        {
            case stateMenu.Main:
                inGame.gameObject.SetActive(false);
                afterGame.gameObject.SetActive(false);
                menu.gameObject.SetActive(true);
                scores.gameObject.SetActive(false);
                settings.gameObject.SetActive(false);
                about.gameObject.SetActive(false);
                endGame = true;
                break;
            case stateMenu.Ingame:
                inGame.gameObject.SetActive(true);
                afterGame.gameObject.SetActive(false);
                menu.gameObject.SetActive(false);
                scores.gameObject.SetActive(false);
                settings.gameObject.SetActive(false);
                about.gameObject.SetActive(false);
                endGame = false;
                break;
            case stateMenu.Aftergame:
                inGame.gameObject.SetActive(false);
                afterGame.gameObject.SetActive(true);
                menu.gameObject.SetActive(false);
                scores.gameObject.SetActive(false);
                settings.gameObject.SetActive(false);
                about.gameObject.SetActive(false);
                endGame = true;
                break;
            case stateMenu.Scores:
                inGame.gameObject.SetActive(false);
                afterGame.gameObject.SetActive(false);
                menu.gameObject.SetActive(false);
                scores.gameObject.SetActive(true);
                settings.gameObject.SetActive(false);
                about.gameObject.SetActive(false);
                endGame = true;
                break;
            case stateMenu.Settings:
                inGame.gameObject.SetActive(false);
                afterGame.gameObject.SetActive(false);
                menu.gameObject.SetActive(false);
                scores.gameObject.SetActive(false);
                settings.gameObject.SetActive(true);
                about.gameObject.SetActive(false);
                endGame = true;
                break;
            case stateMenu.About:
                inGame.gameObject.SetActive(false);
                afterGame.gameObject.SetActive(false);
                menu.gameObject.SetActive(false);
                scores.gameObject.SetActive(false);
                settings.gameObject.SetActive(false);
                about.gameObject.SetActive(true);
                endGame = true;
                break;
            case stateMenu.None:
                inGame.gameObject.SetActive(false);
                afterGame.gameObject.SetActive(false);
                menu.gameObject.SetActive(false);
                scores.gameObject.SetActive(false);
                settings.gameObject.SetActive(false);
                about.gameObject.SetActive(false);
                endGame = true;
                break;

        }



    }


    public void StartGame()
    {
        endGame = true;
        Init();
        currentPosMenu = stateMenu.Ingame;
    }

    public void CancelGame()
    {
        endGame = false;
        board.GameOver();
        currentPosMenu = stateMenu.Main;
    }


    private void OnGUI()
    {
        #if UNITY_EDITOR
        GUI.Label(new Rect(0, 0, 80, 100), "Steps: " + Board.step + "\nSpeed:" + Board.speedGame + "\nX:" + (float)(Board.step + (points * 0.03f)));
        if(GUI.Button(new Rect(80, 0, 120, 40) , "RESET SCORE"))
        {
            StartCoroutine(scoreManager.DeleteAll());
            
        }
        

        #endif

    }
    public void SetTextScore(int i)
    {
        if (yourBest_text != null)
            yourBest_text.text = "Your Best Score: " + i;
    }


    public void SetTextScore(string i)
    {
        if (globalBest_text != null)
            globalBest_text.text = "Bestest Score: " + i;

        if(!isConnection)
            globalBest_text.text = "";

    }


    public void GameOver()
    {
        currentPosMenu = stateMenu.Aftergame;

        if (currentScore != null)
            currentScore.text = "Your current score: " + points;


    }


    public void SaveScore()
    {
        string namePlayer_t = namePlayer.text;

        if (string.IsNullOrEmpty(namePlayer_t))
            return;

        currentPosMenu = stateMenu.None;

        if (!string.IsNullOrEmpty(namePlayer_t))
        {
            scoreManager.SaveScore(namePlayer_t, points);
            points = 0;

        }

       // Init();

    }

    public void ShowSettings()
    {
        currentPosMenu = stateMenu.Settings;
        //scoreManager.GetScore(true);

    }

    public void ShowAbout()
    {
        currentPosMenu = stateMenu.About;
    }

    public void ShowScores()
    {

        currentPosMenu = stateMenu.None;

        scoreManager.GetScore(true);

        if (!isConnection)
            return;

        //currentPosMenu = stateMenu.Scores;

      
    }

    public void BackToMenu()
    {
        currentPosMenu = stateMenu.Main;
    }


    public void Quit()
    {
        Application.Quit();
    }


}


[System.Serializable]
public class Difficulty
{
    public int steps = 0;
    public float speed = 1f;


    public Difficulty(int _steps , float _speed)
    {
        this.steps = _steps;
        this.speed = _speed;

    }

}

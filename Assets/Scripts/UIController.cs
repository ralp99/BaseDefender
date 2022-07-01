using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Text CurrentScoreText;
    public Text HiscoreText;
    public Text CurrentLevelText;
    public Text TopLevelText;
    public GameObject CanvasObject;
    RAGameManager rAGameManager;

    void GameManagerCheck()
    {
        if (!rAGameManager)
        {
            rAGameManager = RAGameManager.Instance;
        }
    }

     void UpdateScore(int newScore)
    {
        CurrentScoreText.text = ("Score: "+rAGameManager.CurrentScore);
    }

     void UpdateHighscore()
    {
        HiscoreText.text = ("Hi Score: "+ rAGameManager.HiScore);
    }

    void UpdateLevel(int newLevel)
    {
        CurrentLevelText.text = ("Level: " + rAGameManager.CurrentGameLevel);
    }

    void UpdateTopLevel()
    {
        TopLevelText.text = ("Top Level: " + rAGameManager.TopGameLevel);
    }

    public void ClickEvents_NewGameStart()
    {
        rAGameManager.RestartGame();
    }

    public void UIVisibility(bool state)
    {
        CanvasObject.SetActive(state);
        if (state)
        {
            GameManagerCheck();
            UpdateHighscore();
            UpdateTopLevel();
            UpdateScore(rAGameManager.CurrentScore);
            UpdateLevel(rAGameManager.CurrentGameLevel);
        }
    }


}

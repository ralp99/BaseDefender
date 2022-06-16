using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Text ScoreText;
    public Text HiscoreText;
    public GameObject CanvasObject;
    RAGameManager rAGameManager;

    void Start()
    {
        rAGameManager = RAGameManager.Instance;
    }

     void UpdateScore(int newScore)
    {
        ScoreText.text = ("Score: "+rAGameManager.CurrentScore);
    }

     void UpdateHighscore()
    {
        HiscoreText.text = ("Hi Score: "+ rAGameManager.HiScore);
    }
 

    public void ClickEvents_NewGameStart()
    {
        rAGameManager.RestartGame();
    }

    public void UIVisibility(bool state)
    {
        CanvasObject.SetActive(state);
    }


}

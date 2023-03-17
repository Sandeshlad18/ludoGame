using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject gamePanel;


    public void game1()
    {
        GameManager.gm.totalPlayerCanPlay = 2;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        GameSeting1();
    }
    public void game2()
    {
        GameManager.gm.totalPlayerCanPlay = 3;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        GameSeting2();
    }
    public void game3()
    {
        GameManager.gm.totalPlayerCanPlay = 4;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    public void game4()
    {
        GameManager.gm.totalPlayerCanPlay = 1;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        GameSeting1();
    }




    void GameSeting1()
    {
        HidePlayers(GameManager.gm.greenPlayerPice);
        HidePlayers(GameManager.gm.bluePlayerPice);
    }
    void GameSeting2()
    {
        HidePlayers(GameManager.gm.greenPlayerPice);
    }


    void HidePlayers(PlayerPiece[] PlayerPieces_)
    {
        for (int i = 0; i < PlayerPieces_.Length; i++)
        {
            PlayerPieces_[i].gameObject.SetActive(false);
        }
    }


}

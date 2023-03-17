using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int StepsToMove;
    public RollingDice rolledDice;
    public bool canPlayerMove;

    public bool canDiceRole ;
    public bool transferDice;
    public bool selfDice ;

    public int redPlayerOut;
    public int bluePlayerOut;
    public int yellowPlayerOut;
    public int greenPlayerOut;

    public int redPlayerCompleted;
    public int bluePlayerCompleted;
    public int yellowPlayerCompleted;
    public int greenPlayerCompleted;

    public RollingDice[] managerRollingDice;

    public PlayerPiece[] bluePlayerPice;
    public PlayerPiece[] redPlayerPice;
    public PlayerPiece[] greenPlayerPice;
    public PlayerPiece[] yellowPlayerPice;

    public int totalPlayerCanPlay;

    List<PathPoint> playerOnPathPointList = new List<PathPoint>();


    private void Awake()
    {
        canDiceRole = true;
        transferDice = false;
        selfDice = false;
        gm = this;
    }

    public void AddPathPoint(PathPoint pathPoint)
    {
        playerOnPathPointList.Add(pathPoint);
    }
    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (playerOnPathPointList.Contains(pathPoint))
        {
            playerOnPathPointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("path point remove not found");
        }
    }

    public void RollingDiceManager()
    {
        
        if (GameManager.gm.transferDice)
        {
            if (GameManager.gm.StepsToMove != 6)
            {
                ShiftDice();
            }
            GameManager.gm.canDiceRole = true;
        }
        else
        {
            if (GameManager.gm.selfDice)
            {
                GameManager.gm.canDiceRole = true;
                GameManager.gm.selfDice = false;
                GameManager.gm.SelfRoal();
            }
        }
    }
    public void SelfRoal()
    {
        if (GameManager.gm.totalPlayerCanPlay == 1 && GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[2])
        {
            Invoke("roled",0.6f);
        }
    }
    //litel bit delay
    void roled()
    {
        GameManager.gm.managerRollingDice[2].mouseRoll();
    }
    void ShiftDice()
    {
        int nextDice;
        //auto player
        if (GameManager.gm.totalPlayerCanPlay == 1)
        {
            if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[0])
            {
                GameManager.gm.managerRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.managerRollingDice[2].gameObject.SetActive(true);
                PassOut(0);
                GameManager.gm.managerRollingDice[2].mouseRoll();
            }
            else
            {
                GameManager.gm.managerRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.managerRollingDice[2].gameObject.SetActive(false);
                PassOut(2);
            }
        }


        //manual player
        else if (GameManager.gm.totalPlayerCanPlay == 2)
        {
            if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[0])
            {
                GameManager.gm.managerRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.managerRollingDice[2].gameObject.SetActive(true);
                PassOut(0);
            }
            else
            {
                GameManager.gm.managerRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.managerRollingDice[2].gameObject.SetActive(false);
                PassOut(2);
            }
        }
        else if (GameManager.gm.totalPlayerCanPlay == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                {
                    if (i == 2)
                    { nextDice = 0; }
                    else
                    { nextDice = i + 1; }
                    i= PassOut(i);
                    if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[i])
                    {
                        GameManager.gm.managerRollingDice[i].gameObject.SetActive(false);
                        GameManager.gm.managerRollingDice[nextDice].gameObject.SetActive(true);
                    }
                }
            }
        }
        else 
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                { nextDice = 0; }
                else
                { nextDice = i + 1; }
                i = PassOut(i);
                if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[i])
                {
                    GameManager.gm.managerRollingDice[i].gameObject.SetActive(false);
                    GameManager.gm.managerRollingDice[nextDice].gameObject.SetActive(true);
                }
            }
        }
    }
    int PassOut(int i)
    {
        if (i == 0) { if (GameManager.gm.redPlayerCompleted == 4) { return i + 1; } }
        else if (i == 1) { if (GameManager.gm.redPlayerCompleted == 4) { return i + 1; } }
        else if (i == 2) { if (GameManager.gm.redPlayerCompleted == 4) { return i + 1; } }
        else if (i == 3) { if (GameManager.gm.bluePlayerCompleted == 4) { return i + 1; } }
        return i;
    }
}

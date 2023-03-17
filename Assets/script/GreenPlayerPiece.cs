using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerPiece : PlayerPiece
{
    RollingDice greenHomeRolDice;

    private void Start()
    {
        greenHomeRolDice = GetComponentInParent<GreenHome>().rollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManager.gm.rolledDice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.rolledDice == greenHomeRolDice && GameManager.gm.StepsToMove == 6)
                {
                    GameManager.gm.greenPlayerOut += 1;
                    MakePlayerReadyToMove(pathParent.greenPath);
                    GameManager.gm.StepsToMove = 0;
                    return;
                }
            }
            if (GameManager.gm.rolledDice == greenHomeRolDice && isReady&&GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
               
                MoveSteps(pathParent.greenPath);
            }
        }
       

    }
}

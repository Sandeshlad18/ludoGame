using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerPiece : PlayerPiece
{
    RollingDice yellowHomeRolDice;

    private void Start()
    {
        yellowHomeRolDice = GetComponentInParent<YellowHome>().rollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManager.gm.rolledDice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.rolledDice == yellowHomeRolDice && GameManager.gm.StepsToMove == 6)
                {
                    GameManager.gm.yellowPlayerOut += 1;
                    MakePlayerReadyToMove(pathParent.yellowPath);
                    GameManager.gm.StepsToMove = 0;
                    return;
                }
            }
            if (GameManager.gm.rolledDice == yellowHomeRolDice && isReady&&GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                
                MoveSteps(pathParent.yellowPath);
            }
        }
       

    }
}

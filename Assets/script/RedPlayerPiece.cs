using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RedPlayerPiece : PlayerPiece
{
    RollingDice redHomeRolDice;

    private void Start()
    {
        redHomeRolDice = GetComponentInParent<RedHome>().rollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManager.gm.rolledDice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.rolledDice == redHomeRolDice && GameManager.gm.StepsToMove == 6)
                {
                    GameManager.gm.redPlayerOut += 1;
                    MakePlayerReadyToMove(pathParent.redPath);
                    GameManager.gm.StepsToMove = 0;
                    return;
                }
            }
            if (GameManager.gm.rolledDice == redHomeRolDice && isReady &&GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                
                MoveSteps(pathParent.redPath);

            }
        }
        
        

    }


}

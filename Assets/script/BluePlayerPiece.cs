using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlayerPiece : PlayerPiece
{
    RollingDice blueHomeRolDice;

    private void Start()
    {
        blueHomeRolDice = GetComponentInParent<BlueHome>().rollingDice;
    }
    private void OnMouseDown()
    {
        if (GameManager.gm.rolledDice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.rolledDice == blueHomeRolDice && GameManager.gm.StepsToMove == 6)
                {
                    GameManager.gm.bluePlayerOut += 1;
                    MakePlayerReadyToMove(pathParent.bluePath);
                    GameManager.gm.StepsToMove = 0;
                    return;
                }
            }
            if (GameManager.gm.rolledDice == blueHomeRolDice && isReady&&GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                
                MoveSteps(pathParent.bluePath);
            }
        }
       

    }
}

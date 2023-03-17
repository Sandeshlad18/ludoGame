using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public bool isReady;
    private bool onmove;
    public int numberOfStepsAlreadyMove;

    public PathObjParent pathParent;
    public PathPoint previousePath;
    public PathPoint currentPath;

    Coroutine moveSteps_coroutine;

    private void Awake()
    {
        pathParent = FindObjectOfType<PathObjParent>();
    }

    public void MoveSteps(PathPoint[] pathPointToMove_)
    {
        moveSteps_coroutine = StartCoroutine(MoveStepEnum(pathPointToMove_));
    }

    public void MakePlayerReadyToMove(PathPoint[] pathPointToMove_)
    {
        isReady = true;
        transform.position = pathPointToMove_[0].transform.position;
        numberOfStepsAlreadyMove = 1;

        previousePath = pathPointToMove_[0];
        currentPath = pathPointToMove_[0];
        currentPath.AddPlayerPices(this);
        // GameManager.gm.RemovePathPoint(previousePath);
        GameManager.gm.AddPathPoint(currentPath);

        GameManager.gm.canDiceRole = true;
        GameManager.gm.selfDice = true;
        GameManager.gm.transferDice = false;
    }

    IEnumerator MoveStepEnum(PathPoint[] pathPointToMove_)
    {

        GameManager.gm.transferDice = false;
        yield return new WaitForSeconds(0.25f);
        int numberofMoveStep = GameManager.gm.StepsToMove;

        
            for (int i = numberOfStepsAlreadyMove; i <(numberOfStepsAlreadyMove+ numberofMoveStep); i++)
            {
            currentPath.rescaleAndRepositionAllPlayerPices();
                if (isPathPointAvailableToMove(numberofMoveStep, numberOfStepsAlreadyMove, pathPointToMove_))
                {
                    transform.position = pathPointToMove_[i].transform.position;
                    yield return new WaitForSeconds(0.20f);
                }
            }

        GameManager.gm.StepsToMove = 0;
        if (isPathPointAvailableToMove(numberofMoveStep, numberOfStepsAlreadyMove, pathPointToMove_))
        {
            
            numberOfStepsAlreadyMove += numberofMoveStep;

            GameManager.gm.RemovePathPoint(previousePath);
            previousePath.RemovePlayerPieces(this);
            currentPath = pathPointToMove_[numberOfStepsAlreadyMove-1];

            if (currentPath.AddPlayerPices(this))
            {
                if (numberOfStepsAlreadyMove == 57)
                {
                    GameManager.gm.selfDice = true;
                }
                else
                {
                    if (GameManager.gm.StepsToMove != 6)
                    {
                        GameManager.gm.transferDice = true;
                        GameManager.gm.canDiceRole = true;
                    }
                    else
                    {
                        GameManager.gm.selfDice = false;
                        GameManager.gm.canDiceRole = true;
                    }
                }

            }
            else
            {
                GameManager.gm.selfDice = true;
            }

            GameManager.gm.AddPathPoint(currentPath);
            previousePath = currentPath;


            //GameManager.gm.StepsToMove = 0;
        }
        GameManager.gm.canPlayerMove = true;
        GameManager.gm.RollingDiceManager();

        if (moveSteps_coroutine != null)
        {
            StopCoroutine(moveSteps_coroutine);
        }
    }

    bool isPathPointAvailableToMove(int numOfStepsToMove,int numOfStepsAlredyMove,PathPoint[] pathPointsToMoveOn)
    {
        if (numOfStepsToMove == 0)
        {
            return false;
        }
        int leftNumOfPathPoints = pathPointsToMoveOn.Length - numOfStepsAlredyMove;

        if (leftNumOfPathPoints >= numOfStepsToMove)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

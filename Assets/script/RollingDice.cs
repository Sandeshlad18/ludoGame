using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    public int numGot;
    [SerializeField] SpriteRenderer spriteHolder;
    [SerializeField] Sprite[] numberSprite;
    public GameObject rollingDice;

    int outPieces;

    PlayerPiece[] currentplayerpices;
    PathPoint[] pathPointToMoveOn;

    Coroutine genarateRandomNumOnDice_coroutine;

    Coroutine moveSteps_coroutine;
    PlayerPiece outPlayerPiece;

    PathObjParent pathParent;

    private void Awake()
    {
        pathParent = FindObjectOfType<PathObjParent>();
    }

    private void OnMouseDown()
    {
       
        genarateRandomNumOnDice_coroutine = StartCoroutine(GenerateRandomNumOnDice());
    }


    public void mouseRoll()
    {

        genarateRandomNumOnDice_coroutine = StartCoroutine(GenerateRandomNumOnDice());
    }


    IEnumerator GenerateRandomNumOnDice()
    {
        if (GameManager.gm.canDiceRole)
        {

            GameManager.gm.canDiceRole = false;
            spriteHolder.gameObject.SetActive(false);
            rollingDice.SetActive(true);

            yield return new WaitForSeconds(1f);
            numGot = Random.Range(0, 6);
            spriteHolder.sprite = numberSprite[numGot];
            numGot += 1;

            GameManager.gm.StepsToMove = numGot;
            GameManager.gm.rolledDice = this;

            spriteHolder.gameObject.SetActive(true);
            rollingDice.SetActive(false);

            yield return new WaitForEndOfFrame();

            int numberGot = GameManager.gm.StepsToMove;
            if (PlayerCanNotMove())
             {
                yield return new WaitForSeconds(0.5f);

                if (numberGot != 6)
                {
                    GameManager.gm.transferDice = true;
                }
                else
                {
                    GameManager.gm.selfDice = true;
                }
             }
             else
             {

                if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[0])
                {
                    outPieces = GameManager.gm.redPlayerOut;
                }
                else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[1])
                {
                    outPieces = GameManager.gm.bluePlayerOut;
                }
                else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[2])
                {
                    outPieces = GameManager.gm.yellowPlayerOut;
                }
                else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[3])
                {
                    outPieces = GameManager.gm.greenPlayerOut;
                }


                if (outPieces == 0 && numberGot != 6)
                {
                    yield return new WaitForSeconds(0.5f);
                    GameManager.gm.transferDice = true;
                }
                else
                {
                    if (outPieces == 0 && numberGot == 6)
                    {
                        MakePlayerReadyToMove(0);
                    }
                    else if (outPieces == 1 && numberGot != 6 && GameManager.gm.canPlayerMove)
                    {
                        int playerPicePosition = CheckOutPlayer();
                        if (playerPicePosition >= 0)
                        {
                            GameManager.gm.canPlayerMove = false;
                            moveSteps_coroutine = StartCoroutine(MoveStepEnum(playerPicePosition));
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                            if (numberGot != 6)
                            { GameManager.gm.transferDice = true; }
                            else
                            { GameManager.gm.selfDice = true; }
                        }
                    }
                    //for robot
                    else if (GameManager.gm.totalPlayerCanPlay == 1 && GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[2])
                    {
                        if (numberGot != 6 && outPieces < 4)
                        {
                            MakePlayerReadyToMove(outPlayerToMove());
                        }
                        else
                        {
                            int playerPicePosition = CheckOutPlayer();
                            if (playerPicePosition >= 0)
                            {
                                GameManager.gm.canPlayerMove = false;
                                moveSteps_coroutine = StartCoroutine(MoveStepEnum(playerPicePosition));

                            }
                            else
                            {
                                yield return new WaitForSeconds(0.5f);
                                if (numberGot != 6)
                                { GameManager.gm.transferDice = true; }
                                else
                                { GameManager.gm.selfDice = true; }
                            }
                        }

                    }
                }

                GameManager.gm.RollingDiceManager();

                if (genarateRandomNumOnDice_coroutine != null)
                {
                    StopCoroutine(genarateRandomNumOnDice_coroutine);
                }
            }
        }

        int outPlayerToMove()
        {
            for (int i = 0; i < 4; i++)
            {
                if (!GameManager.gm.yellowPlayerPice[i].isReady)
                {
                    return i;
                }
            }
            return 0;
        }

        int CheckOutPlayer()
        {
            if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[0])
            {
                currentplayerpices = GameManager.gm.redPlayerPice; pathPointToMoveOn = pathParent.redPath;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[1])
            {
                currentplayerpices = GameManager.gm.bluePlayerPice; pathPointToMoveOn = pathParent.bluePath;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[2])
            {
                currentplayerpices = GameManager.gm.yellowPlayerPice; pathPointToMoveOn = pathParent.yellowPath;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[3])
            {
                currentplayerpices = GameManager.gm.greenPlayerPice; pathPointToMoveOn = pathParent.greenPath;
            }
            for (int i = 0; i < currentplayerpices.Length; i++)
            {
                if (currentplayerpices[i].isReady && isPathPointAvailableToMove(GameManager.gm.StepsToMove, currentplayerpices[i].numberOfStepsAlreadyMove, pathPointToMoveOn))
                {
                    return i;
                }

            }
            return -1;
        }

         bool PlayerCanNotMove()
        {
            if (outPieces > 0)
            {
                bool canNotMove = false;
                if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[0])
                {
                    currentplayerpices = GameManager.gm.redPlayerPice; pathPointToMoveOn = pathParent.redPath;
                }
                else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[1])
                {
                    currentplayerpices = GameManager.gm.bluePlayerPice; pathPointToMoveOn = pathParent.bluePath;
                }
                else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[2])
                {
                    currentplayerpices = GameManager.gm.yellowPlayerPice; pathPointToMoveOn = pathParent.yellowPath;
                }
                else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[3])
                {
                    currentplayerpices = GameManager.gm.greenPlayerPice; pathPointToMoveOn = pathParent.greenPath;
                }

                for (int i = 0; i < currentplayerpices.Length; i++)
                {
                    if (currentplayerpices[i].isReady)
                    {
                        if (isPathPointAvailableToMove(GameManager.gm.StepsToMove, currentplayerpices[i].numberOfStepsAlreadyMove, pathPointToMoveOn))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!canNotMove)
                        {
                            canNotMove = true;
                        }
                    }
                }
                if (canNotMove)
                {
                    return true;
                }
            }

            return false;
        }
        bool isPathPointAvailableToMove(int numOfStepsToMove, int numOfStepsAlredyMove, PathPoint[] pathPointsToMoveOn)
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

         void MakePlayerReadyToMove(int outPlayer)
        {
            if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[0])
            {
                outPlayerPiece = GameManager.gm.redPlayerPice[outPlayer]; pathPointToMoveOn = pathParent.redPath; GameManager.gm.redPlayerOut += 1;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[1])
            {
                outPlayerPiece = GameManager.gm.bluePlayerPice[outPlayer]; pathPointToMoveOn = pathParent.bluePath; GameManager.gm.bluePlayerOut += 1;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[2])
            {
                outPlayerPiece = GameManager.gm.yellowPlayerPice[outPlayer]; pathPointToMoveOn = pathParent.yellowPath; GameManager.gm.yellowPlayerOut += 1;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[3])
            {
                outPlayerPiece = GameManager.gm.greenPlayerPice[outPlayer]; pathPointToMoveOn = pathParent.greenPath; GameManager.gm.greenPlayerOut += 1;
            }

            outPlayerPiece.isReady = true;
            outPlayerPiece.transform.position = pathPointToMoveOn[0].transform.position;
            outPlayerPiece.numberOfStepsAlreadyMove = 1;

            outPlayerPiece.previousePath = pathPointToMoveOn[0];
            outPlayerPiece.currentPath = pathPointToMoveOn[0];
            outPlayerPiece.currentPath.AddPlayerPices(outPlayerPiece);
            //GameManager.gm.RemovePathPoint(outPlayerPiece.previousePath);
            GameManager.gm.AddPathPoint(outPlayerPiece.currentPath);

            GameManager.gm.canDiceRole = true;
            GameManager.gm.selfDice = true;
            GameManager.gm.transferDice = false;
            GameManager.gm.StepsToMove = 0;
            GameManager.gm.SelfRoal();
        }


        IEnumerator MoveStepEnum(int movePlayer)
        {
            if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[0])
            {
                outPlayerPiece = GameManager.gm.redPlayerPice[movePlayer]; pathPointToMoveOn = pathParent.redPath;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[1])
            {
                outPlayerPiece = GameManager.gm.bluePlayerPice[movePlayer]; pathPointToMoveOn = pathParent.bluePath;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[2])
            {
                outPlayerPiece = GameManager.gm.yellowPlayerPice[movePlayer]; pathPointToMoveOn = pathParent.yellowPath;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.managerRollingDice[3])
            {
                outPlayerPiece = GameManager.gm.greenPlayerPice[movePlayer]; pathPointToMoveOn = pathParent.greenPath;
            }


            GameManager.gm.transferDice = false;
            yield return new WaitForSeconds(0.25f);
            int numberofMoveStep = GameManager.gm.StepsToMove;


            for (int i = outPlayerPiece.numberOfStepsAlreadyMove; i < (outPlayerPiece.numberOfStepsAlreadyMove + numberofMoveStep); i++)
            {
                outPlayerPiece.currentPath.rescaleAndRepositionAllPlayerPices();
                if (isPathPointAvailableToMove(numberofMoveStep, outPlayerPiece.numberOfStepsAlreadyMove, pathPointToMoveOn))
                {
                    outPlayerPiece.transform.position = pathPointToMoveOn[i].transform.position;
                    yield return new WaitForSeconds(0.20f);
                }
            }

            GameManager.gm.StepsToMove = 0;
            if (isPathPointAvailableToMove(numberofMoveStep, outPlayerPiece.numberOfStepsAlreadyMove, pathPointToMoveOn))
            {

                outPlayerPiece.numberOfStepsAlreadyMove += numberofMoveStep;

                GameManager.gm.RemovePathPoint(outPlayerPiece.previousePath);
                outPlayerPiece.previousePath.RemovePlayerPieces(outPlayerPiece);
                outPlayerPiece.currentPath = pathPointToMoveOn[outPlayerPiece.numberOfStepsAlreadyMove - 1];

                if (outPlayerPiece.currentPath.AddPlayerPices(outPlayerPiece))
                {
                    if (outPlayerPiece.numberOfStepsAlreadyMove == 57)
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

                GameManager.gm.AddPathPoint(outPlayerPiece.currentPath);
                outPlayerPiece.previousePath = outPlayerPiece.currentPath;


                // GameManager.gm.StepsToMove = 0;
            }
            GameManager.gm.canPlayerMove = true;
            GameManager.gm.RollingDiceManager();

            if (moveSteps_coroutine != null)
            {
                StopCoroutine(moveSteps_coroutine);
            }
        }
    }
}

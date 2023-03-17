using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{

    public PathObjParent pathobjParent;
   public List<PlayerPiece> playerPicesList = new List<PlayerPiece>();
    PathPoint[] pathPointToMoveOn_;

    private void Start()
    {
        pathobjParent = GetComponentInParent<PathObjParent>();
    }


    public bool AddPlayerPices(PlayerPiece playerPiece)
    {
        if (this.name == "Center point")
        {
            Complete(playerPiece);
        }

        if (this.name!= "PathPoint"&&this.name!= "PathPoint (8)"&&this.name!= "PathPoint (13)"&&this.name!= "PathPoint (21)"&&this.name!= "PathPoint (26)"&&this.name!= "PathPoint (34)"&&this.name!= "PathPoint (39)"&&this.name!= "PathPoint (47)")
        {
            if (playerPicesList.Count == 1)
            {
                string prevPlayerPiceName = playerPicesList[0].name;
                string currentPlayerPiceName = playerPiece.name;
                currentPlayerPiceName = currentPlayerPiceName.Substring(0, currentPlayerPiceName.Length - 4);

                if (!prevPlayerPiceName.Contains(currentPlayerPiceName))
                {
                    playerPicesList[0].isReady = false;


                    StartCoroutine(RevertOnStart(playerPicesList[0]));

                    playerPicesList[0].numberOfStepsAlreadyMove = 0;
                    RemovePlayerPieces(playerPicesList[0]);
                    playerPicesList.Add(playerPiece);
                    return false;
                }
            }
        }
        addPlayer(playerPiece);
        return true;
    }

    
    IEnumerator RevertOnStart(PlayerPiece playerPiece)
    {
        if (playerPiece.name.Contains("Blue")) { GameManager.gm.bluePlayerOut -= 1;pathPointToMoveOn_ = pathobjParent.bluePath; }
        else if (playerPiece.name.Contains("green")) { GameManager.gm.greenPlayerOut -= 1;pathPointToMoveOn_ = pathobjParent.greenPath; }
        else if (playerPiece.name.Contains("red")) { GameManager.gm.redPlayerOut -= 1; pathPointToMoveOn_ = pathobjParent.redPath; }
        else if (playerPiece.name.Contains("Yellow")){ GameManager.gm.yellowPlayerOut -= 1; pathPointToMoveOn_ = pathobjParent.yellowPath; }

        for (int i = playerPiece.numberOfStepsAlreadyMove-1; i >= 0; i--)
        {
            playerPiece.transform.position = pathPointToMoveOn_[i].transform.position;
            yield return new WaitForSeconds(0.02f);
        }
        playerPiece.transform.position = pathobjParent.basePath[BasePonitPosition(playerPiece.name)].transform.position;

    }

    int BasePonitPosition(string name)
    {

        for (int i = 0; i < pathobjParent.basePath.Length; i++)
        {
            if (pathobjParent.basePath[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }



    void addPlayer(PlayerPiece playerPiece)
    {
        playerPicesList.Add(playerPiece);
        rescaleAndRepositionAllPlayerPices();
    }


    public void RemovePlayerPieces(PlayerPiece playerPiece)
    {
        if (playerPicesList.Contains(playerPiece))
        {
            playerPicesList.Remove(playerPiece);
            rescaleAndRepositionAllPlayerPices();
        }
    }

    public void Complete(PlayerPiece playerPiece)
    {
        if      (playerPiece.name.Contains("Blue"))  { GameManager.gm.bluePlayerCompleted += 1;   GameManager.gm.bluePlayerOut -= 1;   if (GameManager.gm.bluePlayerCompleted == 4) { ShowSelibration(); } }
        else if (playerPiece.name.Contains("green")) { GameManager.gm.greenPlayerCompleted += 1;  GameManager.gm.greenPlayerOut -= 1;  if (GameManager.gm.greenPlayerCompleted == 4) { ShowSelibration(); } }
        else if (playerPiece.name.Contains("red"))   { GameManager.gm.redPlayerCompleted += 1;    GameManager.gm.redPlayerOut -= 1;    if (GameManager.gm.redPlayerCompleted == 4) { ShowSelibration(); } }
        else if (playerPiece.name.Contains("Yellow")){ GameManager.gm.yellowPlayerCompleted += 1; GameManager.gm.yellowPlayerOut -= 1; if (GameManager.gm.yellowPlayerCompleted == 4) { ShowSelibration(); } }

    }
    public void ShowSelibration()
    {


    }


    public void rescaleAndRepositionAllPlayerPices()
    {
        int plsCount = playerPicesList.Count;
        bool isOdd = (plsCount % 2) == 0 ? false : true;

        int extent = plsCount / 2;
        int counter = 0;
        int spriteLayer =0;

        if (isOdd)
        {
            for (int i = -extent; i <= extent; i++)
            {
                playerPicesList[counter].transform.localScale = new Vector3(pathobjParent.scale[plsCount - 1], pathobjParent.scale[plsCount - 1], 1f);
                playerPicesList[counter].transform.position = new Vector3(transform.position.x + (i * pathobjParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        else
        {
            for (int i = -extent; i < extent; i++)
            {
                playerPicesList[counter].transform.localScale = new Vector3(pathobjParent.scale[plsCount - 1], pathobjParent.scale[plsCount - 1], 1f);
                playerPicesList[counter].transform.position = new Vector3(transform.position.x + (i * pathobjParent.positionDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        for (int i = 0; i < playerPicesList.Count; i++)
        {
            playerPicesList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
        
    }

}

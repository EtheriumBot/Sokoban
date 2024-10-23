using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private GridObject gridObject;
    public GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gridObject = this.GetComponent<GridObject>();
        gameManager = GameManager.reference;
    }


    // Update is called once per frame
    void Update()
    {

        if (gameManager != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                posUpdating("up");
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                posUpdating("left");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                posUpdating("down");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                posUpdating("right");
            }
        }
        else Debug.Log("gameManager is null");
    }

    void posUpdating(string dir)
    {
        //Creating the newGridPos which will be updated at the end if possible, storing oldGridPos
        Vector2Int oldGridPos = gridObject.gridPosition;
        Vector2Int newGridPos = gridObject.gridPosition;


        //Update the current grid position
        if (dir == "up")
        {
            newGridPos += new Vector2Int(0, -1);
        }
        else if (dir == "left")
        {
            newGridPos += new Vector2Int(-1, 0);
        }
        else if (dir == "down")
        {
            newGridPos += new Vector2Int(0, 1);
        }
        else if (dir == "right")
        {
            newGridPos += new Vector2Int(1, 0);
        }

        //Checking for collisions in the new grid position
        if (newGridPos.x > 0 && newGridPos.x < gameManager.gridSize.x + 1 && newGridPos.y > 0 && newGridPos.y < gameManager.gridSize.y + 1)
        {
            bool commitChange = true;

            //Walls
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

            for (int i = 0; i < walls.Length; i++)
            {
                if (walls[i].GetComponent<Wall>().gridPos == newGridPos && commitChange)
                {
                    commitChange = false;
                    //Debug.Log("Can't Move!");
                }
            }

            //Smooths
            GameObject[] smooths = GameObject.FindGameObjectsWithTag("Smooth");

            for (int i = 0; i < smooths.Length; i++)
            {
                if (smooths[i].GetComponent<Smooth>().gridPos == newGridPos && commitChange)
                {
                    commitChange = smooths[i].GetComponent<Smooth>().Moving(oldGridPos);
                }
            }

            //Stickys
            GameObject[] stickys = GameObject.FindGameObjectsWithTag("Sticky");

            for (int i = 0; i < stickys.Length; i++)
            {
                Vector2Int stickyPos = stickys[i].GetComponent<Sticky>().gridPos;
                if (stickyPos.x == oldGridPos.x-1 && stickyPos.y == oldGridPos.y || stickyPos.x == oldGridPos.x+1 && stickyPos.y == oldGridPos.y || //Left or right of where player was 
                    stickyPos.y == oldGridPos.y-1 && stickyPos.x == oldGridPos.x || stickyPos.y == oldGridPos.y+1 && stickyPos.x == oldGridPos.x) //Above or below where player was
                {
                    if (commitChange)
                    {
                        commitChange = stickys[i].GetComponent<Sticky>().Moving(oldGridPos, newGridPos);
                        //Debug.Log("Can't Move!");
                    }
                }
            }

            if (commitChange) gridObject.gridPosition = newGridPos;
            else Debug.Log("Can't Move!");
        }
    }
}

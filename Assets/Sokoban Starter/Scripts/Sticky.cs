using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    private GridObject gridObject;
    private GameManager gameManager;

    public Vector2Int gridPos;

    // Start is called before the first frame update
    void Start()
    {
        gridObject = this.GetComponent<GridObject>();
        gameManager = GameManager.reference;
        gridPos = gridObject.gridPosition;
    }

    // Update is called once per frame
    void Update()
    {
        gridPos = gridObject.gridPosition;
    }

    //Moving is called by other blocks, return true if the other block can move
    public bool Moving(Vector2Int otherOldGridPos, Vector2Int otherNewGridPos)
    {
        //Preparing variables
        Vector2Int oldGridPos = gridPos;
        Vector2Int newGridPos = gridPos;

        //Checking which dir the other block is moving in
        if (otherOldGridPos.x > otherNewGridPos.x) //Left
        {
            newGridPos.x -= 1;
        }
        else if (otherOldGridPos.x < otherNewGridPos.x) //Right
        {
            newGridPos.x += 1;
        }
        else if (otherOldGridPos.y > otherNewGridPos.y) //Up
        {
            newGridPos.y -= 1;
        }
        else if (otherOldGridPos.y < otherNewGridPos.y) //Down
        {
            newGridPos.y += 1;
        }

        //Checking for collisions in the upcoming grid position
        if (newGridPos.x > 0 && newGridPos.x < gameManager.gridSize.x + 1 && newGridPos.y > 0 && newGridPos.y < gameManager.gridSize.y + 1)
        {
            bool commitChange = true;

            //Walls
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

            for (int i = 0; i < walls.Length; i++)
            {
                if (walls[i].GetComponent<Wall>().gridPos == newGridPos && commitChange)
                {
                    if (newGridPos.x > 0 && newGridPos.x < gameManager.gridSize.x + 1 && newGridPos.y > 0 && newGridPos.y < gameManager.gridSize.y + 1)
                    {
                        commitChange = true;
                    }

                    commitChange = false;
                    Debug.Log("Can't Move!");
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
                if (stickyPos.x == oldGridPos.x - 1 && stickyPos.y == oldGridPos.y || stickyPos.x == oldGridPos.x + 1 && stickyPos.y == oldGridPos.y || //Left or right of where player was 
                    stickyPos.y == oldGridPos.y - 1 && stickyPos.x == oldGridPos.x || stickyPos.y == oldGridPos.y + 1 && stickyPos.x == oldGridPos.x) //Above or below where player was
                {
                    if (commitChange)
                    {
                        commitChange = stickys[i].GetComponent<Sticky>().Moving(oldGridPos, newGridPos);
                        //Debug.Log("Can't Move!");
                    }
                }
            }

            if (commitChange)
            {
                gridObject.gridPosition = newGridPos;
                return true;
            }
            else
            {
                //If the other object is going to be inside me, don't let it be inside me
                if (oldGridPos == otherNewGridPos) return false;
                else return true;
            }
        }
        else
        {
            return false;
        }

    }
}

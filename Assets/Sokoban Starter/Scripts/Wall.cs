using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
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
        
    }
}

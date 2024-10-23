using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager reference;

    private GridMaker gridMaker;
    public Vector2Int gridSize;
    // Start is called before the first frame update
    void Awake()
    {
        reference = this; //So that anything that references GameManager will equal this component

        gridMaker = this.GetComponent<GridMaker>();
        gridSize = new Vector2Int((int)gridMaker.dimensions.x, (int)gridMaker.dimensions.y);
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var item in grid)
        //{
        //    Debug.Log(item.ToString());
        //}
        //Debug.Log(gridSize);
    }
}

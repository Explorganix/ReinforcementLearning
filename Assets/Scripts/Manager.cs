using UnityEngine;
using System.Collections.Generic;
using System;

public class Manager : MonoBehaviour {

    public GameObject tile;
    public int worldWidth;
    public int worldHeight;
    public float tileSize;
    public int goldX;
    public int goldY;

    public List<int> wallXCoords;
    public List<int> wallYCoords;
    public List<Vector2> wallAndGoldPositions;



    public Vector2 currentPos;

    [SerializeField]
    GameObject[,] gridWorld;
	// Use this for initialization
	void Start () {
        gridWorld = new GameObject[worldHeight, worldWidth];
        tileSize = tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        GenerateGridWorld();
        currentPos = SetStartPosition(worldHeight, worldWidth);
	}



    // Update is called once per frame
    void Update () {
	
	}

    public void GenerateGridWorld()
    {
        for(int i = 0; i < worldHeight; i++)
        {
            for(int j = 0; j < worldWidth; j++)
            {
                gridWorld[j,i] = (GameObject)Instantiate(tile, new Vector3(tileSize * j, tileSize * i, 0), Quaternion.identity);
                gridWorld[j, i].GetComponent<Tile>().x = j;
                gridWorld[j, i].GetComponent<Tile>().y = i;

                
                if(j == goldX)
                {
                    if(i == goldY)
                    {
                        //gridWorld[j, i].GetComponent<Tile>().SetColor(Color.yellow);
                       // gridWorld[j, i].GetComponent<Tile>().SetReward(1);
                       // wallAndGoldPositions.Add(new Vector2(j, i));
                    }
                }  
                //Debug.Log("Tile [" + j + "," + i + "] color is :" + gridWorld[j,i].GetComponent<Tile>().GetColor().ToString());
                //Debug.Log("Reward : " + gridWorld[j, i].GetComponent<Tile>().reward);
            }
        }

        //set gold position
        gridWorld[goldX, goldY].GetComponent<Tile>().SetColor(Color.yellow);
        gridWorld[goldX, goldY].GetComponent<Tile>().SetReward(1);
        wallAndGoldPositions.Add(new Vector2(goldX, goldY));

        //set tile color and reward for walls
        for (int k = 0; k < wallXCoords.Count; k++)
        {
            int x = wallXCoords[k];
            int y = wallYCoords[k];
            gridWorld[x, y].GetComponent<Tile>().SetColor(Color.black);
            gridWorld[x, y].GetComponent<Tile>().SetReward(-1);
            wallAndGoldPositions.Add(new Vector2(x,y));
        }
    }

    private Vector2 SetStartPosition(int worldHeight, int worldWidth)
    {
        Vector2 pos = new Vector2(UnityEngine.Random.Range(0, worldHeight), UnityEngine.Random.Range(0, worldWidth));
        if (gridWorld[(int)pos.x, (int)pos.y].GetComponent<Tile>().reward == 0)
        {
            gridWorld[(int)pos.x, (int)pos.y].GetComponent<Tile>().SetColor(Color.green);
            return pos;
        }
        else
        {
            return SetStartPosition(worldHeight, worldWidth);
        }
    }
}

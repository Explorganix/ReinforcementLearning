using UnityEngine;
using System.Collections.Generic;

public class Manager : MonoBehaviour {

    public GameObject tile;
    public int worldWidth;
    public int worldHeight;
    public float tileSize;
    public int goldX;
    public int goldY;

    public List<int> wallXCoords;
    public List<int> wallYCoords;



    public Vector2 currentPos;

    [SerializeField]
    GameObject[,] gridWorld;
	// Use this for initialization
	void Start () {
        gridWorld = new GameObject[worldHeight, worldWidth];
        tileSize = tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        GenerateGridWorld();
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
                        gridWorld[j, i].GetComponent<Tile>().SetColor(Color.yellow);
                        gridWorld[j, i].GetComponent<Tile>().SetReward(1);
                    }
                }

               
              
                Debug.Log("Tile [" + j + "," + i + "] color is :" + gridWorld[j,i].GetComponent<Tile>().GetColor().ToString());
                Debug.Log("Reward : " + gridWorld[j, i].GetComponent<Tile>().reward);
            }
        }
        for (int k = 0; k < wallXCoords.Count; k++)
        {
            int x = wallXCoords[k];
            int y = wallYCoords[k];
            gridWorld[x, y].GetComponent<Tile>().SetColor(Color.black);
        }
    }
}

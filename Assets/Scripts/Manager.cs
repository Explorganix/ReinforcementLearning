using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Manager : MonoBehaviour {

    public float alpha;
    public float gamma;
    public float delta;
    public float lambda;
    public float epsilon;
    public int foundGold;
    public int episodesToDo;

    public GameObject tile;
    public int worldWidth;
    public int worldHeight;
    public float tileSize;
    public int goldX;
    public int goldY;




    public List<int> wallXCoords;
    public List<int> wallYCoords;
    public List<Vector2> wallPositions;
    public Vector2 goldPosition;

    float timer;

    public Vector2 currentPos;
    public Vector2 nextPos;

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

        
                timer += Time.deltaTime;
                if (timer > 0f)
                {
                    MovePlayer();
                    CheckForGold(currentPos);
                    timer = 0;
                }
            

	
	}

    public void MovePlayer()
    {
        nextPos = PickNextMove(currentPos);
        gridWorld[(int)currentPos.x, (int)currentPos.y].GetComponent<Tile>().SetColor(Color.white);
        currentPos = nextPos;
        gridWorld[(int)currentPos.x, (int)currentPos.y].GetComponent<Tile>().SetColor(Color.green);
    }

    private void CheckForGold(Vector2 currentPos)
    {
        if(gridWorld[(int)currentPos.x,(int)currentPos.y].GetComponent<Tile>().GetReward() == 1)
        {
            BeginNewEpisose();
        }
    }

    private void BeginNewEpisose()
    {
        foundGold++;
        gridWorld[(int)currentPos.x, (int)currentPos.y].GetComponent<Tile>().SetColor(Color.yellow);
        currentPos = SetStartPosition(worldHeight, worldWidth);
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
                //Debug.Log("Tile [" + j + "," + i + "] color is :" + gridWorld[j,i].GetComponent<Tile>().GetColor().ToString());
                //Debug.Log("Reward : " + gridWorld[j, i].GetComponent<Tile>().reward);
            }
        }

        //set gold position
        gridWorld[goldX, goldY].GetComponent<Tile>().SetColor(Color.yellow);
        gridWorld[goldX, goldY].GetComponent<Tile>().SetReward(1);
        goldPosition = new Vector2(goldX, goldY);

        //set tile color and reward for walls
        for (int k = 0; k < wallXCoords.Count; k++)
        {
            int x = wallXCoords[k];
            int y = wallYCoords[k];
            gridWorld[x, y].GetComponent<Tile>().SetColor(Color.black);
            gridWorld[x, y].GetComponent<Tile>().SetReward(-1);
            wallPositions.Add(new Vector2(x,y));
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

    public Vector2 PickNextMove(Vector2 curPos)
    {
        int rand = UnityEngine.Random.Range(0, 4);
        Vector2 pickMove = new Vector2();
        switch (rand)
        {
            case 0: pickMove = Vector2.up; break;
            case 1: pickMove = Vector2.down; break;
            case 2: pickMove = Vector2.left; break;
            case 3: pickMove = Vector2.right; break;
            default : pickMove = curPos; break;
        }
        Debug.Log("pickMove before :" + pickMove.ToString());
        pickMove += curPos;
        Debug.Log("pickMove after :" + pickMove.ToString());

        if (Enumerable.Range(0, worldWidth).Contains((int)pickMove.x) && Enumerable.Range(0, worldHeight).Contains((int)pickMove.y) && !wallPositions.Contains(pickMove))
        {
            return pickMove;
        }
        else
        {
            return PickNextMove(curPos);
        }
    }
}

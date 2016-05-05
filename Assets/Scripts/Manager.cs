using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Manager : MonoBehaviour
{

    public float alpha;
    public float gamma;
    public float delta;
    public float lambda;
    public float epsilon;
    public int foundGold;
    public int episodesToDo;
    public float stepTime;

    public GameObject tile;
    public GameObject stateTile;
    public int worldWidth;
    public int worldHeight;
    public float tileSize;
    public int goldX;
    public int goldY;
    public string nextActionString;//change to scoped variable after dev
    public string curActionString;//change to scoped variable after dev





    public List<int> wallXCoords;
    public List<int> wallYCoords;
    public List<Vector2> wallPositions;
    public Vector2 goldPosition;

    float timer;

    public Vector2 currentPos;
    public Vector2 nextPos;
    public Vector2 nextAction;
    public Vector2 lastAction;
    public int nextReward;

    [SerializeField]
    GameObject[,] gridWorld;
    GameObject[,] qTable;
    EligibilityTrace[,] eTable;
    // Use this for initialization
    void Start()
    {
        gridWorld = new GameObject[worldHeight, worldWidth];
        tileSize = tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        GenerateGridWorld();


        qTable = new GameObject[worldHeight, worldWidth];
        GenerateQTable();

        eTable = new EligibilityTrace[worldHeight, worldWidth];
        GenerateETable();

        currentPos = SetStartPosition(worldHeight, worldWidth);//initialize S
        lastAction = PickNextAction(currentPos);// initialize a
    }




    // Update is called once per frame
    void Update()
    {


        timer += Time.deltaTime;
        if (timer > stepTime)
        {
            nextPos = currentPos + lastAction;//take action a -> nextPos == S'
            nextReward = gridWorld[(int)nextPos.x, (int)nextPos.y].GetComponent<Tile>().reward;//observe the reward of S'
            nextAction = PickNextAction(nextPos);//Choose next action, A', from S' with epsilon greedy

            //FindNextAction(currentPos, nextPos);
            UpdateDelta(currentPos, nextReward, nextPos);//updating delta - determining whether the next move will hurt or help

            UpdateCurrentEligibilityValue(currentPos, lastAction); // update e table value for current spot

            UpdateEntireQTable(lastAction, alpha, delta); //update all q table values in accordance with their trace authority

            UpdateEntireETable(gamma, lambda); //decay all trace authorities



            MovePlayerSprite(currentPos, nextPos);
            UpdatePositionAndAction();
            CheckForGold(currentPos);
            timer = 0;
        }

        foreach (GameObject go in qTable)
        {
            go.GetComponent<State>().SetColor(go.GetComponent<State>().color);
        }



    }

    private void UpdateEntireETable(float gamma, float lambda)
    {

           foreach(EligibilityTrace e in eTable)
        {
            e.UpdateAllAuthorities(gamma, lambda);
        }
    }

    private void UpdateEntireQTable(Vector2 lAction, float alpha, float delta)
    {
        foreach(GameObject go in qTable)//for every state in qtable
        {
            int xCoord = go.GetComponent<State>().x;
            int yCoord = go.GetComponent<State>().y;

            foreach (Action act in qTable[xCoord, yCoord].GetComponent<State>().actions)//for every action in that state
            {               
                    act.reward += alpha * delta * eTable[xCoord, yCoord].GetActionAuthority(act.move);//update the reward           
            }
        }
    }

    private void UpdateCurrentEligibilityValue(Vector2 cPos, Vector2 lAction)
    {
        eTable[(int)cPos.x, (int)cPos.y].SetActionAuthority(lAction);
    }

    void UpdatePositionAndAction()
    {
        lastAction.x = nextAction.x;
        lastAction.y = nextAction.y;
        currentPos.x = nextPos.x;
        currentPos.y = nextPos.y;
    }

    private void UpdateDelta(Vector2 cPos, int nextStateReward, Vector2 nPos)
    {

        float newQReward = qTable[(int)nPos.x, (int)nPos.y].GetComponent<State>().GetActionReward(nextAction); // gets qtable reward from s'
        float curQReward = qTable[(int)cPos.x, (int)cPos.y].GetComponent<State>().GetActionReward(lastAction); // gets qtable reward from s'
        Debug.Log("newQReward: " + newQReward + "- curQReward:" + curQReward + "= delta:" + (nextStateReward + (gamma * newQReward) - curQReward));
        delta = nextStateReward + (gamma * newQReward) - curQReward;
    }

    //string FindNextAction(Vector2 cPos, Vector2 nPos)
    //{
    //    Vector2 actionPrime = nPos - cPos;
    //   //string actionString;
    //    if(actionPrime == Vector2.up)
    //    {
    //        nextActionString = "UP"; 
    //    }
    //    else if (actionPrime == Vector2.down)
    //    {
    //        nextActionString = "DOWN";
    //    }
    //    else if (actionPrime == Vector2.left)
    //    {
    //        nextActionString = "LEFT";
    //    }
    //    else if (actionPrime == Vector2.right)
    //    {
    //        nextActionString = "RIGHT";
    //    }
    //    return nextActionString;
    //}

    public void MovePlayerSprite(Vector2 cPos, Vector2 nPos)
    {

        gridWorld[(int)cPos.x, (int)cPos.y].GetComponent<Tile>().SetColor(Color.white);
        gridWorld[(int)nPos.x, (int)nPos.y].GetComponent<Tile>().SetColor(Color.green);
    }

    private void CheckForGold(Vector2 currentPos)
    {
        if (gridWorld[(int)currentPos.x, (int)currentPos.y].GetComponent<Tile>().GetReward() == 1)
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



    private void GenerateETable()
    {
        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                eTable[j, i] = new EligibilityTrace();
                //eTable[j, i].x = j;
                //eTable[j, i].y = i;
            }
        }
    }

    public void GenerateGridWorld()
    {
        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                gridWorld[j, i] = (GameObject)Instantiate(tile, new Vector3(tileSize * j, tileSize * i, 0), Quaternion.identity);
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
            wallPositions.Add(new Vector2(x, y));
        }
    }

    public void GenerateQTable()
    {
        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                qTable[j, i] = (GameObject)Instantiate(stateTile, new Vector3(tileSize * j, tileSize * i, -1), Quaternion.identity);
                qTable[j, i].GetComponent<State>().x = j;
                qTable[j, i].GetComponent<State>().y = i;
            }
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

    public Vector2 PickNextAction(Vector2 curPos)
    {
        Vector2 nAction = new Vector2();
        float rand = UnityEngine.Random.Range(0, 1f);
        if (rand > epsilon)
        {
            nAction = ExploreMove(curPos);
        }
        else
        {
            nAction = ExploitMove(curPos);
        }


        if (Enumerable.Range(0, worldWidth).Contains((int)(nAction.x + curPos.x)) && Enumerable.Range(0, worldHeight).Contains((int)(nAction.y + curPos.y)) && !wallPositions.Contains(nAction + curPos))
        {
            return nAction;
        }
        else
        {
            return PickNextAction(curPos);
        }
    }

    Vector2 ExploitMove(Vector2 curPos)
    {
        Vector2 nAction = new Vector2();
        nAction = qTable[(int)curPos.x, (int)curPos.y].GetComponent<State>().ExploitMove(curPos);
        return nAction;
    }

    Vector2 ExploreMove(Vector2 curPos)
    {
        int rand = UnityEngine.Random.Range(0, 4);
        Vector2 nAction = new Vector2();
        switch (rand)
        {
            case 0: nAction = Vector2.up; break;
            case 1: nAction = Vector2.down; break;
            case 2: nAction = Vector2.left; break;
            case 3: nAction = Vector2.right; break;
        }
        //Debug.Log("nAction before :" + nAction.ToString());
        //Debug.Log("nAction after :" + nAction.ToString());
        return nAction;
    }
}

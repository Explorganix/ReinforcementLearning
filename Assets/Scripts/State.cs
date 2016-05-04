using UnityEngine;
using System.Collections.Generic;

public class State : MonoBehaviour {

    public int x;
    public int y;

    public Action up;
    public Action down;
    public Action left;
    public Action right;
    public List<Action> actions;

    void Awake()
    {
        up = new Action(Vector2.up, "UP");
        down = new Action(Vector2.down, "DOWN");
        left = new Action(Vector2.left, "LEFT");
        right = new Action(Vector2.right, "RIGHT");
        actions = new List<Action>() {up, down, left, right };

    }

    void Start()
    {
        foreach(Action act in actions)
        {
            Debug.Log("id = " + act.id + ". reward = " + act.reward + ". move = " + act.move.ToString() + ".");
        }
    }

    public Vector2 ExploitMove(Vector2 curPos)
    {
        float highestReward = up.reward;
        Vector2 bestMove = Vector2.up;

        if (down.reward > highestReward)
        {
            highestReward = down.reward;
            bestMove = Vector2.down;
        }
        if (left.reward > highestReward)
        {
            highestReward = left.reward;
            bestMove = Vector2.left;
        }
           
        if (right.reward > highestReward)
        {
            highestReward = right.reward;
            bestMove = Vector2.right;
        }    

        return bestMove;
    }
}

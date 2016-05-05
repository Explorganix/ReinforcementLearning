using UnityEngine;
using System.Collections.Generic;

public class State : MonoBehaviour {

    public int x;
    public int y;
    public SpriteRenderer sprite;
    public Color color;
    public float highestActionReward;

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

    void Update()
    {
        highestActionReward = GetHighestActionReward();
        color = Color.Lerp(Color.white, Color.red, highestActionReward);
    }

    float GetHighestActionReward() //colors the state space based on the highest reward out of all the actions available in that state
    {
        float highestActionReward = up.reward;

        if (down.reward > highestActionReward)
        {
            highestActionReward = down.reward;
        }
        if (left.reward > highestActionReward)
        {
            highestActionReward = left.reward;
        }

        if (right.reward > highestActionReward)
        {
            highestActionReward = right.reward;
        }
        return highestActionReward;
    }

    public void SetColor(Color col)
    {
        Color c = col;
        sprite.material.color = c;
    }

    public float GetActionReward(Vector2 action)
    {
        float reward = 0;
        foreach(Action act in actions)
        {
            if(act.move == action)
            {
                reward = act.GetReward();
            }
        }
        return reward;
    }
}

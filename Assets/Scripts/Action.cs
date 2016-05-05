using UnityEngine;
using System.Collections;

public class Action {

    public float reward;
    public Vector2 move;
    public string id;

    public Action(Vector2 move, string id)
    {
        reward = Random.Range(.01f, .1f);//.010f, .100f);
        this.move = move;
        this.id = id;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float GetReward()
    {
        return reward;
    }

    public Vector2 GetMove()
    {
        return move;
    }

    public void SetMove(int x, int y)
    {
        move.x = x;
        move.y = y;
    }
    public void UpdateReward(float authority)
    {
        reward *= authority;
    }
}

using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public SpriteRenderer sprite;
    public Color color;
    public int x;
    public int y;
    public int reward;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Color GetColor()
    {
        return color;
    }

    public void SetColor(Color col)
    {
        color = col;
        sprite.material.color = col;
    }

    public int GetReward()
    {
        return reward;
    }

    public void SetReward(int rew)
    {
        reward = rew;
    }
}

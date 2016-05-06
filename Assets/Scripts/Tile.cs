using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public SpriteRenderer sprite;
    public Color color;
    public float colorAlpha;
    public int x;
    public int y;
    public int reward;

	// Use this for initialization
	void Start () {
        if (reward >= 1)
        {
            Color c = sprite.material.color;
            c.a = 1f;
            sprite.material.color = c;
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    public Color GetColor()
    {
        return color;
    }

    public void SetColor(Color col, float alph)
    {
        color = col;
        col.a = alph;
        sprite.material.color = col;
        //sprite.material.color.a = alph;
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

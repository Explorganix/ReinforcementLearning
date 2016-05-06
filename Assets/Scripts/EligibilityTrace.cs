using UnityEngine;
using System.Collections.Generic;

public class EligibilityTrace
{


    public float upAuthority;
    public float downAuthority;
    public float leftAuthority;
    public float rightAuthority;
    public List<float> moveAuthorties;
    //public Vector2 actionTaken;
    //public int x;
    //public int y;

    public EligibilityTrace()
    {
        upAuthority = 0f;
        downAuthority = 0f;
        leftAuthority = 0f;
        rightAuthority = 0f;
        moveAuthorties = new List<float>() { upAuthority, downAuthority, leftAuthority, rightAuthority };
    }

    public void SetActionAuthority(Vector2 move)
    {
        if (move == Vector2.up)
        {
            upAuthority = +1;//try instead of +=1
        }
        else if (move == Vector2.down)
        {
            downAuthority = +1;//try instead of +=1
        }
        else if (move == Vector2.left)
        {
            leftAuthority = +1;//try instead of +=1
        }
        else if (move == Vector2.right)
        {
            rightAuthority = +1; //try instead of +=1
        }
    }

    public float GetActionAuthority(Vector2 move)
    {
        if (move == Vector2.up)
        {
            return upAuthority;
        }
        else if (move == Vector2.down)
        {
            return downAuthority;
        }
        else if (move == Vector2.left)
        {
            return leftAuthority;
        }
        else if (move == Vector2.right)
        {
            return rightAuthority;
        }
        else
        {
            return 0;
        }

    }

    public void UpdateAllAuthorities(float gamma, float lambda)
    {
        for (int i = 0; i < moveAuthorties.Count; i++)
        {
            moveAuthorties[i] = (gamma * lambda) * moveAuthorties[i];

            //update each appropriate authority individually
            switch (i)
            {
                case 0: upAuthority = moveAuthorties[i]; break; 
                case 1: downAuthority = moveAuthorties[i]; break;
                case 2: leftAuthority = moveAuthorties[i]; break;
                case 3: rightAuthority = moveAuthorties[i]; break;
            }
        }


    }

}

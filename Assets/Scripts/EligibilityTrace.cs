using UnityEngine;
using System.Collections;

public class EligibilityTrace {


    public float authority;
    public Vector2 actionTaken;
    public int x;
    public int y;

    public EligibilityTrace()
    {
        authority = 0f;
        actionTaken = Vector2.zero;
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetActionTaken(Vector2 at)
    {
        actionTaken = at;
    }

    public Vector2 GetActionTaken()
    {
        return actionTaken;
    }

    public void SetAuthority(float auth)
    {
        authority = auth;
    }

    public float GetAuthority()
    {
        return authority;
    }
}

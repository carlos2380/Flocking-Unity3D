using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Vector3 x;
    public Vector3 v;
    public Vector3 a;
    public World world;

    void Start ()
    {
        world = FindObjectOfType<World>();
    }
	
	void Update () {
		
	}

    Vector3 cohesion()
    {
        return Vector3.zero;
    }

    Vector3 separation()
    {
        return Vector3.zero;
    }

    Vector3 alignment()
    {
        return Vector3.zero;
    }

    Vector3 combine()
    {
        return Vector3.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Vector3 x;
    public Vector3 v;
    public Vector3 a;
    public World world;
    public AgentConfig conf;
    void Start ()
    {
        world = FindObjectOfType<World>();
        conf = FindObjectOfType<AgentConfig>();

        x = transform.position;
    }
	
	void Update ()
	{
	    float t = Time.deltaTime;

	    a = combine();
	    a = Vector3.ClampMagnitude(a, conf.maxA);

	    v = v + a * t;
	    v = Vector3.ClampMagnitude(v, conf.maxV);

        x = x + v * t;
	    transform.position = x;
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

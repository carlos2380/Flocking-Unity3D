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

	    a = separation();
	    a = Vector3.ClampMagnitude(a, conf.maxA);

	    v = v + a * t;
	    v = Vector3.ClampMagnitude(v, conf.maxV);

        x = x + v * t;
	    transform.position = x;
	}

    Vector3 cohesion()
    {
        Vector3 r = new Vector3();

        var neighbours = world.getNeightbours(this, conf.Rc);

        if (neighbours.Count == 0)
        {
            return r;
        }

        //Find the center of mass of all neighbors
        foreach (var agent in neighbours)
        {
            r += agent.x;
        }

        r /= neighbours.Count;

        // a vector for our position x toward the com r
        r = r - this.x;

        Vector3.Normalize(r);
        return r;
    }

    Vector3 separation()
    {

        Vector3 r = new Vector3();

        var neighbours = world.getNeightbours(this, conf.Rs);

        if (neighbours.Count == 0)
        {
            return r;
        }

        //add the contribution neighbot towards me
        foreach (var agent in neighbours)
        {
            Vector3 towardsMe = this.x - agent.x;
            
            //if magnitude equals 0 both agents are in the same point
            if (towardsMe.magnitude > 0)
            {
                //force contribution is inversly proportional to 
                r += (towardsMe.normalized / towardsMe.magnitude);

            }
           
            return r.normalized;
        }

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

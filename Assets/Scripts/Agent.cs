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
    

    private Vector3 wanderTarget;
    private GameObject debugWanderCube;

    void Start ()
    {
        world = FindObjectOfType<World>();
        conf = FindObjectOfType<AgentConfig>();

        x = transform.position;
        v = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

        if(world.debugWonder) debugWanderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }
	
	void Update ()
	{
	    float t = Time.deltaTime;

	    a = combine();
	    a = Vector3.ClampMagnitude(a, conf.maxA);

	    v = v + a * t;
	    v = Vector3.ClampMagnitude(v, conf.maxV);

        x = x + v * t;

        wrapArround(ref x, -world.bound, world.bound);

	    if (world.debugWonder == false)
	    {
	        transform.position = x;

	        if (v.magnitude > 0)
	        {
	            transform.LookAt(x + v);
	        }
	    }

	}

    Vector3 cohesion()
    {
        Vector3 r = new Vector3();

        var neighbours = world.getNeightbours(this, conf.Rc);

        if (neighbours.Count == 0)
        {
            return r;
        }
        int countAgents = 0;
        //Find the center of mass of all neighbors
        foreach (var agent in neighbours)
        {
            if (isInFieldOfVeiw(agent.x))
            {
                r += agent.x;
                ++countAgents;
            }
        }

        r /= countAgents;

        // a vector for our position x toward the com r
        r = r - this.x;

        r = Vector3.Normalize(r);
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
            if (isInFieldOfVeiw(agent.x))
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
        }

        return Vector3.zero;
    }

    Vector3 alignment()
    {
        Vector3 r = new Vector3();

        var neighbours = world.getNeightbours(this, conf.Ra);

        if (neighbours.Count == 0)
        {
            return r;
        }

        foreach (var agent in neighbours)
        {
            if (isInFieldOfVeiw(agent.x))
            {
                //Match direction and speed == match velocity
                r += agent.v;
            }
        }

        return r.normalized;
    }

    protected virtual Vector3 combine()
    {
        Vector3 r = conf.Kc*cohesion() + conf.Ks*separation() + conf.Ka*alignment() + conf.Kw*wander() + conf.Kavoid*avoidEnemies();
        return r;
    }

    void wrapArround(ref Vector3 v, float min, float max)
    {
        v.x = wrapArroundFloat(v.x, min, max);
        v.y = wrapArroundFloat(v.y, min, max);
        v.z = wrapArroundFloat(v.z, min, max);
    }

    float wrapArroundFloat(float value, float min, float max)
    {
        if (value > max)
        {
            value = min;
        }
        else if (value < min)
        {
            value = max;
        }
        return value;
    }

    bool isInFieldOfVeiw(Vector3 stuff)
    {
        return (Vector3.Angle(this.v, stuff - this.x) <= conf.MaxFieldOfViewAngle || -Vector3.Angle(this.v, stuff - this.x) >= -conf.MaxFieldOfViewAngle);
    }


    protected Vector3 wander()
    {
        float jitter = conf.WanderJitter * Time.deltaTime;

        //add a small random vector to the target's position
        wanderTarget += new Vector3(RandomBinomial()*jitter, 0, RandomBinomial() * jitter);

        //project the vector bacj to unit circle
        wanderTarget = wanderTarget.normalized;

        //inclrease length to be the same of the radius of wander circle
        wanderTarget *= conf.WanderRadius;

        //position the target in front of the agent
        Vector3 targetInLocalSpace = wanderTarget + new Vector3(0, 0, conf.WanderDistance);

        //tranform the target from local space to world space
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);

        if (world.debugWonder) debugWanderCube.transform.position = targetInWorldSpace;

        targetInWorldSpace -= this.x;

        return targetInWorldSpace.normalized;

    }

    float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    Vector3 avoidEnemies()
    {
        Vector3 r = new Vector3();

        var enemies = world.getPredators(this, conf.Ravoid);

        if (enemies.Count == 0)
        {
            return Vector3.zero;
        }

        foreach (var enemy in enemies)
        {
                r += flee(enemy.x);
        }

        return r.normalized;
    }

    Vector3 flee(Vector3 target)
    {
        //Run the oposite direction from target
        Vector3 desiredVel = (this.x - target).normalized * conf.maxV;

        //steer velocity
        return desiredVel - v;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public Transform agentPrefab;
    public int nAgents ;
    public List<Agent> agents;

    void Start ()
	{
        agents = new List<Agent>();
	    spawn(agentPrefab, nAgents);

        agents.AddRange(FindObjectsOfType<Agent>());
	}

	void Update () {
		
	}

    void spawn(Transform prefab, int n)
    {
        for (int i = 0; i < n; ++i)
        {
            var obj = Instantiate(prefab, new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)),
                Quaternion.identity);

        }
    }

    public List<Agent> getNeightbours(Agent agent, float radious)
    {

        List<Agent> neightbours = new List<Agent>();
        foreach (var otherAgent in agents)
        {
            if (otherAgent != agent)
            {
                if (Vector3.Distance(agent.x, otherAgent.x) <= radious)
                {
                    neightbours.Add(otherAgent);
                }
            }

        }

        return neightbours;
    }
}

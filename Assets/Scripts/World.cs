using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public Transform agentPrefab;
    public int nAgents ;
    public List<Agent> agents;
    public float bound;
    public float spawnR;
    public bool debugWonder = false;
    public List<Predator> predators;

    void Start ()
	{
        agents = new List<Agent>();
	    spawn(agentPrefab, nAgents);

        agents.AddRange(FindObjectsOfType<Agent>());
	    predators.AddRange(FindObjectsOfType<Predator>());
    }

	void Update () {
		
	}

    void spawn(Transform prefab, int n)
    {
        for (int i = 0; i < n; ++i)
        {
            var obj = Instantiate(prefab, new Vector3(Random.Range(-spawnR, spawnR), 0, Random.Range(-spawnR, spawnR)),
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

    public List<Predator> getPredators(Agent agent, float radious)
    {

        List<Predator> preds = new List<Predator>();
        foreach (var predator in predators)
        {
            if (Vector3.Distance(agent.x, predator.x) <= radious)
            {
                preds.Add(predator);
            }
        }

        return preds;
    }
}

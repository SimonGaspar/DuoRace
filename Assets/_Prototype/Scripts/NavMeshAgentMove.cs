using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavMeshAgentMove : MonoBehaviour
{
    public List<Transform> goals = new List<Transform>();
    public float rotationStrength = 1f;
    public float distanceOfPoint = 30;

    private Transform goal;
    private UnityEngine.AI.NavMeshAgent agent;

    private int iter = 0;

    void Awake() 
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        goal = goals.First();
        agent.destination = goal.position;
    }

    void Update()
    {
        if (Vector3.Distance(goal.position, transform.position)<distanceOfPoint) {
            iter = ++iter == goals.Count ? 0 : iter;
            goal = goals[iter];
            agent.destination = goal.position;
        }
    }
}

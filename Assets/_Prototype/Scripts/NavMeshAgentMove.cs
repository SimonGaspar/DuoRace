using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentMove : MonoBehaviour
{
    public GameObject Goals;
    public float rotationStrength = 1f;
    public float distanceOfPoint = 150;
    public static string prefixOfPoint = "CheckPoint";
    private Transform goal;
    private List<Transform> goals = new List<Transform>();
    private UnityEngine.AI.NavMeshAgent agent;

    private int iter = 0;

    void Awake() 
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = this.transform.position;
        agent.stoppingDistance = 20;
    }

    public void Init()
    {
        goals = Goals.GetComponentsInChildren<Transform>().Where(x => x.name.Contains(prefixOfPoint)).ToList();
        goal = goals.First();
        agent.destination = goal.position;
        agent.stoppingDistance = 0;
    }

    void Update()
    {
        if (goal!=null && Vector3.Distance(goal.position, transform.position)<distanceOfPoint) {
            iter = ++iter == goals.Count ? 0 : iter;
            goal = goals[iter];
            agent.destination = goal.position;
        }

        this.GetComponent<NavMeshAgent>().speed = this.GetComponent<NavMeshAgent>().speed < 60 ? this.GetComponent<NavMeshAgent>().speed < 20 ? this.GetComponent<NavMeshAgent>().speed * Random.Range(1f, 1.5f) : this.GetComponent<NavMeshAgent>().speed * Random.Range(0.5f, 1.5f) : 40;
    }
}

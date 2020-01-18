using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class _StartRaceManager : MonoBehaviour
{
    public GameObject StartPosition;
    public GameObject Player;
    public List<GameObject> Cars = new List<GameObject>();
    public GameObject Goals;
    public string prefixOfPosition = "Position";
    private static List<GameObject> CreatedCars = new List<GameObject>();
    private Animator animator;
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        var position = StartPosition.GetComponentsInChildren<Transform>().Where(x => x.name.Contains(prefixOfPosition)).ToList();
        Player.transform.position = position[0].position;
        Player.transform.rotation = position[0].rotation;

        for (int i = 1; i < Cars.Count; i++) {
            var carObject = GameObject.Instantiate(Cars[i], position[i].position, position[i].rotation);
            var agentMoveComponent = carObject.GetComponent<NavMeshAgentMove>();
            carObject.GetComponent<Rigidbody>().isKinematic = true;
            agentMoveComponent.Goals = Goals;
            CreatedCars.Add(carObject);
        }
    }

    public static void StartAgents() {
        foreach (var car in CreatedCars)
        {
            var carObject = car.GetComponent<NavMeshAgentMove>();
            carObject.Init();
            car.GetComponent<Rigidbody>().isKinematic = false;
            carObject.enabled = true;
        }
    }
}

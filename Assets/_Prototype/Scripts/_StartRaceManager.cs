using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using VehiclePhysics;

public class _StartRaceManager : MonoBehaviour
{
    public GameObject StartPosition;
    public GameObject Player;
    public List<GameObject> Cars = new List<GameObject>();
    public GameObject Goals;
    public string prefixOfPosition = "Position";
    public static int maxLaps=1;
    public GameObject vpCamera;

    public static GameObject PlayerInstance { get; private set; }

    private static List<GameObject> CreatedCars = new List<GameObject>();
    private List<Transform> _goals = new List<Transform>();
    private Transform _goal; 
    private static Animator animator;
    private static int currentLap = 1;
    private static bool gameOver = false;
    private static int position;
    private bool startEnd;
    void Awake()
    {
        startEnd = true;
        var laps = this.GetComponentsInChildren<Text>().Where(x => x.name.Equals("CurrentLaps")).First();
        laps.text = $"Laps {currentLap}/{maxLaps}";

        animator = this.GetComponent<Animator>();
        var position = StartPosition.GetComponentsInChildren<Transform>().Where(x => x.name.Contains(prefixOfPosition)).ToList();
        PlayerInstance = GameObject.Instantiate(Player, position[0].position, position[0].rotation);
        //Player.transform.position = position[0].position;
        //Player.transform.rotation = position[0].rotation;
        PlayerInstance.GetComponent<Rigidbody>().isKinematic = true;

        for (int i = 1; i < Cars.Count; i++) {
            var carObject = GameObject.Instantiate(Cars[i], position[i].position, position[i].rotation);
            var agentMoveComponent = carObject.GetComponent<NavMeshAgentMove>();
            //carObject.GetComponent<Rigidbody>().isKinematic = true;
            agentMoveComponent.Goals = Goals;
            CreatedCars.Add(carObject);
        }

        _goals = Goals.GetComponentsInChildren<Transform>().Where(x => x.name.Contains(NavMeshAgentMove.prefixOfPoint)).ToList();
        _goal = _goals.First();

        cameraTarget= vpCamera.GetComponent<VPCameraController>();
    }

    public static void StartAgentsAndPlayer() {
        PlayerInstance.GetComponent<Rigidbody>().isKinematic = false;
        foreach (var car in CreatedCars)
        {
            var carObject = car.GetComponent<NavMeshAgentMove>();
            carObject.Init();
            //car.GetComponent<Rigidbody>().isKinematic = false;
            carObject.enabled = true;
        }
    }

    public static void StopAgentsAndPlayer()
    {
        PlayerInstance.GetComponent<Rigidbody>().isKinematic = true;
        foreach (var car in CreatedCars)
        {
            var carObject = car.GetComponent<NavMeshAgent>();
            //car.GetComponent<Rigidbody>().isKinematic = true;
            carObject.enabled = false;
        }
    }

    int iter = 0;
    float distanceOfPoint = 50f;
    VPCameraController cameraTarget;
    void Update()
    {
        if (!gameOver && Vector3.Distance(_goal.position, PlayerInstance.transform.position) < distanceOfPoint)
        {
            if (++iter == _goals.Count)
            {
                iter = 0;
                currentLap++;

                if (currentLap <= maxLaps)
                {
                    var laps = this.GetComponentsInChildren<Text>().Where(x => x.name.Equals("CurrentLaps")).First();
                    laps.text = $"Laps {currentLap}/{maxLaps}";
                }
            }
            else
            {
                _goal = _goals[iter];
            }
        }
        if (gameOver) {
            if (currentLap > maxLaps)
            {
                if (startEnd)
                {
                    var gameOverText = this.GetComponentsInChildren<Text>().Where(x => x.name.Equals("GameOver")).First();
                    gameOverText.text = gameOverText.text.Replace("{position}", $"{position}.");
                    startEnd = false;
                }
                StopAgentsAndPlayer();
            }
        }
        cameraTarget.target = PlayerInstance.transform;
    }

    public static void CheckEndGame()
    {
        if (!gameOver && currentLap > maxLaps)
        {
            gameOver = true;
            position = CreatedCars.Select(x => x.GetComponent<NavMeshAgentMove>()).Count(x => x.currentLaps > maxLaps)+1;
            StopAgentsAndPlayer();
            animator.SetBool("GameOver", gameOver);
        }
    }
}

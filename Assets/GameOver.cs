using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if (other.tag.Equals("Player"))
            _StartRaceManager.CheckEndGame();
    }
}

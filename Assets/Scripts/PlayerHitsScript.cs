using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitsScript : MonoBehaviour
{

    
    //якщо гравець вдар€Їтьс€ в перешкоду запускаЇтьс€ к≥нець гри ≥ ан≥мац≥€ струсу камери
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log(other.name);
            PlayerMoveScript.Instance.cameraAnimator.SetTrigger("Shake");
            Handheld.Vibrate();
            StartCoroutine(PlayerMoveScript.Instance.Death());
        }
    }
}

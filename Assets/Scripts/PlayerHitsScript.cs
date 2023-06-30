using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitsScript : MonoBehaviour
{
    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    //якщо гравець вдар€Їтьс€ в перешкоду запускаЇтьс€ к≥нець гри ≥ ан≥мац≥€ струсу камери
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            _body.isKinematic = true;
        }
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log(other.name);
            PlayerMoveScript.Instance.cameraAnimator.SetTrigger("Shake");
            Handheld.Vibrate();
            StartCoroutine(PlayerMoveScript.Instance.Death());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            _body.isKinematic = false;
        }
    }
}

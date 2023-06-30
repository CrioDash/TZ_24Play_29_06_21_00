using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitsScript : MonoBehaviour
{

    
    //���� ������� ���������� � ��������� ����������� ����� ��� � ������� ������ ������
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

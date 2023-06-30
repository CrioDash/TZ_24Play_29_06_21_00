using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlideControlScript : MonoBehaviour
{
    public GameObject GuidePanel;
    public GameObject Pointer;
    public Transform leftSide;
    public Transform rightSide;
    public Button restartButton;

    private bool _fingerDown;
    private RectTransform _canvasRect;
    

    private void Awake()
    {
        _canvasRect = GetComponent<RectTransform>();
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        StartCoroutine(PointerMove());
    }
    
   

    //Якщо гравець дотикається екрану і не стоїть пауза гравець переміщується по борду
    public void Update()
    {
        if (_fingerDown == false && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            _fingerDown = true;
        }
        else if (Input.touchCount == 0)
        {
            _fingerDown = false;
        }
        if(!_fingerDown)
            return;
        if (GuidePanel.activeSelf)
        {
            GuidePanel.SetActive(false);
            GameManager.Instance.ChangeState();
        }
        Vector3 pos = PlayerMoveScript.Instance.transform.position;
        pos.x = Input.GetTouch(0).position.x / (_canvasRect.rect.width-75);
        pos.x = pos.x * 4 - 2;
        if(!GameManager.Instance.IsPaused)
            PlayerMoveScript.Instance.transform.position = pos;
    }

    //Анімація руху курсору
    public IEnumerator PointerMove()
    {
        while (true)
        {
            float t = 0;
            while (t < 1)
            {
                Pointer.transform.position = Vector3.Lerp(leftSide.transform.position, rightSide.transform.position, t);
                t += Time.deltaTime;
                yield return null;
            }

            t = 0;
            while (t < 1)
            {
                Pointer.transform.position = Vector3.Lerp(rightSide.transform.position, leftSide.transform.position, t);
                t += Time.deltaTime;
                yield return null;
            }
        }
    }
    
}

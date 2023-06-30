using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideControlScript : MonoBehaviour
{
    public GameObject GuidePanel;
    public GameObject Pointer;
    public Transform leftSide;
    public Transform rightSide;
    
    private RectTransform _canvasRect;
    

    private void Awake()
    {
        _canvasRect = GetComponent<RectTransform>();
        StartCoroutine(PointerMove());
    }
    
    

    //якщо гравець дотикаЇтьс€ екрану ≥ не стоњть пауза гравець перем≥щуЇтьс€ по борду
    public void Update()
    {
        if(Input.touchCount==0) 
            return;
        //якщо п≥знавальна панель активна, то вона вимикаЇтьс€
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

    //јн≥мац≥€ руху курсору
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

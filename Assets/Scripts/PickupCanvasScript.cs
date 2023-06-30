using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class PickupCanvasScript:MonoBehaviour
    {
        public float textFloatingSpeed;
        public GameObject textPrefab;

        public static PickupCanvasScript Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void CreateText(Vector3 pos)
        {
            transform.position = pos;
            StartCoroutine(TextFade(Instantiate(textPrefab, transform).GetComponent<TextMeshProUGUI>()));
        }

        private IEnumerator TextFade(TextMeshProUGUI text)
        {
            text.transform.localPosition = Vector3.zero;
            text.color = Color.white;
            Color clr = text.color;
            float t = 0;
            while (t<1)
            {
                Vector3 pos = text.transform.localPosition;
                pos.y += textFloatingSpeed*Time.deltaTime;
                text.transform.localPosition = pos;
                text.color = Color.Lerp(clr, new Color(clr.r, clr.g, clr.b, 0), t);
                t += Time.deltaTime*2;
                yield return null;
            }

            text.color = Color.clear;
        }
        
    }
}
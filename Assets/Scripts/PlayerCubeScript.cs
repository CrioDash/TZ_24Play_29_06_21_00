using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCubeScript:MonoBehaviour
    {
        private Rigidbody _body;
        

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                _body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                transform.SetParent(null);
                Debug.Log("Removed: " + name);
                PlayerMoveScript.Instance.Cubes.Remove(this);
                PlayerMoveScript.Instance.cameraAnimator.SetTrigger("Shake");
                Handheld.Vibrate();
                if (PlayerMoveScript.Instance.Cubes.Count == 0)
                    StartCoroutine(PlayerMoveScript.Instance.Death());
            }
            if (other.CompareTag("Pickup"))
            {
                other.GetComponent<BoxCollider>().enabled = false;
                PlayerMoveScript.Instance.Jump();
                StartCoroutine(PickupAnimation(other.gameObject));
                StartCoroutine(PlayerMoveScript.Instance.CreateCube());
            }
        }
        
        public IEnumerator PickupAnimation(GameObject gm)
        {
            float t = 0;
            Vector3 startScale = gm.transform.localScale;
            while (t < 1)
            {
                gm.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                t += Time.deltaTime * 4;
                yield return null;
            }
            Destroy(gm);
        }
    }
}
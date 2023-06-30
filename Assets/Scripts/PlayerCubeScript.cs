using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCubeScript:MonoBehaviour
    {
        private Rigidbody _body;
        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _body = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //При вхожденні в фрагмент з стіною робить колайдери меншими, щоб не завдіти зайві перешкоди
            if(other.CompareTag("Wall"))
            {
                _body.isKinematic = true;
                _collider.center = Vector3.zero;
                _collider.size = Vector3.one *0.95f;
            }
            
            //При зіткненні з перешкодою кубик, що завдів перешкоду, видаляється зі списку кубиків гравця,
            //програється анімація струсу камери і, якщо кубиків не залишилось, гра закінчується
            if (other.CompareTag("Obstacle"))
            {
                _body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                transform.SetParent(null);
                PlayerMoveScript.Instance.Cubes.Remove(this);
                Vector3 pos = transform.localPosition;
                pos.z -= 0.3f;
                transform.localPosition = pos;
                PlayerMoveScript.Instance.cameraAnimator.SetTrigger("Shake");
                Handheld.Vibrate();
                if (PlayerMoveScript.Instance.Cubes.Count == 0)
                    StartCoroutine(PlayerMoveScript.Instance.Death());
            }
            
            //При входженні в підбиральний кубик програється анімація його підбору, гравець стрибає і під ним створюється новий кубик
            if (other.CompareTag("Pickup"))
            {
                other.GetComponent<BoxCollider>().enabled = false;
                PlayerMoveScript.Instance.Jump();
                StartCoroutine(PickupAnimation(other.gameObject));
                StartCoroutine(PlayerMoveScript.Instance.CreateCube());
            }
        }

        
        //При виході в фрагменту з стіною робить колайдери звичайними
        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Wall"))
            {
                _collider.center = new Vector3(0,0.05f,0);
                _collider.size = new Vector3(1, 1.1f, 1);
                _body.isKinematic = false;
            }
        }

        //Анімація зменшення підабраного кубика
        public IEnumerator PickupAnimation(GameObject gm)
        {
            float t = 0;
            Vector3 startScale = gm.transform.localScale;
            while (t < 1)
            {
                gm.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                t += Time.deltaTime * 8;
                yield return null;
            }
            Destroy(gm);
        }
    }
}
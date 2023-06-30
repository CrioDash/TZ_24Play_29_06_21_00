using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class TrackSpawnScript : MonoBehaviour
    {
        private int[,,] _obstacles= {
            {
                {1,1,1,1,1},
                {1,1,1,1,0},
                {0,1,1,1,0},
                {0,0,1,0,0},
                {0,0,1,0,0}
            },
            {
                
                {1,1,1,1,1},
                {1,1,1,1,1},
                {0,0,0,1,1},
                {0,0,0,1,1},
                {0,0,0,1,1}
            },
            {
                {1,1,1,1,1},
                {1,1,1,1,1},
                {1,1,0,0,0},
                {1,1,0,0,0},
                {1,1,0,0,0}
            },
            {
                {1,1,1,1,1},
                {0,0,0,0,0},
                {1,1,1,1,1},
                {0,0,0,0,0},
                {0,0,0,0,0}
                
            }
        };

        public int trackLength ;
        public GameObject trackPrefab;
        public GameObject pickupPrefab;
        public GameObject obstaclePrefab;

        private Transform _obstacleSpawn;

        private int _trackCount=0;

        private void Start()
        {
            Generate(3,false);
            StartCoroutine(GeneratingTracks());
        }

        void Generate(int count, bool animation)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject gm = Instantiate(trackPrefab, transform);
                GameObject mesh = gm.GetComponentInChildren<MeshRenderer>().gameObject;
                Vector3 scale = mesh.transform.localScale;
                scale.z = trackLength;
                mesh.transform.localScale = scale;
                Vector3 pos = gm.transform.localPosition;
                pos.z = _trackCount * trackLength;
                gm.transform.localPosition = pos;
                _obstacleSpawn = gm.GetComponentsInChildren<BoxCollider>()[1].transform;
                pos = _obstacleSpawn.transform.localPosition;
                pos.z = trackLength / 2;
                _obstacleSpawn.transform.localPosition = pos;
                _trackCount++;
                for (int j = 0; j < 3; j++)
                {
                    GameObject pickup = Instantiate(pickupPrefab, gm.transform);
                    pos = pickup.transform.localPosition;
                    pos.z = -trackLength / 9 + j * trackLength / 6;
                    pos.x = Mathf.Round(Random.Range(-2, 2));
                    pickup.transform.localPosition = pos;
                }

                int obsNum = Random.Range(0, _obstacles.GetLength(0));
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if(_obstacles[obsNum, j, k]==0)
                            continue;
                        GameObject obstacle = Instantiate(obstaclePrefab, gm.transform);
                        obstacle.transform.localPosition = _obstacleSpawn.transform.localPosition;
                        Vector3 obsPos = obstacle.transform.localPosition;
                        obsPos.x += k;
                        obsPos.y += j;
                        obstacle.transform.localPosition = obsPos;
                    }
                }
                if (animation)
                    StartCoroutine(AnimateTrack(gm));
            }
        }

        public IEnumerator GeneratingTracks()
        {
            while (true)
            {
                if (GameManager.Instance.IsPaused)
                {
                    yield return null;
                    continue;
                }
                yield return new WaitForSeconds(2.5f);
                Generate(1, true);
            }
        }

        public IEnumerator AnimateTrack(GameObject gm)
        {
            Vector3 posStart = gm.transform.localPosition;
            posStart.y = -60;
            gm.transform.localPosition = posStart;
            Vector3 posEnd = posStart;
            posEnd.y = 0;
            float t = 0;
            while (t < 1)
            {
                gm.transform.localPosition = Vector3.Lerp(posStart, posEnd, t);
                t += Time.deltaTime;
                yield return null;
            }
        }
    }
}
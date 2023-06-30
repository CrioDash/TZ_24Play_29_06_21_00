using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveScript : MonoBehaviour
{
    //Параметри героя
    public float speed;
    public float jumpHeight;
    
    //Об'єкт в якому містяться всі кубики гравця
    public Transform CubeParent;

    //Префаби
    public GameObject EndScreen;
    public GameObject CubeCreateParticle;
    public GameObject CubePrefab;
    public Rigidbody StickmanBody;
    public TrailRenderer Trail;
    
    //Аніматори
    public Animator playerAnimator;
    public Animator cameraAnimator;
    
    [HideInInspector]
    public List<PlayerCubeScript> Cubes;
    
    public static PlayerMoveScript Instance;

    //Список з частинами тіла героя
    private List<Rigidbody> _rigidbodies;

    private void Awake()
    {
        Instance = this;
        playerAnimator = GetComponentInChildren<Animator>();
        _rigidbodies = StickmanBody.GetComponentsInChildren<Rigidbody>().ToList();
        _rigidbodies.Remove(StickmanBody);    
    }

    void Start()
    {
        _rigidbodies.ForEach(rb => rb.isKinematic = true);
        Cubes = GetComponentsInChildren<PlayerCubeScript>().ToList();
        
    }
    
    //Якщо не стоїть павза, то гравець переміщується, а слід рухається за ним
    void Update()
    {
        
        if(GameManager.Instance.IsPaused)
           return;
       Vector3 pos = transform.position;
       pos.z += speed * Time.deltaTime;
       transform.position = pos;
       if (Trail != null)
       {
           pos = transform.position;
           pos.y = -0.45f;
           Trail.transform.position = pos;
       }
    }

    //Якщо вже не стоїть павза, то гра зупиняться, вмикається регдол і з'являється вікно кінця гри
    public IEnumerator Death()
    {
        if(GameManager.Instance.IsPaused)
            yield break;
        GameManager.Instance.ChangeState();
        _rigidbodies.ForEach(rb => rb.isKinematic = false);
        Destroy(Trail.gameObject);
        playerAnimator.enabled = false;
        EndScreen.SetActive(true);
    }

    //Стрибок гравця
    public void Jump()
    {
        playerAnimator.SetTrigger("Jump");
        Vector3 pos = StickmanBody.transform.localPosition;
        pos.y += jumpHeight;
        StickmanBody.transform.localPosition = pos;
    }

    //Анімація створення нового кубика під гравцем та програш партіклу
    public IEnumerator CreateCube()
    {
        yield return new WaitForSeconds(0.05f);
        GameObject gm = Instantiate(CubeCreateParticle, CubeParent);
        gm.transform.localPosition = new Vector3(0,(Cubes.Count-1) * 1.075f,0);
        gm = Instantiate(CubePrefab, CubeParent);
        PickupCanvasScript.Instance.CreateText(gm.transform.position);
        Cubes.Add(gm.GetComponent<PlayerCubeScript>());
        gm.transform.localScale = Vector3.one * 0.1f;
        gm.transform.localPosition = new Vector3(0,(Cubes.Count-1) * 1.075f,0);
        float t = 0;
        while (t < 1)
        {
            gm.transform.localScale = Vector3.Lerp(Vector3.one *0.1f, new Vector3(0.9f, 0.95f, 0.9f), t);
            t += Time.deltaTime * 6;
            yield return null;
        }
        gm.transform.localScale = new Vector3(0.9f, 0.95f, 0.9f);
    }
}

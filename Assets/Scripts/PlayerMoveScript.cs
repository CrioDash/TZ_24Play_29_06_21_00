using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveScript : MonoBehaviour
{
    public float speed;
    
    public Transform CubeParent;

    public GameObject CubeCreateParticle;
    public GameObject CubePrefab;
    public GameObject StickmanBody;
    
    public Animator playerAnimator;
    public Animator cameraAnimator;
    
    public List<PlayerCubeScript> Cubes;
    
    public static PlayerMoveScript Instance;

    
    private List<Rigidbody> _rigidbodies;

    private void Awake()
    {
        Instance = this;
        playerAnimator = GetComponentInChildren<Animator>();
        
        _rigidbodies = StickmanBody.GetComponentsInChildren<Rigidbody>().ToList();
    }

    void Start()
    {
        Cubes = GetComponentsInChildren<PlayerCubeScript>().ToList();
        GameManager.Instance.ChangeState();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            SceneManager.LoadScene(0);
       if(GameManager.Instance.IsPaused)
           return;
       Vector3 pos = transform.position;
       pos.z += speed * Time.deltaTime;
       transform.position = pos;
    }

    public IEnumerator Death()
    {
        _rigidbodies.ForEach(rb => rb.isKinematic = false);
        playerAnimator.enabled = false;
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.ChangeState();
    }

    public void Jump()
    {
        Debug.Log("Jump");
        Vector3 pos = StickmanBody.transform.localPosition;
        pos.y += 1f;
        StickmanBody.transform.localPosition = pos;
        playerAnimator.SetTrigger("Jump");
    }

    public IEnumerator CreateCube()
    {
        yield return new WaitForSeconds(0.05f);
        GameObject gm = Instantiate(CubeCreateParticle, CubeParent);
        gm.transform.localPosition = new Vector3(0,(Cubes.Count-1) * 1.05f,0);
        gm = Instantiate(CubePrefab, CubeParent);
        Cubes.Add(gm.GetComponent<PlayerCubeScript>());
        gm.transform.localScale = Vector3.one * 0.1f;
        gm.transform.localPosition = new Vector3(0,(Cubes.Count-1) * 1.05f,0);
        float t = 0;
        while (t < 1)
        {
            gm.transform.localScale = Vector3.Lerp(Vector3.one *0.1f, Vector3.one * 0.95f, t);
            t += Time.deltaTime * 8;
            yield return null;
        }
        gm.transform.localScale = Vector3.one*0.95f;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacked : MonoBehaviour
{
    public LayerMask enemy;
    public LayerMask vic;
    public GameObject end;

    public GameObject fail_;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        int res = 1 << other.gameObject.layer;
        if ((res & enemy.value) > 0)
        {
            fail();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int res = 1 << other.gameObject.layer;
        if ((res & vic.value) > 0)
        {
            victory();
        }
    }

    private void fail()
    {
        //Debug.Log("fail");
        end.SetActive(true);
        Time.timeScale = 0;
    }

    private void victory()
    {   
        end.SetActive(true);
        fail_.SetActive(true);
    }
}

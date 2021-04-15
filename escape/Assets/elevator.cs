using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
    public Transform pos1, pos2;
    public int Time;
    private int direction;
    // Start is called before the first frame update
    void Start()
    {
        if (Time == 0) Time = 1;
        this.gameObject.transform.position = pos1.position;
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == 1)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = (pos2.position - pos1.position) / Time;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = (pos1.position - pos2.position) / Time;
        }

        if ((gameObject.transform.position - pos1.position).magnitude < 1.0f)
        {
            direction = 1;
        }
        if ((gameObject.transform.position - pos2.position).magnitude < 1.0f)
        {
            direction = 2;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIsStill : MonoBehaviour
{
    private Rigidbody2D rb;
    public InputController inputController;

    public Transform playerTrans;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // if (rb.velocity == Vector2.zero && rb.angularVelocity == 0 && rb.bodyType == RigidbodyType2D.Dynamic)
        // {
        //     Debug.Log("Still");
        //     rb.bodyType = RigidbodyType2D.Kinematic;
        // }
        //
        // if (inputController.CheckIfToolInPlayerRange(this.gameObject.transform.position))
        // {
        //     this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        // }
        // else
        // {
        //     this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        // }
        LayerMask layerMask = ~((1 << 9) | (1 << 10));
        RaycastHit2D hit =
            Physics2D.Raycast(transform.position, playerTrans.position - this.transform.position, 1000,layerMask);
        Debug.Log(hit.collider.gameObject.name);
        if (hit.collider == playerTrans.gameObject.GetComponent<CapsuleCollider2D>() &&
            inputController.CheckIfToolInPlayerRange(this.gameObject.transform.position))
        {
            
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
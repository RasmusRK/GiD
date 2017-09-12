using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    private Rigidbody2D r2rb;
    public float timeToKill;
    void Start ()
    {
        r2rb = GetComponent<Rigidbody2D> ();

        //r2rb.velocity = (Vector2) transform.up * speed;
    }
    void Update()
    {
        Destroy(gameObject,timeToKill);
 
    }

 }

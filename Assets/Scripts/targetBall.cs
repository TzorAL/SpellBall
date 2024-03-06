using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetBall : MonoBehaviour{
    private Rigidbody2D rb;
    Vector3 lastVelocity;
    public float maxVelocity = 0.25f;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(9.8f * 40f, 9.8f * 40f));
    }

    void Update(){
        lastVelocity = rb.velocity;

        if(rb.velocity.magnitude > maxVelocity){
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }

    }

    private void OnCollisionEnter2D(Collision2D coll){
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized ,coll.contacts[0].normal);

        rb.velocity = direction * Mathf.Max(speed, 0.25f);
   }
}
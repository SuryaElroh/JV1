using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{

    float speed = 5f;

    private Rigidbody rb;
    private GameObject ball;

    // Use this for initialization
    void Start()
    {
        ball = GameObject.Find("Ball(Clone)");

        rb = GetComponent<Rigidbody>();

    }

    // Renormalize the velocity after collision
    void FixedUpdate()
    {
        if(rb != null)
        {
            if (rb.velocity.sqrMagnitude > 0f)
            {
                rb.velocity = rb.velocity.normalized * speed;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col != null)
        {
            // Increment the score of the turn's looser
            // Then destroy the ball and make it respawn to center
            switch (col.collider.name)
            {
                case "Wall1":
                    print("Wall1");
                    Network.Destroy(ball);
                    Destroy(ball);
                    GameController.Instance.CmdSetScore(2);
                    break;
                case "Wall2":
                    print("Wall2");
                    Network.Destroy(ball);
                    Destroy(ball);
                    GameController.Instance.CmdSetScore(1);
                    break;
            }
        }
    }
}

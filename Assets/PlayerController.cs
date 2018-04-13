using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    float speed = 70f;
    public static Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        float MoveZ = Input.GetAxis("Vertical");

        if(rb != null)
        {
            rb.velocity = new Vector3(0, 0, MoveZ) * speed;
        }
	}


    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public static void InitPlayerPosition()
    {
        rb.position = new Vector3(-9, 0.75f, 0);
    }
}

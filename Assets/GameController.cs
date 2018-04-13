using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : NetworkBehaviour {

    private float speed = 5f;
    private int playerCounter = 0;
    private static int Count1;
    private static int Count2;
    private static int a;
    private GameObject ball;

    public static GameController Instance;
    public GameObject ballPrefab;
    public Text Score1;
    public Text Score2;
    public Text Victory;

    // Use this for initialization
    void Start () {
        Instance = this;
        InitGame();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (a == 1)
        {
            Respawn();
        }

        // If one counter is equal to 2 => Stop the game
        if (Count1 == 5 | Count2 == 5)
        {
            StopGame();
        }

        if (NetworkServer.connections.Count == 2 & playerCounter == 0)
        {
            playerCounter += 1;
        }
        if (NetworkServer.connections.Count == 2 & playerCounter == 1)
        {
            StartCoroutine(WaitToBegin());
            playerCounter += 1;
        }

    }

    IEnumerator WaitToBegin()
    {
        yield return new WaitForSeconds(5);
        InstantiateBall();
        NetworkServer.Spawn(ball);
    }

    IEnumerator WaitRespawn()
    {
        yield return new WaitForSeconds(2);
        InstantiateBall();
        NetworkServer.Spawn(ball);
    }

    void Respawn()
    {
        //CmdSetScore();
        StartCoroutine(WaitRespawn());
        a = 0;
    }

    void InstantiateBall()
    {
        // Instantiate the ball at the define position
        ball = Instantiate(ballPrefab, new Vector3(0, 0.5f, 0), new Quaternion(0, 0, 0, 0));

        // Choose a random direction
        float px = Random.Range(0, 2) == 0 ? -1 : 1;
        float pz = Random.Range(0, 2) == 0 ? -1 : 1;

        // Set the ball's velocity
        if(ball != null)
        {
            ball.GetComponent<Rigidbody>().velocity = new Vector3(speed * px, 0f, speed * pz);
        }
    }

    // Call the Client RPC method
    [Command]
    public void CmdSetScore(int Player)
    {
        RpcSetScore(Player);
    }

    // Increment the player's score
    [ClientRpc]
    void RpcSetScore(int Player)
    {
        if (Player == 1)
        {
            Count1 += 1;
            a = 1;
        }
        if (Player == 2)
        {
            Count2 += 1;
            a = 1;
        }
        Score1.text = Count1.ToString();
        Score2.text = Count2.ToString();
    }

    void InitGame()
    {
        a = 0;

        // Initialize the Player1's score
        Count1 = 0;
        Score1.text = Count1.ToString();

        // Initialize the Player2's score
        Count2 = 0;
        Score2.text = Count2.ToString();

        // Initialize the victory message
        Victory.text = "";
    }

    // Stop the game
    void StopGame()
    {
        Destroy(ball);

        if(Count1 == 5)
        {
            Victory.text = "Player 1 is the winner !";
        }
        else
        {
            Victory.text = "Player 2 is the winner !";
        }
    }

    public void RestartGame()
    {
        InitGame();
        PlayerController.InitPlayerPosition();
        StartCoroutine(WaitToBegin());
    }
    
}

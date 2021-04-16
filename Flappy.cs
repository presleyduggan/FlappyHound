using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flappy : MonoBehaviour
{

    public GameManager GameManager;
    public int JUMP_HEIGHT;
    private Rigidbody2D rigidbody;
    public GameObject cube;
    public AudioSource bark;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !GameManager.gameovercheck)
        {
            Jump();
        }
    }

    void Jump()
    {
        rigidbody.velocity = Vector2.up * JUMP_HEIGHT;
        bark.Play();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("hit");

        /*
        if(cube.active == true)
        {
            cube.active = false;
        }
        else
        {
        cube.active = true;
        }
        */
        GameManager.GameOver();
        
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        GameManager.score++;
        GameManager.UpdateScore(GameManager.score);
    }
}

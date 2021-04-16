using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public AudioSource cry;
    public Transform Hydrant_head;
    public Transform Hydrant_side;
    public Transform Hydrant_middle;

    public Transform Hydrant_full;

    public Transform Cloud_large;
    public Transform Cloud_small1;
    public Transform Cloud_small2;

    public Transform Score_Collider;

    public GameObject GameOverPanel;

    public TMP_Text game_score_text;
    public TMP_Text score_text;

    public TMP_Text jump_text;

    public int score;

    public bool gameovercheck = false;

    public int HYDRANT_SPEED;

    public const float HYDRANT_WIDTH = 16.8f;
    public const float HEAD_HEIGHT = 34.0f;
    public const float SIDE_HEIGHT = 21.0f;
    public const float SIDE_OFFSET = -7.56f;
    public const float H_LOW = -32.7f;
    public const float H_HIGH = 10.0f;

    public const float C_MAX = 42.9f;
    public const float C_LOW = 6.9f;


    private List<Transform> Hydrants;
    private List<Transform> Clouds;

    private List<Transform> Colliders;

    private int q = 100; // where hydrant spawns
    
    public double nextHydrantTimer = 0.0;

    public double nextCloudTimer = 0.0;
    public double nextHydrantMax = 0.0;

    public double nextCloudMax = 0.0;

    public int destroyedHydrants = 0;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        Hydrants = new List<Transform>();
        Clouds = new List<Transform>();
        Colliders = new List<Transform>();
        SpawnHydrant(104,H_LOW, 40);
        SpawnClouds(102);
        //SpawnHydrant(70,H_HIGH, 40);
        //SpawnHydrant(160,H_HIGH, 40);
        score = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !gameovercheck)
        {
            Time.timeScale = 1;
            jump_text.enabled = false;
            game_score_text.enabled = true;
        }
        q = q + 10;
        MoveHydrants();
        LevelGeneration();
        MoveClouds();
        MoveColliders();
    }

    void SpawnHydrant(float x, float y, float gap)
    {
        Transform hydrant = Instantiate(Hydrant_full);
        hydrant.position = new Vector3(x,y);

        Transform score_check = Instantiate(Score_Collider);
        score_check.position = new Vector3(x,0);

        Transform hydrant2 = Instantiate(Hydrant_full);
        hydrant2.Rotate(0,180,180);
        hydrant2.position = new Vector3(x,y+gap);       
        
        Hydrants.Add(hydrant);
        Hydrants.Add(hydrant2);
        Colliders.Add(score_check);

    }

    void SpawnClouds(float x)
    {
        int cloud_type = Random.Range(1,3);
        float cloud_pos = Random.Range(C_LOW,C_MAX);
        float cloud_gap = Random.Range(40,70);

        switch(cloud_type)
        {
            case 1:
                Transform cloud = Instantiate(Cloud_large);
                cloud.position = new Vector3(x+cloud_gap,cloud_pos);
                Clouds.Add(cloud);
                break;

            case 2:
                Transform cloud2 = Instantiate(Cloud_small1);
                cloud2.position = new Vector3(x+cloud_gap,cloud_pos);
                Clouds.Add(cloud2);
                break;

            case 3:
                Transform cloud3 = Instantiate(Cloud_small2);
                cloud3.position = new Vector3(x+cloud_gap,cloud_pos);
                Clouds.Add(cloud3);
                break;
        }
    }

    void MoveHydrants()
    {
        List<Transform> hydrant_list = Hydrants;

        for(int n = 0; n < Hydrants.Count; n++)
        {
            Hydrants[n].position += new Vector3(-1,0,0) * HYDRANT_SPEED * Time.deltaTime;
        

            if(Hydrants[n].position.x < -100)
            {
                Destroy(Hydrants[n].gameObject);
                Hydrants.Remove(Hydrants[n]);
                destroyedHydrants++;
                //difficulty ramp up
                if(destroyedHydrants > 0 && destroyedHydrants%20 == 0 && nextHydrantMax > 1.9)
                {
                    nextHydrantMax = nextHydrantMax - nextHydrantMax/4;
                    //SpawnHydrant(102,H_HIGH, 40);
                }
            }



        }

        

    }


    void MoveClouds()
    {
        List<Transform> cloud_list = Clouds;

        for(int n = 0; n < Clouds.Count; n++)
        {
            Clouds[n].position += new Vector3(-1,0,0) * HYDRANT_SPEED * Time.deltaTime;
        

            if(Clouds[n].position.x < -120)
            {
                Destroy(Clouds[n].gameObject);
                Clouds.Remove(Clouds[n]);
                //difficulty ramp up
               
            }



        }
    }

    void MoveColliders()
    {
        List<Transform> collider_list = Colliders;

        for(int n = 0; n < Colliders.Count; n++)
        {
            Colliders[n].position += new Vector3(-1,0,0) * HYDRANT_SPEED * Time.deltaTime;
        

            if(Colliders[n].position.x < -120)
            {
                Destroy(Colliders[n].gameObject);
                Colliders.Remove(Colliders[n]);
                //difficulty ramp up
               
            }



        }
    }

    void LevelGeneration()
    {
        nextHydrantTimer += Time.deltaTime; 
        nextCloudTimer += Time.deltaTime;
        if(nextHydrantTimer > nextHydrantMax)
        {
            float pos = Random.Range(H_LOW,H_HIGH);
            if(destroyedHydrants > 20)
            {
                if((int)(pos)%2 == 0)
                {
                    SpawnHydrant(102,pos, 30);
                }
                else
                {
                    SpawnHydrant(102,pos, 28);
                }
            }
            else
                SpawnHydrant(102,pos, 32);

            
            nextHydrantTimer = 0.0f;
        }

        if(nextCloudTimer > nextCloudMax)
        {
            SpawnClouds(102);
            nextCloudTimer = 0.0;
        }

        
    }

    public void GameOver()
    {
        cry.Play();
        gameovercheck = true;
        Time.timeScale = 0;
        game_score_text.enabled = false;
        score_text.text = "Score: " + score;
        //score_text = the_score.text();
        GameOverPanel.active = true;
    }


    public void UpdateScore(int score)
    {
        game_score_text.text = "Score: " + score;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game was Quit");
    }

    
}

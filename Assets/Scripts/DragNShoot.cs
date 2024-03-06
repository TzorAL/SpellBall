using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragNShoot : MonoBehaviour{

    public float power = 10f;
    public Rigidbody2D rb;
    
    public Vector2 minPower;
    public Vector2 maxPower;

    TrajectoryLine tl;
    public string word = "PTYXIO";

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    Vector3 lastVelocity;
    public float maxVelocity = 10f;
    int index = 0;
    bool time_stopped = false;

    GameObject paused_text;
    GameObject win_text;

    public AudioSource bell_sound;
    public AudioSource pause_theme;
    public AudioSource main_theme;
    public AudioSource unpause_theme;

    // Start is called before the first frame update
    private void Start(){
        cam = Camera.main;
        Debug.Log("Start");
        tl = GetComponent<TrajectoryLine>();
        rb.AddForce(new Vector2(9.8f * 40f, 9.8f * 40f));

        paused_text = GameObject.Find("Paused");
        paused_text.SetActive(false);

        win_text = GameObject.Find("Win");
        win_text.SetActive(false);

        bell_sound = GameObject.Find("collision_sound").GetComponent<AudioSource>();
        pause_theme = GameObject.Find("pause_theme").GetComponent<AudioSource>();
        main_theme = GameObject.Find("main_theme").GetComponent<AudioSource>();
        unpause_theme = GameObject.Find("unpause_theme").GetComponent<AudioSource>();
        
        main_theme.Play();
    }

    // Update is called once per frame
    private void Update(){
        Debug.Log("HI");

        if (Input.GetKeyDown("space")){
            if(!time_stopped){
                PauseGame();
            }
            else{
                UnpauseGame();  
            }
        }

        if(Input.GetKey(KeyCode.Escape)){
            Debug.Log("Return to Menu");
            main_theme.Stop();
            SceneManager.LoadScene(0);
        }

        if(Input.GetMouseButtonDown(0)){
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;

            Debug.Log(startPoint);
            // Debug.Log("Update");
            slowTime();
        }

        if(Input.GetMouseButton(0)){
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;
            tl.RenderLine(startPoint, currentPoint);

        }
        
        if(Input.GetMouseButtonUp(0)){
            ResumeGame();
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;
            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            rb.AddForce(force * power, ForceMode2D.Impulse);
            tl.EndLine();
        }

        lastVelocity = rb.velocity;

        if(rb.velocity.magnitude > maxVelocity){
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }

    }

    private void OnCollisionEnter2D(Collision2D coll){
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized ,coll.contacts[0].normal);
        var coll_name = coll.gameObject.name;
        string letter = coll_name.Substring(coll_name.LastIndexOf('_') + 1);

        rb.velocity = direction * Mathf.Max(speed, 0f);
        
        if (coll_name.Contains("Letter")){
            // Debug.Log("Collided with object " + coll_name);
            // Debug.Log("Collided with letter " + letter);

            if(index <= word.Length - 1){ //till the end of word
                // Debug.Log("Checking letter " + word[index] + "of word " + word);
                if(word.Substring(0,index).Contains(letter)){ // if letter already collided (not wrong)
                    bell_sound.Play();
                }
                
                if(letter == word[index].ToString()){ //if letter is the next in line
                    bell_sound.Play();
                    coll.gameObject.GetComponent<Renderer>().material.color = Color.green; //colour ball green
                    index++;
                    // Debug.Log("Index " + index + ", World Length " + word.Length);
                    if(index == word.Length){
                        StartCoroutine(finish());
                    }
                }
                else{
                    if(!(word.Substring(0,index).Contains(letter))){ // if letter not already collided (wrong)
                        coll.gameObject.GetComponent<Renderer>().material.color = Color.red; //colour ball red
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart
                    }
                }
            }
        }
    }

    public IEnumerator finish(){
        Debug.Log("Finished! Restarting...");

        win_text.SetActive(true);
        yield return new WaitForSeconds(5);   //Wait
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart
        win_text.SetActive(false);
    }

    void slowTime() {
        Time.timeScale = 0.25f;
    }

    void PauseGame (){
        Debug.Log("ZA WARUDO!!!");
        paused_text.SetActive(true);
        time_stopped = true;
        Time.timeScale = 0;
        main_theme.Pause();
        pause_theme.Play();
    }

    void UnpauseGame(){
        Debug.Log("Time has begun to move again!!!");
        paused_text.SetActive(false);  
        time_stopped = false;
        Time.timeScale = 1;
        unpause_theme.Play();
        main_theme.Play();
    }

    void ResumeGame (){
        Time.timeScale = 1;
    }    
}

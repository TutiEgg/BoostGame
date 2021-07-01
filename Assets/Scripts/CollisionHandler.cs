using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelDelay = 1f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;
    Movements move;
    bool isTransitioning = false;
    bool collisionDisable = false;
    float boostfuel = 50f;

    void Start() {
        move =  gameObject.GetComponent<Movements>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update() {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys(){
        if(Input.GetKeyDown(KeyCode.L)){
            LoadNextLevel();
        } else if (Input.GetKeyDown(KeyCode.C)) {
            collisionDisable = !collisionDisable;
        } 

    }

    void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag){
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                //other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                
                MeshRenderer[] egg = other.gameObject.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < egg.Length; i++) {
                    if(egg[i].tag == "Mesh"){
                        egg[i].enabled = false;
                    }
                }
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                AddFuel();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    void OnCollisionEnter(Collision other) {
        
        if(isTransitioning || collisionDisable){ return; }
        
        switch (other.gameObject.tag){
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                AddFuel();

                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void AddFuel(){
        if(move.currentFuel + boostfuel > 100f){
            move.currentFuel = 100f;
        }else {
            move.currentFuel = move.currentFuel + boostfuel;
            if(move.currentFuel >= 50){
                Color color = new Color(6f/255f, 125f/255f, 21f/255f);
                move.fuelSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
            }
        }
    }
    private void StartSuccessSequence(){

        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movements>().enabled = false;
        Invoke("LoadNextLevel", levelDelay);
    }

    void StartCrashSequence(){

        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movements>().enabled = false;
        Invoke("ReloadLevel", levelDelay);
    }
    void ReloadLevel(){

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel(){

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        
    }
}

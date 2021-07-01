using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movements : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem rightParticles;
    [SerializeField] ParticleSystem leftParticles;

    [SerializeField] public Slider fuelSlider;
    [SerializeField] public float fuel = 100f;
    [SerializeField] public float fuelBurnrate = 20f;
    AudioSource audioSource;
    Rigidbody rb;
    bool isAlive;
    public float currentFuel;
    

    void Start()
    {
       rb = GetComponent<Rigidbody>(); 
       audioSource = GetComponent<AudioSource>();
       currentFuel = fuel; // maximal fuel am Anfang
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        fuelSlider.value = currentFuel / fuel;
    }

    void ProcessThrust(){
        if (Input.GetKey(KeyCode.Space))
        {
            if(currentFuel > 50){
                Color color = new Color(6f/255f, 125f/255f, 21f/255f);
                fuelSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
                StartThrusting();
                currentFuel -= fuelBurnrate * Time.deltaTime;
            }else if(currentFuel >= 20){
                Color color = new Color(255f/255f, 128f/255f, 0f/255f);
                fuelSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
                StartThrusting();
                currentFuel -= fuelBurnrate * Time.deltaTime;
            }else if(currentFuel < 20 && currentFuel > 1){
                Color color = new Color(255f/255f, 51f/255f, 0f/255f);
                fuelSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
                StartThrusting();
                currentFuel -= fuelBurnrate * Time.deltaTime;
            } else {
                StopThrusting();
            }

        }
        else
        {
            StopThrusting();
        }
    }
    void ProcessRotation(){

        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotate();
        }
    }

    
    void StartThrusting(){

        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticles.isPlaying) {
            mainEngineParticles.Play();
        }
    }

    void StopThrusting(){
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void StopRotate()
    {
        rightParticles.Stop();
        leftParticles.Stop();
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftParticles.isPlaying)
        {
            leftParticles.Play();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightParticles.isPlaying)
        {
            rightParticles.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; 
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; 
    }

}

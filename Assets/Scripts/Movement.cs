using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float sideThrust = 10f;
    Rigidbody m_Rigidbody;

    [Header("Audio")]
    [SerializeField] AudioClip thrusterClip;
    [SerializeField] AudioSource sideThrusterAudioSource;
    [SerializeField] AudioClip fuelPickupClip;
    AudioSource audioSource;

    [Header("Particles")]
    [SerializeField] ParticleSystem thrusterParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    [Header("Fuel")]
    float currentFuel;
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float refuelRate = 1f;
    [SerializeField] float combustionRate = 5f;
    [SerializeField] int fuelPickupValue;
    [SerializeField] float refuelCooldown = 2f;
    float timeUntilUseFuelAgain;
    Slider fuelMeter;

    CollisionHandler m_collisionHandler;

    void Start()
    {

        fuelMeter = FindObjectOfType<HUDManager>().GetFuelMeter();

        m_Rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        m_collisionHandler = GetComponent<CollisionHandler>();
        m_collisionHandler.SetRotationAudioSource(sideThrusterAudioSource);
        fuelMeter.maxValue = maxFuel;
        currentFuel = maxFuel;
    }


    void Update()
    {

        if ((timeUntilUseFuelAgain > Time.timeSinceLevelLoad) && currentFuel < maxFuel * 0.2f)
        {
            StopThrusting();
            ProcessRotation();
            return;
        }

        ProcessThrust();
        ProcessRotation();
        if (currentFuel <= Mathf.Epsilon)
        {
            timeUntilUseFuelAgain = Time.timeSinceLevelLoad + refuelCooldown;
        }
        Debug.Log("TimeSinceEmpty" + timeUntilUseFuelAgain);
    }


    void ProcessThrust()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();

        }
        else
        {
            StopThrusting();
        }


    }
    private void StartThrusting()
    {
        DecreaseFuel();
        if (currentFuel > 0)
        {
            m_Rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(thrusterClip);

            }
            if (!thrusterParticles.isPlaying)
            {
                thrusterParticles.Play();
            }

        }


    }

    void StopThrusting()
    {
        audioSource.Stop();
        thrusterParticles.Stop();
        Refuel();
    }

    void ProcessRotation()
    {

        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();

        }

        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();

        }
        else
        {
            StopRotatingParticles();
            sideThrusterAudioSource.Stop();
        }



    }

    private void RotateRight()
    {
        ApplyRotation(-sideThrust);
        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
            sideThrusterAudioSource.Play();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(sideThrust);
        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
            sideThrusterAudioSource.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        m_Rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        m_Rigidbody.freezeRotation = false;
    }
    public void StopRotatingParticles()
    {
        leftThrusterParticles.Stop();
        rightThrusterParticles.Stop();
    }
    public void StopMainEngineParticles()
    {
        thrusterParticles.Stop();
    }

    void DecreaseFuel()
    {
        if (currentFuel > Mathf.Epsilon)
        {
            currentFuel -= Time.deltaTime * combustionRate;
            fuelMeter.value = currentFuel;
        }
    }
    void Refuel()
    {
        if (currentFuel <= maxFuel)
        {
            currentFuel += Time.deltaTime * refuelRate;
            fuelMeter.value = currentFuel;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fuel")
        {
            Debug.Log("picking up fuel");

            other.GetComponent<AudioSource>()?.Play();
            //AudioSource.PlayClipAtPoint(fuelPickupClip, transform.position);
            //audioSource.PlayOneShot(fuelPickupClip);
            currentFuel += Mathf.Clamp(fuelPickupValue, 0, maxFuel - currentFuel);
            //Destroy(other.gameObject);
            other.GetComponent<MeshRenderer>().enabled = false;
            other.GetComponent<Collider>().enabled = false;
        }
    }


}

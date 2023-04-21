using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip successClip;

    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource m_audioSource;

    AudioSource rotationAudioSource;

    bool collisionDisable = false;
    bool isTransitioning = false;

    HUDManager m_hudManager;

    void Start()
    {
        m_hudManager = FindObjectOfType<HUDManager>();
        m_audioSource = GetComponent<AudioSource>();
        

    }
    void Update()
    {
        DisableCollisions();
    }
    void DisableCollisions()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;

            Debug.Log("cheat active?" + collisionDisable);

        }

    }

    public void SetRotationAudioSource(AudioSource wackenAudioSource)
    {
        rotationAudioSource = wackenAudioSource;
    }


    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisable) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Obstacle":
                Debug.Log("POOF");
                StartCrashSequence();
                break;
            case "Finish":
                Debug.Log("Finish");
                StartSuccessSequence();
                break;
            //add Fuel * Powerups
            default:
                Debug.Log("default");
                StartCrashSequence();
                break;

        }


    }

    void StartCrashSequence()
    {
        // todo add SFX
        // todo add particle FX
        explosionParticles.Play();
        m_audioSource.Stop();
        rotationAudioSource.Stop();
        m_audioSource.PlayOneShot(explosionClip, 0.4f);
        gameObject.GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
        isTransitioning = true;
        StopThrustingParticles();


    }
    void StartSuccessSequence()
    {
        successParticles.Play();
        // todo add SFX
        // todo add particle FX
        m_audioSource.Stop();
        rotationAudioSource.Stop();
        gameObject.GetComponent<Movement>().enabled = false;
        m_audioSource.PlayOneShot(successClip, 0.4f);
        Invoke("LoadNextLevel", levelLoadDelay);
        isTransitioning = true;
        StopThrustingParticles();


    }

    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        m_hudManager.ResetFuel();
    }
    public void LoadNextLevel()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            LoadMenu();
        }
        else
        {
            ReloadLevel();
        }
        m_hudManager.ResetFuel();

    }

    void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    void StopThrustingParticles()
    {
        {
            GetComponent<Movement>().StopRotatingParticles();
            GetComponent<Movement>().StopMainEngineParticles();
        }
    }
}

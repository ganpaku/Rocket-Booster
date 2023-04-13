using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float sideThrust = 10f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

     void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            m_Rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        }


    }
    void ProcessRotation()
    {

        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(sideThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-sideThrust);
        }

    }

    void ApplyRotation(float rotationThisFrame)
    {
        m_Rigidbody.freezeRotation = true; 
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        m_Rigidbody.freezeRotation = false;
    }
}

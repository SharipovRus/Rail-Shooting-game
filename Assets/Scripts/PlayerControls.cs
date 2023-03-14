using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
   [Header("General Setup Settings")]
   [Tooltip("How fast ship move up and down based upon player input")] 
   [SerializeField] float controlSpeed = 10f;
   [Tooltip("How far player moves horizontal")] [SerializeField] float xRange = 5f;
   [Tooltip("How far player moves vertical")][SerializeField] float yRange = 5f;
   
   [Header("Laser gun array")]
   [Tooltip("Add all player lasers here")]
   [SerializeField] GameObject[] lasers;


   [Header("Screen position based tuning")]
   [SerializeField] float posotionPitchFactor = -2f;
   [SerializeField] float positionYourFactor = 2f;
   
   [Header("Player input based tuning")]
   [SerializeField] float controlPitchFactor = -15f;
   [SerializeField] float controllRollFactor = -20f;
   float yThrow, xThrow;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }


    void ProcessRotation()
    {
        float pitchDueToPosistion = transform.localPosition.y * posotionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        
        float pitch = pitchDueToPosistion + pitchDueToControlThrow ;
        float yaw = transform.localPosition.x * positionYourFactor;
        float roll = xThrow * controllRollFactor;


        transform.localRotation = Quaternion.Euler(pitch, yaw,roll);
    }

    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clamptXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYpos = transform.localPosition.y + yOffset;
        float clamptYPos = Mathf.Clamp(rawYpos, -yRange, yRange);

        transform.localPosition = new Vector3
        (clamptXPos, clamptYPos, transform.localPosition.z);
    }


    void ProcessFiring()
    {
        if(Input.GetButton("Fire1"))
        {
            SetLasersAcrive(true);
        }
        else
        {
            SetLasersAcrive(false);
        }
    }

    void SetLasersAcrive(bool isActive)
    {
        foreach(GameObject laser in lasers)
        {
            var emissonmodule = laser.GetComponent<ParticleSystem>().emission;
            emissonmodule.enabled = isActive;
        }
    }
}
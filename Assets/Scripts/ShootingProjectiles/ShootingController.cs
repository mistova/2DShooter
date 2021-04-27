using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class which controlls player aiming and shooting
/// </summary>
public class ShootingController : MonoBehaviour
{
    [Header("GameObject/Component References")]
    [Tooltip("The projectile to be fired.")]
    public GameObject projectilePrefab = null;
    [Tooltip("The transform in the heirarchy which holds projectiles if any")]
    public Transform projectileHolder = null;

    [Header("Input")]
    [Tooltip("Whether this shooting controller is controled by the player")]
    public bool isPlayerControlled = false;

    [Header("Firing Settings")]
    [Tooltip("The minimum time between projectiles being fired.")]
    public float fireRate = 0.05f;

    [Tooltip("The maximum diference between the direction the" +
        " shooting controller is facing and the direction projectiles are launched.")]
    public float projectileSpread = 1.0f;

    // The last time this component was fired
    private float lastFired = Mathf.NegativeInfinity;

    [Header("Effects")]
    [Tooltip("The effect to create when this fires")]
    public GameObject fireEffect;

    //The input manager which manages player input
    private InputManager inputManager = null;

    [SerializeField]
    Transform [] guns;

    /// <summary>
    /// Description:
    /// Standard unity function that runs every frame
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    private void Update()
    {
        ProcessInput();
    }

    /// <summary>
    /// Description:
    /// Standard unity function that runs when the script starts
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    private void Start()
    {
        SetupInput();
    }

    /// <summary>
    /// Description:
    /// Attempts to set up input if this script is player controlled and input is not already correctly set up 
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void SetupInput()
    {
        if (isPlayerControlled)
        {
            if (inputManager == null)
            {
                inputManager = InputManager.instance;
            }
            if (inputManager == null)
            {
                Debug.LogError("Player Shooting Controller can not find an InputManager in the scene, there needs to be one in the " +
                    "scene for it to run");
            }
        }
    }

    /// <summary>
    /// Description:
    /// Reads input from the input manager
    /// Inputs:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    void ProcessInput()
    {
        if (isPlayerControlled)
        {
            if (inputManager.firePressed || inputManager.fireHeld)
            {
                Fire();
            }
        }   
    }

    /// <summary>
    /// Description:
    /// Fires a projectile if possible
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    public void Fire()
    {
        // If the cooldown is over fire a projectile
        if ((Time.timeSinceLevelLoad - lastFired) > fireRate)
        {
            // Launches a projectile
            SpawnProjectile();

            if (fireEffect != null)
            {
                if (guns.Length < 1)
                    Instantiate(fireEffect, transform.position, transform.rotation, null);
                else
                    for (int i = 0; i < guns.Length; i++)
                    {
                        //Debug.Log("I: " + i);
                        Instantiate(fireEffect, guns[i].position, guns[i].rotation, null);
                    }
            }

            // Restart the cooldown
            lastFired = Time.timeSinceLevelLoad;
        }
    }

    /// <summary>
    /// Description:
    /// Spawns a projectile and sets it up
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    public void SpawnProjectile()
    {
        // Check that the prefab is valid
        if (projectilePrefab != null)
        {
            if (guns.Length < 1)
            {
                GameObject projectileGameObject = Instantiate(projectilePrefab, transform.position, transform.rotation, null);
                Vector3 rotationEulerAngles = projectileGameObject.transform.rotation.eulerAngles;
                rotationEulerAngles.z += Random.Range(-projectileSpread, projectileSpread);
                projectileGameObject.transform.rotation = Quaternion.Euler(rotationEulerAngles);
                if (projectileHolder != null)
                {
                    projectileGameObject.transform.SetParent(projectileHolder);
                }
            }
            else
                for (int i = 0; i < guns.Length; i++)
                {
                    GameObject projectileGameObject = Instantiate(projectilePrefab, guns[i].position, guns[i].rotation, null);
                    Vector3 rotationEulerAngles = projectileGameObject.transform.rotation.eulerAngles;
                    rotationEulerAngles.z += Random.Range(-projectileSpread, projectileSpread);
                    projectileGameObject.transform.rotation = Quaternion.Euler(rotationEulerAngles);
                    if (projectileHolder != null)
                    {
                        projectileGameObject.transform.SetParent(projectileHolder);
                    }
                }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
using UnityEditor;
using System.Timers;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class Player : MonoBehaviour
{
    private CharacterController characterController;

    [Header("Aiming Settings")]
    [Space(10)]

    private FirstPersonController firstPersonController;
    [SerializeField] private RawImage aimCrosshair;

    [SerializeField] private LayerMask aimlayerMask;

    [Range(0f, 1f)]
    public float aimSensitivity;

    public Vector3 mousePosition = Vector3.zero;

    [Header("FootSteps Audio Settings")]
    [Space(10)]

    [SerializeField] private AudioClip[] footStepsAudio;

    [SerializeField] private float footStepsVolume = 0.4f;

    [SerializeField] private float minDistanceTraveled = 3f;

    private Vector3 lastPosition;
    private float totalDistance; 

    [Header("Landing Audio Settings")]
    [Space(10)]

    [SerializeField] private AudioClip landingAudio;

    [SerializeField] private float landingVolume = 0.3f;

    [SerializeField] private float fallingThreshold = 5f;

    private bool wasFalling = false;

    [Header("Blink Ability Audio Settings")]
    [Space(10)]

    [SerializeField] private AudioClip blinkAudio;

    [SerializeField] private float blinkVolume = 0.2f;

    private BlinkAbility blinkAbility;

    [Header("HealthBar Settings")]
    [Space(10)]

    [SerializeField] private HealthBar healthBar;

    private Health playerHealth;
    
    private void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();     
        characterController = GetComponent<CharacterController>();
        blinkAbility = GetComponent<BlinkAbility>();

    }
    private void Start()
    {
        playerHealth = new Health(100);
    }

    // Update is called once per frame
    void Update()
    {
        firstPersonController.ChangeSensitivity(aimSensitivity);
        Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimlayerMask))
        {
            mousePosition = raycastHit.point;
        }

        if (firstPersonController.Grounded)
        {
            float distance = Vector3.Distance(lastPosition, transform.position);
            totalDistance += distance;
            lastPosition = transform.position;
        }

        //Debug.Log($" distance : {distance}  total distance = {totalDistance} ");
        if (totalDistance > minDistanceTraveled)
        {
            if (footStepsAudio.Length > 0)
            {
                var index = Random.Range(0, footStepsAudio.Length);
                AudioSource.PlayClipAtPoint(footStepsAudio[index], transform.TransformPoint(characterController.center), landingVolume);
                totalDistance = 0f;
            }
        }

        if (/*!firstPersonController.Grounded && */characterController.velocity.y < -fallingThreshold)
        {
            // Player is falling
            wasFalling = true;
        }
        else if (firstPersonController.Grounded && wasFalling)
        {
            // Player has landed, play the landing sound
            AudioSource.PlayClipAtPoint(landingAudio, transform.TransformPoint(characterController.center), footStepsVolume);
            wasFalling = false;
        }

        if (blinkAbility.playAudio) {
            AudioSource.PlayClipAtPoint(blinkAudio,mousePosition, blinkVolume);
            blinkAbility.playAudio = false;
        }
    }

    public void PlayerDamaged(int value)
    {
        playerHealth.Damage(value);
        healthBar.UpdateHealth(playerHealth.GetMaxHealth, playerHealth.GetCurrentHealth);
        if (playerHealth.GetCurrentHealth == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}

using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    [Header("References")]
    [Space(10)]
    private FirstPersonController firstPersonController;

    private float initialMoveSpeed;



    [Header("Ability Settings")]
    [Space(10)]
    [SerializeField] private float burstSpeedValue;
    [SerializeField] private float burstSpeedTime;
    [SerializeField] private float invisibilityTime;
    public bool invisible;

    private float abilityActivationTime;


    private void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();
        initialMoveSpeed = firstPersonController.MoveSpeed;
    }

    private void OnEnable()
    {
        firstPersonController.MoveSpeed = burstSpeedValue;
        invisible = true;
        abilityActivationTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - abilityActivationTime >= burstSpeedTime) firstPersonController.MoveSpeed = initialMoveSpeed;

        if (Time.time - abilityActivationTime >= invisibilityTime)
        {
            invisible = false;
            this.enabled = false;
        }
    }
}

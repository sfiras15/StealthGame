using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private Health enemyHealth;
    [SerializeField] private EnemyAi enemyAi;

    // Start is called before the first frame update
    void Awake()
    {
        enemyHealth = new Health(100);
        enemyAi = GetComponent<EnemyAi>();
    }
    private void Update()
    {
        if (enemyHealth.GetCurrentHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void EnemyDamaged(int value)
    {
        enemyHealth.Damage(value);
        healthBar.UpdateHealth(enemyHealth.GetMaxHealth, enemyHealth.GetCurrentHealth);
        enemyAi.Chase();
    }
}

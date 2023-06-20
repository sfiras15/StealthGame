using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private Health enemyHealth;
    public int id;
    private void OnEnable()
    {
        PlayerManager.onHittingEnemy += EnemyDamaged;
    }
    private void OnDisable()
    {
        PlayerManager.onHittingEnemy -= EnemyDamaged;
    }
    // Start is called before the first frame update
    void Awake()
    {
        enemyHealth = new Health(100);
    }
    private void Update()
    {
        if (enemyHealth.GetCurrentHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void EnemyDamaged(int id)
    {
        if (this.id == id)
        {
            enemyHealth.Damage(PlayerManager.instance.gunSystem.GetGunDamage);
            healthBar.UpdateHealth(enemyHealth.GetMaxHealth, enemyHealth.GetCurrentHealth);
        }
        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int projectileDamage = 5;

    private Player player;

    
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player;
    }
    private void OnEnable()
    {
        //CancelInvoke();
        Invoke(nameof(DisableProjectile),3f);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    private void DisableProjectile()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.PlayerDamaged(projectileDamage);
            gameObject.SetActive(false);
        }
    }
}

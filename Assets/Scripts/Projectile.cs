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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.PlayerDamaged(projectileDamage);
        }
    }
}

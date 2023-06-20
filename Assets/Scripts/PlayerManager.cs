using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Player player;

    public GunSystem gunSystem;

    public static event Action<int> onHittingEnemy;


    public void HitEnemy(int id)
    {
        if (onHittingEnemy != null)
        {
            onHittingEnemy(id);
        }
    }

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gunSystem = player.GetComponentInChildren<GunSystem>();

    }
}

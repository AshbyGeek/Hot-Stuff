using UnityEngine;
using System.Collections;
using System;

public class SquirrelHealth : MonoBehaviour
{
    public ObservableEvent SquirrelDied = new ObservableEvent();

    public float maxHealth = 100;

    public float damageRate = 50;

    public AudioSource deathSound;

    private PrettyTerrain terrain;
    private TerrainTile curTile;
    private float health = 100;


    // Use this for initialization
    void Start()
    {
        health = maxHealth;

        terrain = (PrettyTerrain)FindObjectOfType(typeof(PrettyTerrain));
    }

    void Update()
    {
        (int row, int col) = terrain.tileFromPos(transform.localPosition);
        curTile = terrain.tiles[row, col];

        if (curTile.isOnFire())
        {
            health -= damageRate * Time.deltaTime;
            if (health <= 0)
            {
                killSquirrel();
            }
        }
    }

    void killSquirrel()
    {
        Destroy(this.gameObject);
        OnSquirrelDied();
        deathSound.Play();
    }

    protected void OnSquirrelDied()
    {
        SquirrelDied.SendMessage(this);
    }
}

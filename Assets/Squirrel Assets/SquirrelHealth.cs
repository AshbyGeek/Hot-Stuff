using UnityEngine;
using System.Collections;
using System;

public class SquirrelHealth : MonoBehaviour
{
    public ObservableEvent SquirrelDied = new ObservableEvent();

    private float health = 100;
    public float maxHealth = 100;

    public float damageRate = 50;

    public AudioSource deathSound;

    private EnvironmentEngine engine;
    private PrettyTerrain terrain;
    private TerrainTile curTile;


    // Use this for initialization
    void Start()
    {
        health = maxHealth;

        curTile = null;
        engine = (EnvironmentEngine)FindObjectOfType(typeof(EnvironmentEngine));
        terrain = (PrettyTerrain)FindObjectOfType(typeof(PrettyTerrain));
    }

    void Update()
    {
        Vector2 tileInd = terrain.tileFromPos(transform.localPosition);
        try
        {
            curTile = engine.mapgen.tiles[(int)tileInd.x, (int)tileInd.y];
        }
        catch
        {
            curTile = null;
        }

        if (curTile != null && curTile.isOnFire())
        {
            health -= damageRate * Time.deltaTime;
        }

        //check if the character has died
        if (health <= 0)
        {
            killSquirrel();
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

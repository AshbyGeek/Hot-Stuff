using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class TerrainTile
{
    // Relative height level of this tile. This is not in world units.
    [SerializeField]
    public float height;

    // Current heat level
    [SerializeField]
    public float heatIndex;

    // Minimum amount of heat to be considered 'on fire'
    [SerializeField]
    public float heatThresh;

    //This caps the level of the heat
    //so that we don't have ridiculously hot fires
    [SerializeField]
    public float maxHeat;

    [NonSerialized]
    public GameObject fireObj;
    
    [NonSerialized]
    public GameObject dispObj;

    [NonSerialized]
    protected float newheat = 0;
    
    [NonSerialized]
    protected bool isFlaming = false;

    public virtual void init(float height)
    {
        this.height = height;
        this.heatIndex = 0;
        this.maxHeat = 10;
    }

    public virtual void accumulateHeat(float more)
    {
        // do nothing
        return;
    }

    public virtual void updateHeat()
    {
        //do nothing
        return;
    }



    //default behaviour for tiles, flame if the heat is abrove the threshold
    //don't if its not
    //override this function if a tile needs to behave differently
    public virtual void updateFlames()
    {
        if (!this.isFlaming && this.heatIndex > this.heatThresh)
        {
            this.fireObj.SetActive(true);
            isFlaming = true;
        }
        else if (this.isFlaming && this.heatIndex < this.heatThresh)
        {
            this.fireObj.SetActive(false);
            isFlaming = false;
        }
    }

    public bool isOnFire()
    {
        return isFlaming;
    }

}

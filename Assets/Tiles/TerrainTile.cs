using UnityEngine;
using System.Collections;

public class TerrainTile : ScriptableObject
{
    public float height;
    public float heatIndex;
    public GameObject fireObj;
    public GameObject dispObj;

    public float heatThresh;

    //This caps the level of the heat
    //so that we don't have ridiculously hot fires
    public float maxHeat;

    protected float newheat = 0;
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

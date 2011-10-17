using UnityEngine;
using System.Collections;

public class TerrainTile : ScriptableObject{
	
	public enum TileType{
		mtn,
		tree,
		brush,
		plain,
		water,
		none
	}
	
	public float height;
	public float heatIndex;
	public TileType type;
	public GameObject fireObj;
	public GameObject dispObj;
	
	public float heatThresh;
	private float newheat=0;
	
	public void init(float height){
		this.height = height;
		this.heatIndex = 0;
		this.type = TerrainTile.TileType.none;
	}
	
	
	public void accumulateHeat(float more)
	{
		newheat += more;
	}
	
	public void updateHeat()
	{
		heatIndex += newheat;
		newheat=0;
		
		if (this.heatIndex > this.heatThresh){
			this.fireObj.SetActiveRecursively(true);
		}
	}
	
}

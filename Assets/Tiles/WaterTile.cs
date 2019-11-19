using UnityEngine;
using System;

[Serializable]
public class WaterTile : TerrainTile {
	//set whether or not this tile can catch on fire.
	//ie. whether or not enough gas has been dumped in it.
	[SerializeField]
	public bool isFlamable;
	
	public override void init(float height){
		base.init(height);
		this.isFlamable = false;
		
		//this tile cannot catch on fire
		this.heatThresh = 999999.0f;
	}
}

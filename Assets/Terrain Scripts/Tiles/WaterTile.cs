using UnityEngine;
using System.Collections;

public class WaterTile : TerrainTile {
	//set whether or not this tile can catch on fire.
	//ie. whether or not enough gas has been dumped in it.
	public bool isFlamable;
	
	public new void init(float height){
		base.init(height);
		this.type = TerrainTile.TileType.water;
		this.isFlamable = false;
		
		this.heatThresh = 30.0f;
	}
	
	public override void updateFlames(){
		if (isFlamable){
			base.updateFlames();
		}
	}
	
	public override void startFire (){
		if (isFlamable)
			base.startFire ();
	}
	

}

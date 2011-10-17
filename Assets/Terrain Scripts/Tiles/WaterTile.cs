using UnityEngine;
using System.Collections;

public class WaterTile : TerrainTile {
	public new void init(float height){
		base.init(height);
		this.type = TerrainTile.TileType.water;
		
		this.heatThresh = 30.0f;
	}
	//here we will do water specific stuff
}

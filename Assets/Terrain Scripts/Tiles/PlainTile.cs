using UnityEngine;
using System.Collections;

public class PlainTile : TerrainTile {
	public new void init(float height){
		base.init(height);
		this.type = TerrainTile.TileType.plain;
		
		this.heatThresh = 3.0f;
	}
	//here we will do plains specific stuff
}

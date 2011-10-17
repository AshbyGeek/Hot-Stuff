using UnityEngine;
using System.Collections;

public class BrushTile : TerrainTile {
	public new void init(float height){
		base.init(height);
		this.type = TerrainTile.TileType.brush;
		
		this.heatThresh = 2.0f;
	}
	//here we will do brush specific stuff
}

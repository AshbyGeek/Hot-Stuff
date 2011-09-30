using UnityEngine;
using System.Collections;

public class PlainTile : TerrainTile {
	public new void init(float height){
		base.init(height);
		this.type = TerrainTile.TileType.plain;
	}
	//here we will do plains specific stuff
}

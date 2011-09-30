using UnityEngine;
using System.Collections;

public class MtnTile : TerrainTile {
	public new void init(float height){
		base.init(height);
		this.type = TerrainTile.TileType.mtn;
	}
	//here we will do Mountain specific stuff
}

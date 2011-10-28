using UnityEngine;
using System.Collections;

public class MtnTile : TerrainTile {
	public new void init(float height){
		base.init(height);
		this.type = TerrainTile.TileType.mtn;
		
		this.heatThresh = 10.0f;
	}
	//here we will do Mountain specific stuff
	public override void updateFlames ()
	{
	}
}

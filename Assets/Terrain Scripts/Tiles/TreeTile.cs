using UnityEngine;
using System.Collections;

public class TreeTile : TerrainTile{
	public new void init(float height){
		base.init(height);
		this.type = TerrainTile.TileType.tree;
		
		this.heatThresh = 1.0f;
	}
	
	//here we will do tree specific stuff
	float treeFireBoost=1f;
	
	public override void accumulateHeat(float more)
	{
		if (heatThresh < maxHeat)
			newheat += more*treeFireBoost;
	}
	
	public override void updateHeat()
	{
		heatIndex += newheat;
		newheat=0;
		updateFlames();
	}
	
}

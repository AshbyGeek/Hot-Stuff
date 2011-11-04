using UnityEngine;
using System.Collections;

public class BrushTile : TerrainTile {
	public new void init(float height){
		base.init(height);
		this.type = TerrainTile.TileType.brush;
		
		this.heatThresh = 2.0f;
	}
	//here we will do brush specific stuff
	float brushFireBoost=1.5f;
	
	public override void accumulateHeat(float more)
	{
		newheat += more*brushFireBoost;
	}
	
	public override void updateHeat()
	{
		heatIndex += newheat;
		newheat=0;
		updateFlames();
	}
	
}

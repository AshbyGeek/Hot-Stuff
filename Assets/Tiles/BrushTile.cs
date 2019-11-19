using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BrushTile : TerrainTile {
	public override void init(float height){
		base.init(height);
		this.heatThresh = 2.0f;
	}

	//here we will do brush specific stuff
	[NonSerialized]
	float brushFireBoost=1.5f;
	
	public override void accumulateHeat(float more)
	{
		if (heatIndex < maxHeat){
			newheat += more*brushFireBoost;
		}
	}
	
	public override void updateHeat()
	{
		heatIndex += newheat;
		newheat=0;
		updateFlames();
	}	
}

using UnityEngine;
using System.Collections;

public class MtnTile : TerrainTile {
	public override void init(float height){
		base.init(height);
		this.heatThresh = 10.0f;
	}
	
	//here we will do Mountain specific stuff
	public override void updateFlames ()
	{
	}
}

using UnityEngine;
using System.Collections;

public class PlainTile : TerrainTile {
	public new void init(float height){
		base.init(height);
		this.heatThresh = 3.0f;
	}
}

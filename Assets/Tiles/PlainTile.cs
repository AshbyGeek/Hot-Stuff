using System;

[Serializable]
public class PlainTile : TerrainTile {
	public override void init(float height){
		base.init(height);
		this.heatThresh = 3.0f;
	}
}

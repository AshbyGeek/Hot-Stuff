using UnityEngine;
using System.Collections;

public class TerrainTile : ScriptableObject{
	
	public enum TileType{
		mtn,
		tree,
		brush,
		plain,
		water,
		none
	}
	
	public float height;
	public int heatIndex;
	public TileType type;
	
	public void init(float height){
		this.height = height;
		this.heatIndex = 0;
		this.type = TerrainTile.TileType.none;
	}
}

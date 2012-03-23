using UnityEngine;
using System.Collections;

public class EnvironmentEngine : MonoBehaviour {
	
	public TerrainGen mapgen;
	public float baseFireSpeed;
	
	// Use this for initialization
	void Start () {
		
	}
	
	
	//called when the user wants to start a fire
	public void addFire(int i, int j){
		addFire(i,j,0.1f);
	}
	public void addFire(int i, int j, float amount){
		if (i > mapgen.tiles.GetLength(0))
			return;
		if (i < 0)
			return;
		if (j > mapgen.tiles.GetLength(1))
			return;
		if (j < 0)
			return;
		addFire(mapgen.tiles[i,j], amount);
		
	}
	public void addFire(TerrainTile tile){
		addFire(tile,0.1f);
	}
	public void addFire(TerrainTile tile, float amount){
		if (tile.heatThresh >= 999999.0f)
			return;
		tile.heatIndex += amount;
	}
	
	// Update is called once per frame
	void Update () {
		float fireScale = baseFireSpeed * Time.deltaTime;
		int row;
		int col;
		
		for (row=0; row<mapgen.rows; row++)
		{
			for (col=0; col<mapgen.cols; col++)
			{
				if (mapgen.tiles[row,col] != null){
					int up=row-1;
					int down=row+1;
					int left=col-1;
					int right=col+1;
	
									
					if (up >= 0)
					{					
						if (left>=0 && mapgen.tiles[up,left]!=null)
							mapgen.tiles[row,col].accumulateHeat(fireScale * mapgen.tiles[up,left].heatIndex);
						if (right<mapgen.cols && mapgen.tiles[up,right]!=null)
							mapgen.tiles[row,col].accumulateHeat(fireScale * mapgen.tiles[up,right].heatIndex);
					}
					if (down < mapgen.rows)
					{
						if (left >= 0 && mapgen.tiles[down,left]!=null)
							mapgen.tiles[row,col].accumulateHeat(fireScale * mapgen.tiles[down,left].heatIndex);
						if (right<mapgen.cols && mapgen.tiles[down,right] != null)
							mapgen.tiles[row,col].accumulateHeat(fireScale * mapgen.tiles[down,right].heatIndex);
					}
				}
			}
		}
				
		for (row=0; row<mapgen.rows; row++)
		{
			for (col=0; col<mapgen.cols; col++)
			{
				if (mapgen.tiles[row,col] != null)
					mapgen.tiles[row,col].updateHeat();
			}
		}
		
	}
}

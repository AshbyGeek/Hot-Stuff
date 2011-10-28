using UnityEngine;
using System.Collections;

public class EnvironmentEngine : MonoBehaviour {
	
	public TerrainGen mapgen;
	public float baseFireSpeed;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void setFire(int i, int j){
		if (i > mapgen.tiles.GetLength(0))
			return;
		if (i < 0)
			return;
		if (j > mapgen.tiles.GetLength(1))
			return;
		if (j < 0)
			return;
		mapgen.tiles[i,j].heatIndex += 30;
	}
	
	// Update is called once per frame
	void Update () {
			
		float fireScale = baseFireSpeed * Time.deltaTime;
		
		for (int row=0; row<mapgen.rows; row++)
		{
			for (int col=0; col<mapgen.cols; col++)
			{
				int up=row-1;
				int down=row+1;
				int left=col-1;
				int right=col+1;

								
				if (up >= 0)
				{					
					if (left>=0)
						mapgen.tiles[row,col].accumulateHeat(fireScale * mapgen.tiles[up,left].heatIndex);
					if (right<mapgen.cols)
						mapgen.tiles[row,col].accumulateHeat(fireScale * mapgen.tiles[up,right].heatIndex);
				}
				if (down < mapgen.rows)
				{
					if (left >= 0)
						mapgen.tiles[row,col].accumulateHeat(fireScale * mapgen.tiles[down,left].heatIndex);
					if (right<mapgen.cols)
						mapgen.tiles[row,col].accumulateHeat(fireScale * mapgen.tiles[down,right].heatIndex);
				}
			}
		}
				
		for (int row=0; row<mapgen.rows; row++)
		{
			for (int col=0; col<mapgen.cols; col++)
			{
				mapgen.tiles[row,col].updateHeat();
			}
		}
		
	}
}
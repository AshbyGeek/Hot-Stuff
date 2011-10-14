using UnityEngine;
using System.Collections;

public class EnvironmentEngine : MonoBehaviour {
	
	public TerrainGen mapgen;
	public float baseFireSpeed;
	
	// Use this for initialization
	void Start () {
		
	
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

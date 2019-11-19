using UnityEngine;
using System.Collections;

public class EnvironmentEngine : MonoBehaviour
{
    private TerrainTile[,] tiles;

    public float baseFireSpeed;

	void Start()
	{
        var terrain = (PrettyTerrain)FindObjectOfType(typeof(PrettyTerrain));
        tiles = terrain.tiles;
	}

    //called when the user wants to start a fire
    public void addFire(int i, int j)
    {
        addFire(i, j, 0.1f);
    }

    public void addFire(int i, int j, float amount)
    {
        if (i > tiles.Rows() || j > tiles.Cols())
            return;
        if (i < 0 || j < 0)
            return;
        addFire(tiles[i, j], amount);
    }

    public void addFire(TerrainTile tile)
    {
        addFire(tile, 0.1f);
    }

    public void addFire(TerrainTile tile, float amount)
    {
        tile.heatIndex += amount;
    }

    // Update is called once per frame
    void Update()
    {
        float fireScale = baseFireSpeed * Time.deltaTime;
        int row;
        int col;

        for (row = 0; row < tiles.Rows(); row++)
        {
            for (col = 0; col < tiles.Cols(); col++)
            {
                int up = row - 1;
                int down = row + 1;
                int left = col - 1;
                int right = col + 1;

                if (up >= 0)
                {
                    if (left >= 0 && tiles[up, left].IsFlamable())
                        tiles[row, col].accumulateHeat(fireScale * tiles[up, left].heatIndex);
                    if (right < tiles.Cols() && tiles[up, right].IsFlamable())
                        tiles[row, col].accumulateHeat(fireScale * tiles[up, right].heatIndex);
                }
                if (down < tiles.Rows())
                {
                    if (left >= 0 && tiles[down, left].IsFlamable())
                        tiles[row, col].accumulateHeat(fireScale * tiles[down, left].heatIndex);
                    if (right < tiles.Cols() && tiles[down, right].IsFlamable())
                        tiles[row, col].accumulateHeat(fireScale * tiles[down, right].heatIndex);
                }
            }
        }

        foreach (TerrainTile curTile in tiles)
        {
            if (curTile.heatIndex > curTile.maxHeat)
            {
                curTile.heatIndex = curTile.maxHeat;
            }
            curTile.updateHeat();
        }
    }
}

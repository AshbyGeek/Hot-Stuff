using System;

public static class Extensions
{
    public static int Rows(this TerrainTile[,] tiles) => tiles.GetUpperBound(0) - tiles.GetLowerBound(0);

    public static int Cols(this TerrainTile[,] tiles) => tiles.GetUpperBound(1) - tiles.GetLowerBound(1);

    public static bool IsFlamable(this TerrainTile tile) => tile.heatThresh <= tile.maxHeat;
}
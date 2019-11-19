using System;

public interface ITerrainSource
{
    TerrainTile[,] GenerateTiles();
    int GetWaterHeight();
}
using System;

public class PreviousMapGenerator : ITerrainSource
{
    public string _filePath = @"Z:\tmp\squirrelsMap.json";

    public int GetWaterHeight() => 17; //TODO: HACK: actually store and return this value

    public TerrainTile[,] GenerateTiles()
    {
        return _filePath.FromJson();
    }
}
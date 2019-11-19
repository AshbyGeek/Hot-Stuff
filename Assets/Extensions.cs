using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class Extensions
{
    public static int Rows(this TerrainTile[,] tiles) => tiles.GetLength(0);

    public static int Cols(this TerrainTile[,] tiles) => tiles.GetLength(1);

    public static bool IsFlamable(this TerrainTile tile) => tile.heatThresh <= tile.maxHeat;

    public static string ToJson(this TerrainTile[,] tiles) 
    {
        var settings = new JsonSerializerSettings(){
            TypeNameHandling = TypeNameHandling.All,
        };
        return JsonConvert.SerializeObject(tiles, settings);
    }

    public static TerrainTile[,] FromJson(this string json)
    {
        var settings = new JsonSerializerSettings(){
            TypeNameHandling = TypeNameHandling.All,
        };
        return JsonConvert.DeserializeObject<TerrainTile[,]>(json, settings);
    }
}
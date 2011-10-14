using UnityEngine;
using System.Collections;

public class PrettyTerrainTile : MonoBehaviour {
	TerrainTile tile; //link to the logic tile
	bool isFlaming;
	
	public void init(TerrainTile tile, Vector3 loc, Mesh mesh){
		this.tile = tile;
		isFlaming = false;
		
		Transform trans = GetComponent<Transform>();
		trans.position = loc;
		
		//add the appropriate mesh
		if (mesh != null){
			MeshFilter meshFilter = this.gameObject.AddComponent<MeshFilter>();
			meshFilter.mesh = mesh;
			MeshRenderer meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
		}
		
		//add a flame base
		//todo
		
	}
	
	/*void update(){
		if (tile.heatIndex > tile.heatThreshold){
			if (!isFlaming){
				this.addFlames();
			}else{
				this.updateFlames();
			}
		}
	}
	
	void addFlames(){
		//todo	
	}
	
	void updateFlames(){
		//todo
	}*/
}

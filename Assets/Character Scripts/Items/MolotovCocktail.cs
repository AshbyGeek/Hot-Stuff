using UnityEngine;
using System.Collections;

public class MolotovCocktail : Item {
	public float throwSpeed = 25.0f;
	public float heatAdded = 4.0f;
	
	void Awake() {
		base.defPos = new Vector3(1.0f,1.206215f,0.148f);
		//base.identifier = "Molotov Cocktail";
	}
	
	public override void useItem() {
		base.useItem();
		Quaternion rot = Camera.mainCamera.transform.rotation;
		Vector3 throwDir = rot * Vector3.forward;
		rigidbody.AddForce(throwDir * throwSpeed,ForceMode.Impulse);
		rigidbody.AddTorque(1,0,0,ForceMode.Impulse);
	}
	
	//use the collider to start a fire
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag != "Items"){
			PrettyTerrain terrain = (PrettyTerrain) FindObjectOfType(typeof(PrettyTerrain));
			Vector2 tileInd = terrain.tileFromPos(transform.localPosition);
			try{
				EnvironmentEngine engine = (EnvironmentEngine) FindObjectOfType(typeof(EnvironmentEngine));
				TerrainTile curTile = engine.mapgen.tiles[(int)tileInd.x,(int)tileInd.y];
				engine.addFire(curTile, heatAdded);
			}catch{
				return;
			}
		}
	}
}

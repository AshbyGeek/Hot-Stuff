using UnityEngine;
using System.Collections;

public class MolotovCocktail : MonoBehaviour {
	
	public float throwSpeed = 25.0f;
	
	private bool thrown = false;
	
	// Update is called once per frame
	void Update () {
		if(!thrown && Input.GetButton("Fire1")){
			transform.parent = null;
			rigidbody.constraints = RigidbodyConstraints.None;
			Vector3 throwDir = (Vector3.forward + Vector3.up);
			throwDir.Normalize();
			rigidbody.AddRelativeForce(throwDir * throwSpeed,ForceMode.Impulse);
			rigidbody.AddTorque(1,0,0,ForceMode.Impulse);
			thrown = true;
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		PrettyTerrain terrain = (PrettyTerrain) FindObjectOfType(typeof(PrettyTerrain));
		Vector2 tileInd = terrain.tileFromPos(transform.localPosition);
		try{
			EnvironmentEngine engine = (EnvironmentEngine) FindObjectOfType(typeof(EnvironmentEngine));
			TerrainTile curTile = engine.mapgen.tiles[(int)tileInd.x,(int)tileInd.y];
			engine.addFire(curTile, 4.0f);
		}catch{
			return;
		}
		
	}
}

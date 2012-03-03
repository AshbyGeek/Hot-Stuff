using UnityEngine;
using System.Collections;

public class SquirrelHealth : MonoBehaviour {
	public float health = 100;
	public float maxHealth = 100;
	
	public AudioSource deathSound;
	
	private EnvironmentEngine engine;
	private PrettyTerrain terrain;
	private TerrainTile curTile;
	
	// Use this for initialization
	void Start () {
		health = maxHealth;
		
		curTile = null;
		engine = (EnvironmentEngine) FindObjectOfType(typeof(EnvironmentEngine));
		terrain = (PrettyTerrain) FindObjectOfType(typeof(PrettyTerrain));
	}
	
	void Update(){
		Vector2 tileInd = terrain.tileFromPos(transform.localPosition);
		try{
			curTile = engine.mapgen.tiles[(int)tileInd.x,(int)tileInd.y];
		}catch{
			curTile = null;
		}
	}
	
	void FixedUpdate(){
		if (curTile != null){
			if (curTile.isOnFire()){
				health -= curTile.heatIndex*Time.deltaTime/10.0f;
			}
		}
		
		//check if the character has died
		if (health < 0)
		{
			killSquirrel();
		}
	}
	
	void killSquirrel(){
		Destroy(this.gameObject);
		GameObject tmp = GameObject.Find("SquirrelLauncher");
		tmp.GetComponent<SquirrelSpawner>().numSquirrels -= 1;
		deathSound.Play();
	}
}

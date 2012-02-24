using UnityEngine;
using System.Collections;

public class CharacterHealth : MonoBehaviour {
	
	public float health;
	public float maxHealth;
	
    public Vector2 bar_pos = new Vector2(20,40);
    public Vector2 bar_size = new Vector2(60,20);
    public Texture2D bar_emptyTex;
    public Texture2D bar_fullTex;
	
	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	void Update(){
		CharacterInteractions tmp = GetComponent<CharacterInteractions>();
		
		if (tmp.curTile != null){
			if (tmp.curTile.isOnFire()){
				health -= tmp.curTile.heatIndex/10.0f;
			}
		}
		
		//check if the character has died
		if (health < 0)
		{
			tmp.killCharacter();
		}
	}
	
	void OnGUI(){
       //draw the background:
       GUI.BeginGroup(new Rect(bar_pos.x, bar_pos.y, bar_size.x, bar_size.y));
         GUI.Box(new Rect(0,0, bar_size.x, bar_size.y), bar_emptyTex);

         //draw the filled-in part:
         GUI.BeginGroup(new Rect(0,0, bar_size.x * health/maxHealth, bar_size.y));
          GUI.Box(new Rect(0,0, bar_size.x, bar_size.y), bar_fullTex);
         GUI.EndGroup();
       GUI.EndGroup();
	}
}

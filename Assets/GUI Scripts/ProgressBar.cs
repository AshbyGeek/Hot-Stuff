using UnityEngine;
using System.Collections;

//borrowed from internet forums
public class ProgressBar: ScriptableObject {
    public float barDisplay; //current progress
    public Vector2 pos = new Vector2(20,40);
    public Vector2 size = new Vector2(60,20);
    public Texture2D emptyTex;
    public Texture2D fullTex;

    public void draw() {
       //draw the background:
       GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
         GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);

         //draw the filled-in part:
         GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
          GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
         GUI.EndGroup();
       GUI.EndGroup();
    }
}

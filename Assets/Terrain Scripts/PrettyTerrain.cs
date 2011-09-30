using UnityEngine;
using System.Collections;

public class PrettyTerrain : MonoBehaviour {
	public TerrainGen terrain;
	
	//
	public GameObject boundsObj;
	public GameObject water_obj;
	public GameObject tree_obj;
	public GameObject plains_obj;
	public GameObject mountain_obj;
	public GameObject brush_obj;
		
	public Vector3 size = new Vector3(80,40,80);
	
	void generateMesh(){
		float halfX = (terrain.rows - 1)/2.0f;
		float halfZ = (terrain.cols - 1)/2.0f;
				
		//make a mesh object with each vertex representing a point in the log matrix
		Vector3[] newVertices = new Vector3[terrain.rows * terrain.cols];
		Vector2[] newUV = new Vector2[newVertices.Length];
		
		Vector2 uvScale = new Vector2 (1.0f / (terrain.rows - 1), 1.0f / (terrain.cols - 1));
		Vector3 sizeScale = new Vector3 (size.x / (terrain.rows - 1), size.y / 100.0f, size.z / (terrain.rows - 1));
		
		int rects = (terrain.rows - 1) * (terrain.cols - 1);
		int[] newTriangles = new int[rects*2*3];
		
		int curTri = 0;
		for (int i = 0; i < terrain.rows - 1; i++){
			for (int j = 0; j < terrain.cols - 1; j++){
				newTriangles[curTri] 		= (i)	*terrain.cols + (j+1);
				newTriangles[curTri + 1] 	= (i+1)	*terrain.cols +  j;
				newTriangles[curTri + 2]	=  i	*terrain.cols +  j;
				
				newTriangles[curTri + 3] 	= (i)	*terrain.cols + (j+1);
				newTriangles[curTri + 4] 	= (i+1)	*terrain.cols + (j+1);
				newTriangles[curTri + 5] 	= (i+1)	*terrain.cols +  j;
				
				curTri += 6;
			}
		}
		
		for (int i = 0; i < terrain.rows; i++){
			for (int j = 0; j < terrain.cols; j++){
				float x = (i - halfX)*sizeScale.x;
				float y = terrain.tiles[i, j].height*sizeScale.y;
				float z = (j - halfZ)*sizeScale.z;
				
				newVertices[i*terrain.cols + j] = new Vector3(x,y,z);
				newUV[i*terrain.cols + j] = Vector2.Scale(new Vector2(x,z),uvScale);
				
				
				GameObject tmpObj;
				
				switch(terrain.tiles[i,j].type)
				{
				case TerrainTile.TileType.mtn:
					tmpObj = mountain_obj;
					break;
				case TerrainTile.TileType.tree:
					tmpObj = tree_obj;
					break;
				case TerrainTile.TileType.brush:
					tmpObj = brush_obj;
					break;
				case TerrainTile.TileType.plain:
					tmpObj = plains_obj;
					break;
				case TerrainTile.TileType.water:
					tmpObj = water_obj;
					break;
				default:
					tmpObj = null;
					break;
				}
				if (tmpObj != null)
				{
					Vector3 newLoc = Vector3.Scale(new Vector3(x,y,z),this.transform.localScale);
					GameObject tmp = (GameObject) Instantiate(tmpObj, newLoc,Quaternion.identity);
					tmp.transform.localScale = Vector3.Scale(new Vector3(.5f,.5f,.5f),tmp.transform.localScale);
				}
			}
		}
		
		Mesh mesh = new Mesh();
		mesh.Clear();
		mesh.vertices = newVertices;
		mesh.triangles = newTriangles;
		mesh.uv = newUV;
		mesh.RecalculateNormals();
		mesh.Optimize(); //Higher load time, faster draw speed
		//terrainObjMeshFilter.mesh = tmp;
		//mesh.bounds = new Bounds(Vector3.zero,
		//                         new Vector3(halfX*sizeScale.x,sizeScale.y,halfZ*sizeScale.z));
		
		this.GetComponent<MeshFilter>().mesh = mesh;
		this.GetComponent<MeshCollider>().sharedMesh = mesh;
		mesh.RecalculateBounds();
	}
	
	void Start () {
		terrain.init(terrain.rows,terrain.cols);

		terrain.randomize(terrain.Mtn_Spawns,terrain.Water_Spawns,terrain.Tree_Spawns);
		terrain.smooth(.00000001, 3000, 0.9);
		terrain.toTerrainTiles();
		
		terrain.startFinished = true;
		this.generateMesh();
	}
}

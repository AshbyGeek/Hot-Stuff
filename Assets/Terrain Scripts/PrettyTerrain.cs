using UnityEngine;
using System.Collections;

public class PrettyTerrain : MonoBehaviour {
	public TerrainGen terrain;
	
	public GameObject water_obj;
	public GameObject tree_obj;
	public GameObject plains_obj;
	public GameObject mountain_obj;
	public GameObject brush_obj;
	public GameObject FireObj;
		
	public Vector3 treeScale = new Vector3(1.0f,1.0f,1.0f);
	
	private float halfX;
	private float halfZ;
	private Vector3 sizeScale;
	
	void generateMesh(){
		//adjust the scales of the input objects
		tree_obj.transform.localScale = Vector3.Scale(treeScale,tree_obj.transform.localScale);
				
		halfX = (terrain.rows - 1)/2.0f;
		halfZ = (terrain.cols - 1)/2.0f;
				
		//make a mesh object with each vertex representing a point in the log matrix
		Vector3[] newVertices = new Vector3[terrain.rows * terrain.cols];
		Vector2[] newUV = new Vector2[newVertices.Length];
		
		Vector2 uvScale = new Vector2 (1.0f / (terrain.rows - 1), 1.0f / (terrain.cols - 1));
		sizeScale = new Vector3 (1.0f / (terrain.rows - 1), 1.0f / 100.0f, 1.0f / (terrain.cols - 1));
		
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
				float z = (j - halfZ)*sizeScale.z;
				float y;
				if (terrain.tiles[i,j] == null)
					y = terrain.array[i,j]*sizeScale.y;
				else
					y = terrain.tiles[i, j].height*sizeScale.y;
				
				newVertices[i*terrain.cols + j] = new Vector3(x,y,z);
				newUV[i*terrain.cols + j] = Vector2.Scale(new Vector2(x,z),uvScale);
					
				if (terrain.tiles[i,j] != null){
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
					default:
						tmpObj = null;
						break;
					}
					
					GameObject tileObj = new GameObject("tile[" + i + "," + j + "]");
					tileObj.transform.position = Vector3.Scale(new Vector3(x,y,z),
					                                                gameObject.transform.localScale);
					tileObj.transform.parent = this.gameObject.transform;
					tileObj.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
									
					//add the mesh object as a child of the tile
					if (tmpObj != null){
						GameObject tmpDispObj = (GameObject) Instantiate(tmpObj);
						tmpDispObj.transform.parent = tileObj.transform;
						
						Vector3 tmpVect = Random.insideUnitSphere;
						Vector2 tmpXY = new Vector2(tmpVect.x,tmpVect.z);
						tmpVect -= Vector3.up*(tmpXY.magnitude*2.5f);
						tmpDispObj.transform.localPosition = Vector3.Scale(Random.insideUnitSphere - Vector3.up*2,sizeScale*2.0f);
						
						tmpDispObj.name = "DispObj";
						terrain.tiles[i,j].dispObj = tmpDispObj;
					}
					
					//add the fire particle system as a child of the tile
					GameObject tmpFireObj = (GameObject) Instantiate(FireObj);
					tmpFireObj.transform.parent = tileObj.transform;
					tmpFireObj.transform.localPosition = Vector3.zero;
					tmpFireObj.name = "FireObj";
					tmpFireObj.SetActiveRecursively(false);
					terrain.tiles[i,j].fireObj = tmpFireObj;
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
		
		this.GetComponent<MeshFilter>().mesh = mesh;
		this.GetComponent<MeshCollider>().sharedMesh = mesh;
		mesh.RecalculateBounds();
		
		//set the height of the water
		float tmpY = terrain.plain_thresh*gameObject.transform.localScale.y*sizeScale.y;
		water_obj.transform.position = new Vector3(0,tmpY,0);
	}
	
	public Vector2 tileFromPos(Vector3 pos){
		float i = pos.x/sizeScale.x;
		float j = pos.z/sizeScale.z;
		
		i /= gameObject.transform.localScale.x;
		j /= gameObject.transform.localScale.z;
		
		i += halfX;
		j += halfZ;
		
		i = Mathf.Round(i);
		j = Mathf.Round(j);
		
		return new Vector2(i,j);
	}
	
	void Start () {
		terrain.init(terrain.rows,terrain.cols);
		
		//lock the corners to very deep values
		terrain.lockPoint(0,0,-100);
		terrain.lockPoint(0,terrain.cols-1,-100);
		terrain.lockPoint(terrain.rows-1,0,-100);
		terrain.lockPoint(terrain.rows-1,terrain.cols-1,-100);
		
		terrain.lockEdges(4,-50,terrain.water_thresh-10);
		terrain.smooth(.01, 15, 1.9);
		terrain.setEdgeLocks(true, terrain.water_thresh - 5);
		terrain.lockBelowVal(terrain.water_thresh);
		
		terrain.lockWaterBorders(20,3);
		terrain.lockInnerTerrain(5,10,3,5,2,5);
		
		//terrain.randomize(terrain.Mtn_Spawns,terrain.Water_Spawns,terrain.Tree_Spawns);
		terrain.smooth(.00001, 2000, .9);
		
		terrain.setAllLocks(false);
		terrain.smooth(.01,5,.2);
		
		terrain.toTerrainTiles();
		
		this.generateMesh();
		
		//terrain.tiles[0,0].heatIndex = 30.0f;
	}
}

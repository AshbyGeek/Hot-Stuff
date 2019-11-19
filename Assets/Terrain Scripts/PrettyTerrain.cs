using UnityEngine;
using System.Collections;
using System.Linq;

public class PrettyTerrain : MonoBehaviour
{
    public GameObject water_obj;
    public GameObject tree_obj;
    public GameObject plains_obj;
    public GameObject mountain_obj;
    public GameObject brush_obj;
    public GameObject FireObj;

    public Vector3 treeScale = new Vector3(1.0f, 1.0f, 1.0f);

    private float halfX;
    private float halfZ;
    private Vector3 sizeScale;

    [HideInInspector]
    public TerrainTile[,] tiles;

    [HideInInspector]
    public int waterHeight;

    private void computeScaleInfo(int rows, int cols)
    {
        halfX = (rows - 1) / 2.0f;
        halfZ = (cols - 1) / 2.0f;
        sizeScale = new Vector3(1.0f / (rows - 1), 1.0f / 100.0f, 1.0f / (cols - 1));
    }

    private Mesh GenerateMesh(TerrainTile[,] tiles)
    {
        //adjust the scales of the input objects
        tree_obj.transform.localScale = Vector3.Scale(treeScale, tree_obj.transform.localScale);

        //make a mesh object with each vertex representing a point in the log matrix
        Vector3[] newVertices = new Vector3[tiles.Rows() * tiles.Cols()];
        Vector2[] newUV = new Vector2[newVertices.Length];

        Vector2 uvScale = new Vector2(1.0f / (tiles.Rows() - 1), 1.0f / (tiles.Cols() - 1));
        sizeScale = new Vector3(1.0f / (tiles.Rows() - 1), 1.0f / 100.0f, 1.0f / (tiles.Cols() - 1));

        int rects = (tiles.Rows() - 1) * (tiles.Cols() - 1);
        int[] newTriangles = new int[rects * 2 * 3];

        int curTri = 0;
        for (int i = 0; i < tiles.Rows() - 1; i++)
        {
            for (int j = 0; j < tiles.Cols() - 1; j++)
            {
                newTriangles[curTri] = (i) * tiles.Cols() + (j + 1);
                newTriangles[curTri + 1] = (i + 1) * tiles.Cols() + j;
                newTriangles[curTri + 2] = i * tiles.Cols() + j;

                newTriangles[curTri + 3] = (i) * tiles.Cols() + (j + 1);
                newTriangles[curTri + 4] = (i + 1) * tiles.Cols() + (j + 1);
                newTriangles[curTri + 5] = (i + 1) * tiles.Cols() + j;

                curTri += 6;
            }
        }

        for (int i = 0; i < tiles.Rows(); i++)
        {
            for (int j = 0; j < tiles.Cols(); j++)
            {
                Vector3 pos = GetTileCenter(i, j, tiles[i, j].height);

                newVertices[i * tiles.Cols() + j] = pos;
                newUV[i * tiles.Cols() + j] = Vector2.Scale(new Vector2(pos.x, pos.z), uvScale);
            }
        }

        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.uv = newUV;
        mesh.RecalculateNormals();
        mesh.Optimize(); //Higher load time, faster draw speed
        return mesh;
    }

    private Vector3 GetTileCenter(int row, int col, float tileHeight)
    {
        float x = (row - halfX) * sizeScale.x;
        float z = (col - halfZ) * sizeScale.z;
        float y = tileHeight * sizeScale.y;

        return new Vector3(x, y, z);
    }

    private void AddDetailsToTiles(TerrainTile[,] tiles)
    {
        for (int i = 0; i < tiles.Rows(); i++)
        {
            for (int j = 0; j < tiles.Cols(); j++)
            {
                TerrainTile curTile = tiles[i, j];
                var pos = GetTileCenter(i, j, curTile.height);

                GameObject tileObj = new GameObject("tile[" + i + "," + j + "]");
                tileObj.transform.position = Vector3.Scale(pos, gameObject.transform.localScale);
                tileObj.transform.parent = this.gameObject.transform;
                tileObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                //add the mesh object as a child of the tile
                GameObject tmpObj;
                if (curTile is MtnTile)
                {
                    tmpObj = mountain_obj;
                }
                else if (curTile is TreeTile)
                {
                    tmpObj = tree_obj;
                }
                else if (curTile is BrushTile)
                {
                    tmpObj = brush_obj;
                }
                else if (curTile is PlainTile)
                {
                    tmpObj = plains_obj;
                }
                else
                {
                    tmpObj = null;
                }
                if (tmpObj != null)
                {
                    GameObject tmpDispObj = (GameObject)Instantiate(tmpObj);
                    tmpDispObj.transform.parent = tileObj.transform;

                    Vector3 tmpVect = Random.insideUnitSphere;
                    Vector2 tmpXY = new Vector2(tmpVect.x, tmpVect.z);
                    tmpVect -= Vector3.up * (tmpXY.magnitude * 2.5f);
                    tmpDispObj.transform.localPosition = Vector3.Scale(Random.insideUnitSphere - Vector3.up * 2, sizeScale * 2.0f);

                    tmpDispObj.name = "DispObj";
                    tiles[i, j].dispObj = tmpDispObj;
                }

                //add the fire particle system as a child of the tile
                GameObject tmpFireObj = (GameObject)Instantiate(FireObj);
                tmpFireObj.transform.parent = tileObj.transform;
                tmpFireObj.transform.localPosition = Vector3.zero;
                tmpFireObj.name = "FireObj";
                tmpFireObj.SetActive(false);
                tiles[i, j].fireObj = tmpFireObj;
            }
        }
    }

    public (int row, int col) tileFromPos(Vector3 pos)
    {
        float i = pos.x / sizeScale.x;
        float j = pos.z / sizeScale.z;

        i /= gameObject.transform.localScale.x;
        j /= gameObject.transform.localScale.z;

        i += halfX;
        j += halfZ;

        int row = (int)Mathf.Round(i);
        int col = (int)Mathf.Round(j);

        if (row < 0) row = 0;
        if (row > tiles.Rows()) row = tiles.Rows();
        if (col < 0) col = 0;
        if (col > tiles.Cols()) col = tiles.Cols();

        return (row, col);
    }

    void Awake()
    {
        ITerrainSource source = new RandomTerrainGenerator();

        tiles = source.GenerateTiles();
        computeScaleInfo(tiles.Rows(), tiles.Cols());
        AddDetailsToTiles(tiles);

        var mesh = GenerateMesh(tiles);
        this.GetComponent<MeshFilter>().mesh = mesh;
        this.GetComponent<MeshCollider>().sharedMesh = mesh;
        mesh.RecalculateBounds();

        //set the height of the water
        float tmpY = source.GetWaterHeight() * gameObject.transform.localScale.y * sizeScale.y;
        water_obj.transform.position = new Vector3(0, tmpY, 0);
    }

    private interface ITerrainSource
    {
        TerrainTile[,] GenerateTiles();
        int GetWaterHeight();
    }

    private class RandomTerrainGenerator : ITerrainSource
    {
        public TerrainGen terrain = new TerrainGen()
        {
            max_val = 100,
            mtn_thresh = 75,
            tree_thresh = 46,
            brush_thresh = 25,
            plain_thresh = 17,
            water_thresh = 7,
            mtn_chain = 4,
            water_chain = 2,
            cols = 40,
            rows = 40,
            Mtn_Spawns = 3,
            Water_Spawns = 3,
            Tree_Spawns = 3,
        };

        public int GetWaterHeight() => terrain.plain_thresh;

        public TerrainTile[,] GenerateTiles()
        {
            terrain.init(terrain.rows, terrain.cols);

            //lock the corners to very deep values
            terrain.lockPoint(0, 0, -100);
            terrain.lockPoint(0, terrain.cols - 1, -100);
            terrain.lockPoint(terrain.rows - 1, 0, -100);
            terrain.lockPoint(terrain.rows - 1, terrain.cols - 1, -100);

            terrain.lockEdges(4, -50, terrain.water_thresh - 10);
            terrain.smooth(.01, 15, 1.9);
            terrain.setEdgeLocks(true, terrain.water_thresh - 5);
            terrain.lockBelowVal(terrain.water_thresh);

            terrain.lockWaterBorders(20, 3);
            terrain.lockInnerTerrain(5, 10, 3, 5, 2, 5);

            //terrain.randomize(terrain.Mtn_Spawns,terrain.Water_Spawns,terrain.Tree_Spawns);
            terrain.smooth(.00001, 2000, .9);

            terrain.setAllLocks(false);
            terrain.smooth(.01, 5, .2);

            return terrain.toTerrainTiles();
        }
    }
}

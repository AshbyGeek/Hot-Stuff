using UnityEngine;
using System.Collections;

public class TerrainGen : MonoBehaviour {
	
	public enum tileType{
		mountain,
		tree,
		brush,
		plains,
		water
	}
	
	public MeshFilter terrainObjMeshFilter;
	public float terrHeight = 5.0f;
	
	public Object water_obj;
	public Object tree_obj;
	public Object plains_obj;
	public Object mountain_obj;
	public Object brush_obj;
	public int size = 40;
	
	public int Tree_Spawns = 5;
	public int Water_Spawns = 5;
	public int Mtn_Spawns = 5;
	
	
	private int cols;
    private int rows;

    private const int mtn_thresh = 80;
    private const int plain_thresh = 20;
    private const int brush_thresh = 40;
    private const int tree_thresh = 60;

    private const int mtn_chain = 0;
    private const int water_chain = 2;

    private float[] array;
    private bool[,] locked;
	
	// Use this for initialization
	void Start () {
		this.init(size,size);
		this.lockElev(1,1, 90);
		this.lockElev(1,2, 90);
		this.lockElev(2,2, 90);
		this.lockElev(2,1, 90);
		
		this.lockElev(21,21, 0);
		this.lockElev(21,22, 0);
		this.lockElev(22,22, 0);
		this.lockElev(22,21, 0);


		this.randomize(Mtn_Spawns,Water_Spawns,Tree_Spawns);
		this.smooth(.00000001, 3000,0.9);
		
		Vector3[] newVertices = new Vector3[this.rows * this.cols];
		Vector2[] newUV = new Vector2[newVertices.Length];
		
		int rects = (this.rows - 1) * (this.cols - 1);
		int[] newTriangles = new int[rects*2*3];
		
		int curTri = 0;
		for (int i = 0; i < this.rows - 1; i++){
			for (int j = 0; j < this.cols - 1; j++){
				newTriangles[curTri] 		= (i)	*this.cols + (j+1);
				newTriangles[curTri + 1] 	= (i+1)	*this.cols +  j;
				newTriangles[curTri + 2]	=  i	*this.cols +  j;
				
				newTriangles[curTri + 3] 	= (i)	*this.cols + (j+1);
				newTriangles[curTri + 4] 	= (i+1)	*this.cols + (j+1);
				newTriangles[curTri + 5] 	= (i+1)	*this.cols +  j;
				
				curTri += 6;
			}
		}
		
		
		for (int i = 0; i < size; i++){
			for (int j = 0; j < size; j++){
				Object tmpObj;
				
				switch(type(i,j))
				{
				case tileType.mountain:
					tmpObj = mountain_obj;
					break;
				case tileType.tree:
					tmpObj = tree_obj;
					
					break;
				case tileType.brush:
					tmpObj = brush_obj;
					break;
				case tileType.plains:
					tmpObj = plains_obj;
					break;
				default:
					tmpObj = water_obj;
					break;
				}
				
				newVertices[i*this.cols + j] = new Vector3(i - (this.rows - 1)/2,
				                                           this.array[i*this.cols+j]/terrHeight,
				                                           j - (this.cols - 1)/2);
				newUV[i*this.cols + j] = new Vector2(i - (this.rows - 1)/2,
				                                     j - (this.cols - 1)/2);
				
				if (tmpObj != null)
				{
					Object newInst = Instantiate(tmpObj,
				            new Vector3(i - (this.rows - 1)/2,
                                        this.array[i*this.cols+j]/terrHeight,
                                        j - (this.cols - 1)/2),
				            Quaternion.identity);
				}
				
			}
		}
		
		
		Mesh tmp = new Mesh();
		tmp.vertices = newVertices;
		tmp.triangles = newTriangles;
		tmp.uv = newUV;
		tmp.RecalculateNormals();
		tmp.RecalculateBounds();
		//tmp.Optimize(); //Higher load time, faster draw speed
		terrainObjMeshFilter.mesh = tmp;
		
	}

    public void init(int cols, int rows)
    {
        this.cols = cols;
        this.rows = rows;
        this.array = new float[cols * rows];
        this.locked = new bool[rows, cols];

        for (int i = 0; i < cols; i++)
            for (int j = 0; j < rows; j++)
                this.locked[i, j] = false;

        for (int i = 0; i < cols * rows; i++)
        {
            this.array[i] = 50;
        }
    }

    public void randomize(int num_mtn, int num_water, int num_forest)
    {
        for (int y = 0; y < num_mtn; y++)
        {
            int randrow = Random.Range(1, this.rows - 2);
            int randcol = Random.Range(1, this.cols - 2);
            //this.lockElev(randrow,randcol,Random.Range(TerrainGen.mtn_thresh,100);
            this.lockElev(randrow, randcol, 100);

            for (int x = 0; x < num_mtn; x++)
            {
                randrow += Random.Range(-2, 5);
                randcol += Random.Range(-2, 5);
                randrow = System.Math.Max(1, randrow);
                randrow = System.Math.Min(this.rows - 2, randrow);
                randcol = System.Math.Max(1, randcol);
                randcol = System.Math.Min(this.cols - 2, randcol);
                this.lockElev(randrow, randcol, 100);
                this.lockElev(randrow, randcol+1, 100);
                this.lockElev(randrow+1, randcol, 100);
                this.lockElev(randrow+1, randcol+1, 100);
            }
        }

        for (int y = 0; y < num_water; y++)
        {
            int randrow = Random.Range(0, this.rows - 1);
            int randcol = Random.Range(0, this.cols - 1);
            //this.lockElev(randrow,randcol,Random.Range(0,self.water_thresh-10);
            this.lockElev(randrow, randcol, 0);

            for (int x = 0; x < num_mtn; x++)
            {
                randrow += Random.Range(-2, 5);
                randcol += Random.Range(-2, 5);
                randrow = System.Math.Max(0, randrow);
                randrow = System.Math.Min(this.rows - 1, randrow);
                randcol = System.Math.Max(0, randcol);
                randcol = System.Math.Min(this.cols - 1, randcol);
                this.lockElev(randrow, randcol, 0);
            }
        }


        for (int y = 0; y < num_forest; y++)
        {
            int randrow = Random.Range(0, this.rows - 1);
            int randcol = Random.Range(0, this.cols - 1);
            this.lockElev(randrow, randcol, Random.Range(TerrainGen.brush_thresh, TerrainGen.mtn_thresh - 1));
            //this.lockElev(randrow, randcol, 0);
        }

    }

    public void lockElev(int row, int col, int value)
    {
        int index = row * this.cols + col;
        this.array[index] = value;
        this.locked[row, col] = true;
    }

    public TerrainGen.tileType type(int row, int col)
    {
        float curVal = this.value(row, col);

        if (curVal > TerrainGen.mtn_thresh)
            return TerrainGen.tileType.mountain;
        if (curVal > TerrainGen.tree_thresh)
            return TerrainGen.tileType.tree;
        if (curVal >= TerrainGen.brush_thresh)
            return TerrainGen.tileType.brush;
        if (curVal >= TerrainGen.plain_thresh)
            return TerrainGen.tileType.plains;

        return TerrainGen.tileType.water;
    }

    public float value(int row, int col)
    {
        return this.array[row * this.cols + col];
    }

    public void smooth(double thresh, int max_pass, double weight)
    {
        // poisson solver!
        double resid = 1;
        int iter_count = 0;

        while ((resid > thresh) & (iter_count < max_pass))
        {
            resid = 0;
            iter_count += 1;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    //skip locked cells
                    if (this.locked[i, j])
                        continue;

                    //need to update i,j and add to resid

                    int r = System.Math.Min(j + 1, this.cols - 1);
                    int l = System.Math.Max(j - 1, 0);
                    int u = System.Math.Min(i + 1, this.rows - 1);
                    int d = System.Math.Max(i - 1, 0);

                    //update = 0.25*(this.value(i+1,j) + this.value(i-1,j) +
                    //               this.value(i,j+1) + this.value(i,j-1) -
                    //               4*this.value(i,j)

                    double update = 0.25 * (this.value(d, j) + this.value(u, j) +
                                   this.value(i, r) + this.value(i, l) -
                                   4 * this.value(i, j));

                    this.array[i * this.cols + j] += (int)(update * weight);
                    resid += update * update;

                }
            }

            if ((iter_count % 100) == 0)
            {
                System.Console.WriteLine("residual: " + System.Convert.ToString(resid));
                System.Console.WriteLine("pass: " + System.Convert.ToString(iter_count));
            }
        }
    }

}
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
	
	public Object water_tile;
	public Object tree_tile;
	public Object plains_tile;
	public Object mountain_tile;
	public Object brush_tile;
	public int size = 40;
	
	public int Tree_Spawns = 5;
	public int Water_Spawns = 5;
	public int Mtn_Spawns = 5;
	
	// Use this for initialization
	void Start () {
		this.init(size,size);
		//this.lockElev(0,1, 0);
		//this.lockElev(19,1, 0);


		this.randomize(8,8,8);
		this.smooth(.00001, 2000,0.9);
		
		
		for (int i = 0; i < size; i++){
			for (int j = 0; j < size; j++){
				Object tmpObj;
				
				switch(type(i,j))
				{
				case tileType.mountain:
					tmpObj = mountain_tile;
					break;
				case tileType.tree:
					tmpObj = tree_tile;
					break;
				case tileType.brush:
					tmpObj = brush_tile;
					break;
				case tileType.plains:
					tmpObj = plains_tile;
					break;
				default:
					tmpObj = water_tile;
					break;
				}
				
				Instantiate(tmpObj, new Vector3(i,0,j), Quaternion.identity);
			}
		}
	}

	// Update is called once per frame
	//void Update () {

	//}
	
	int cols;
    int rows;

    const int mtn_thresh = 58;
    const int plain_thresh = 43;
    const int brush_thresh = 47;
    const int tree_thresh = 47;

    const int mtn_chain = 0;
    const int water_chain = 2;

    int[] array;
    bool[,] locked;

    public void init(int cols, int rows)
    {
        this.cols = cols;
        this.rows = rows;
        this.array = new int[cols * rows];
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
            int randrow = Random.Range(0, this.rows - 1);
            int randcol = Random.Range(0, this.cols - 1);
            //this.lockElev(randrow,randcol,Random.Range(TerrainGen.mtn_thresh,100);
            this.lockElev(randrow, randcol, 100);

            for (int x = 0; x < num_mtn; x++)
            {
                randrow += Random.Range(-2, 5);
                randcol += Random.Range(-2, 5);
                randrow = System.Math.Max(0, randrow);
                randrow = System.Math.Min(this.rows - 1, randrow);
                randcol = System.Math.Max(0, randcol);
                randcol = System.Math.Min(this.cols - 1, randcol);
                this.lockElev(randrow, randcol, 100);
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
        int curVal = this.value(row, col);

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

    public int value(int row, int col)
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

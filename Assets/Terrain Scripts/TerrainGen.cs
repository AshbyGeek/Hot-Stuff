using UnityEngine;
using System.Collections;

public class TerrainGen : MonoBehaviour {
	
    public int mtn_thresh = 75;
    public int tree_thresh = 46;
    public int brush_thresh = 25;
    public int plain_thresh = 17;
	public int water_thresh = 7;
	//anything lower than this is deep water
	
    public int mtn_chain = 4;
    public int water_chain = 2;
	

    private float[,] array;
    private bool[,] locked;
	public TerrainTile[,] tiles;
	
	public int cols;
    public int rows;
	public int Mtn_Spawns;
	public int Water_Spawns;
	public int Tree_Spawns;
	
	public bool startFinished = false;
	
	void Start () {
	}
	
	
	//initialize some basic data fields
    public void init(int cols, int rows)
    {
        this.cols = cols;
        this.rows = rows;
        this.array = new float[rows, cols];
        this.locked = new bool[rows, cols];
		
		//use this to init lock values to 0
        setAllLocks(false);

        for (int i = 0; i < rows; i++)
        {
			for (int j = 0; j < cols; j++)
            	this.array[i,j] = 50;
        }
    }
	
	public void setAllLocks(bool state){
        for (int i = 0; i < cols; i++)
            for (int j = 0; j < rows; j++)
                this.locked[i, j] = state;
	}
	
	//make a collection of terrain tiles from the purely numeric array
	public void toTerrainTiles(){
		this.tiles = new TerrainTile[this.rows,this.cols];
		for (int i = 0; i < this.rows; i++){
			for (int j = 0; j < this.cols; j++){
				float height = this.array[i,j];
				TerrainTile curTile;
								
		        if (height >= mtn_thresh){
					MtnTile tmp = ScriptableObject.CreateInstance<MtnTile>();					tmp.init(height);
		            curTile = (TerrainTile) tmp;
				}
		        else if (height >= tree_thresh){
					TreeTile tmp = ScriptableObject.CreateInstance<TreeTile>();
					tmp.init(height);
		            curTile = (TerrainTile) tmp;
				}
				else if (height >= brush_thresh){
					BrushTile tmp = ScriptableObject.CreateInstance<BrushTile>();
					tmp.init(height);
		            curTile = (TerrainTile) tmp;
				}
				else if (height >= plain_thresh){
					PlainTile tmp = ScriptableObject.CreateInstance<PlainTile>();
					tmp.init(height);
		            curTile = (TerrainTile) tmp;
				}
				else if (height >= water_thresh){
					WaterTile tmp = ScriptableObject.CreateInstance<WaterTile>();
					tmp.init(height);
		            curTile = (TerrainTile) tmp;
				}
				else{
					curTile = null;
				}
				tiles[i,j] = curTile;
			}
		}
	}

    public void randomize(int num_mtn, int num_water, int num_forest)
    {
        for (int y = 0; y < num_mtn; y++)
        {
            int randrow = Random.Range(1, this.rows - 2);
            int randcol = Random.Range(1, this.cols - 2);
            //this.lockElev(randrow,randcol,Random.Range(TerrainGen.mtn_thresh,100);
            this.lockBlock(randrow, randcol, 100);

            for (int x = 0; x < num_mtn; x++)
            {
                randrow += Random.Range(-2, 5);
                randcol += Random.Range(-2, 5);
                randrow = System.Math.Max(1, randrow);
                randrow = System.Math.Min(this.rows - 2, randrow);
                randcol = System.Math.Max(1, randcol);
                randcol = System.Math.Min(this.cols - 2, randcol);
                this.lockBlock(randrow, randcol, 100);
            }
        }

        for (int y = 0; y < num_water; y++)
        {
            int randrow = Random.Range(0, this.rows - 1);
            int randcol = Random.Range(0, this.cols - 1);
            //this.lockElev(randrow,randcol,Random.Range(0,self.water_thresh-10);
            this.lockBlock(randrow, randcol, 0);

            for (int x = 0; x < num_mtn; x++)
            {
                randrow += Random.Range(-2, 5);
                randcol += Random.Range(-2, 5);
                randrow = System.Math.Max(0, randrow);
                randrow = System.Math.Min(this.rows - 1, randrow);
                randcol = System.Math.Max(0, randcol);
                randcol = System.Math.Min(this.cols - 1, randcol);
                this.lockBlock(randrow, randcol, 0);
            }
        }


        for (int y = 0; y < num_forest; y++)
        {
            int randrow = Random.Range(0, this.rows - 1);
            int randcol = Random.Range(0, this.cols - 1);
            this.lockBlock(randrow, randcol, Random.Range(brush_thresh, mtn_thresh - 1));
            //this.lockElev(randrow, randcol, 0);
        }

    }

    public void lockBlock(int row, int col, int value)
    {
		if ((row+1 < this.rows 	&& this.locked[row+1, col]) ||
		    (row-1 > 0 			&& this.locked[row-1, col]) ||
		    (col+1 < this.cols 	&& this.locked[row, col+1]) ||
		    (col-1 > 0 			&& this.locked[row, col-1])
		   )
			return;
		
        this.array[row, col] = value;
        this.locked[row, col] = true;
		
		if (row+1 < this.rows){
			this.locked[row + 1, col] = true;
        	this.array[row+1, col] = value;
		}
		if (row-1 > 0){
			this.locked[row-1, col] = true;
        	this.array[row-1, col] = value;
		}
		
		if (col+1 < this.cols){
			this.locked[row,    col + 1] = true;
        	this.array[row,		col + 1] = value;
		}
		if (col-1 > 0){
			this.locked[row, col-1]	= true;
        	this.array[row, col - 1] = value;
		}
    }
	
	public void lockPoint(int row, int col, int value){
		this.array[row,col] = value;
		this.locked[row,col] = true;
	}
	
	public void lockEdges(){
		//lock the corners to very deep values
		lockPoint(0,0,-20);
		lockPoint(0,cols-1,-20);
		lockPoint(rows-1,0,-20);
		lockPoint(rows-1,cols-1,-20);
		
		//smooth to get the values between
		smooth(.0000001,4,.5);
		
		for (int j = 0; j < cols; j+= cols-1){
			for (int i = 0; i < rows; i++){
				if (array[i,j] >= water_thresh)
					lockPoint(i,j,water_thresh);
				else
					locked[i,j] = true;
			}
		}
		for (int i = 0; i < rows; i+= rows-1){
			for (int j = 0; j < cols; j++){
				if (array[i,j] >= water_thresh)
					lockPoint(i,j,water_thresh);
				else
					locked[i,j] = true;
			}
		}
	}
	
	
	//default smoother
	public void smooth(){
		this.smooth(.00000001, 3000, 0.9);
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

                    //update = 0.25*(this.array(i+1,j) + this.array(i-1,j) +
                    //               this.array(i,j+1) + this.array(i,j-1) -
                    //               4*this.array(i,j)

                    double update = 0.25 * (this.array[d, j] + this.array[u, j] +
                                   this.array[i, r] + this.array[i, l] -
                                   4 * this.array[i, j]);

                    this.array[i, j] += (int)(update * weight);
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

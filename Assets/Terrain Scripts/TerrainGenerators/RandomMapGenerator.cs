using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomTerrainGenerator : ITerrainSource
{
    private TerrainGen terrain = new TerrainGen()
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

    private class TerrainGen
    {
        public int max_val = 100;
        public int mtn_thresh = 75;
        public int tree_thresh = 46;
        public int brush_thresh = 25;
        public int plain_thresh = 17;
        public int water_thresh = 7;
        //anything lower than this is deep water

        public int mtn_chain = 4;
        public int water_chain = 2;

        public int cols;
        public int rows;
        public int Mtn_Spawns;
        public int Water_Spawns;
        public int Tree_Spawns;


        private float[,] array;
        private bool[,] locked;


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
                    this.array[i, j] = 50;
            }
        }

        public void setAllLocks(bool state)
        {
            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    this.locked[i, j] = state;
        }

        // Extract the completed 2 dimensional tile array
        public TerrainTile[,] toTerrainTiles()
        {
            var tiles = new TerrainTile[this.rows, this.cols];
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    float height = this.array[i, j];
                    TerrainTile curTile;

                    if (height >= mtn_thresh)
                    {
                        curTile = new MtnTile();
                        curTile.init(height);
                    }
                    else if (height >= tree_thresh)
                    {
                        curTile = new TreeTile();
                        curTile.init(height);
                    }
                    else if (height >= brush_thresh)
                    {
                        curTile = new BrushTile();
                        curTile.init(height);
                    }
                    else if (height >= plain_thresh)
                    {
                        curTile = new PlainTile();
                        curTile.init(height);
                    }
                    else if (height >= water_thresh)
                    {
                        curTile = new WaterTile();
                        curTile.init(height);
                    }
                    else
                    {
                        curTile = new DeepWaterTile();
                        curTile.init(height);
                    }
                    tiles[i, j] = curTile;
                }
            }
            return tiles;
        }

        //rowVals and colVals must have the same length
        //they are the x, y indexes of possible lockable locations
        public void lockRandBlocks(int numLocks, List<int> rowVals, List<int> colVals, int minVal, int maxVal)
        {
            for (int i = 0; i < numLocks; i++)
            {
                while (true)
                {
                    int indx = Random.Range(0, rowVals.Count);
                    int row = rowVals[indx];
                    int col = colVals[indx];
                    int val = Random.Range(minVal, maxVal);
                    if (lockBlock(row, col, val))
                        break;
                }
            }
        }

        public void lockWaterBorders(int numLocks, int borderSize)
        {
            var rowVals = new List<int>();
            var colVals = new List<int>();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (row < borderSize || row > rows - 1 - borderSize)
                    {
                        if (col > borderSize && col < cols - 1 - borderSize)
                        {
                            rowVals.Add(row);
                            colVals.Add(col);
                        }
                        else
                        {
                            array[row, col] = water_thresh;
                        }
                    }
                    else
                    {
                        if (col > borderSize && col < cols - 1 - borderSize)
                        {
                            array[row, col] = water_thresh;
                        }
                        else
                        {
                            rowVals.Add(row);
                            colVals.Add(col);
                        }
                    }
                }
            }

            lockRandBlocks(numLocks, rowVals, colVals, water_thresh + 1, plain_thresh - 1);
        }

        public void lockInnerTerrain(int numLocks, int borderSize)
        {
            List<int> rowVals = new List<int>();
            List<int> colVals = new List<int>();
            for (int row = borderSize; row < rows - borderSize; row++)
            {
                for (int col = borderSize; col < cols - borderSize; col++)
                {
                    rowVals.Add(row);
                    colVals.Add(col);
                }
            }

            lockRandBlocks(numLocks, rowVals, colVals, water_thresh, max_val);
        }
        public void lockInnerTerrain(int mtn_locks,
                                     int tree_locks,
                                     int brush_locks,
                                     int plains_locks,
                                     int water_locks,
                                     int borderSize)
        {
            var rowVals = new List<int>();
            var colVals = new List<int>();
            for (int row = borderSize; row < rows - borderSize; row++)
            {
                for (int col = borderSize; col < cols - borderSize; col++)
                {
                    rowVals.Add(row);
                    colVals.Add(col);
                }
            }

            lockRandBlocks(mtn_locks, rowVals, colVals, mtn_thresh, max_val);
            lockRandBlocks(tree_locks, rowVals, colVals, tree_thresh, mtn_thresh - 1);
            lockRandBlocks(brush_locks, rowVals, colVals, brush_thresh, tree_thresh - 1);
            lockRandBlocks(plains_locks, rowVals, colVals, plain_thresh, brush_thresh - 1);
            lockRandBlocks(water_locks, rowVals, colVals, water_thresh, plain_thresh - 1);
        }

        public void lockBelowVal(int val)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (array[i, j] < val)
                        locked[i, j] = true;
                }
            }
        }

        public void setEdgeLocks(bool state, int maxVal)
        {
            for (int i = 0; i < rows; i++)
            {
                locked[i, 0] = state;
                if (array[i, 0] > maxVal)
                    array[i, 0] = maxVal;

                locked[i, cols - 1] = state;
                if (array[i, cols - 1] > maxVal)
                    array[i, cols - 1] = maxVal;
            }

            for (int j = 0; j < cols; j++)
            {
                locked[0, j] = state;
                if (array[0, j] > maxVal)
                    array[0, j] = maxVal;

                locked[rows - 1, j] = state;
                if (array[rows - 1, j] > maxVal)
                    array[rows - 1, j] = maxVal;
            }
        }

        public void lockEdges(int numLocks, int minVal, int maxVal)
        {
            for (int i = 0; i < numLocks; i++)
            {
                int tmp = Random.Range(0, rows);
                int val = Random.Range(minVal, maxVal);
                lockPoint(tmp, 0, val);

                tmp = Random.Range(0, rows);
                val = Random.Range(minVal, maxVal);
                lockPoint(tmp, cols - 1, val);

                tmp = Random.Range(0, cols);
                val = Random.Range(minVal, maxVal);
                lockPoint(0, tmp, val);


                tmp = Random.Range(0, cols);
                val = Random.Range(minVal, maxVal);
                lockPoint(rows - 1, tmp, val);
            }
        }


        //these are legacy methods
        public void randomize(int num_mtn, int num_water, int num_forest)
        {
            randomize(num_mtn, num_water, num_forest, 0);
        }
        public void randomize(int num_mtn, int num_water, int num_forest, int border)
        {
            for (int y = 0; y < num_mtn; y++)
            {
                while (true)
                {
                    int randrow = Random.Range(border, this.rows - 1 - border);
                    int randcol = Random.Range(border, this.cols - 1 - border);
                    //this.lockElev(randrow,randcol,Random.Range(TerrainGen.mtn_thresh,100);
                    if (!lockBlock(randrow, randcol, 100))
                        continue;

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
                    break;
                }
            }

            for (int y = 0; y < num_water; y++)
            {
                while (true)
                {
                    int randrow = Random.Range(border, this.rows - 1 - border);
                    int randcol = Random.Range(border, this.cols - 1 - border);
                    //this.lockElev(randrow,randcol,Random.Range(0,self.water_thresh-10);
                    if (!this.lockBlock(randrow, randcol, 0))
                        continue;

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
                    break;
                }
            }


            for (int y = 0; y < num_forest; y++)
            {
                int randrow = Random.Range(border, this.rows - 1 - border);
                int randcol = Random.Range(border, this.cols - 1 - border);
                this.lockBlock(randrow, randcol, Random.Range(brush_thresh, mtn_thresh - 1));
                //this.lockElev(randrow, randcol, 0);
            }

        }

        //returns false if the point was unlockable
        public bool lockBlock(int row, int col, int value)
        {
            if (locked[row, col] ||
                (row + 1 < this.rows && this.locked[row + 1, col]) ||
                (row - 1 > 0 && this.locked[row - 1, col]) ||
                (col + 1 < this.cols && this.locked[row, col + 1]) ||
                (col - 1 > 0 && this.locked[row, col - 1])
               )
                return false;

            this.array[row, col] = value;
            this.locked[row, col] = true;

            if (row + 1 < this.rows)
            {
                this.locked[row + 1, col] = true;
                this.array[row + 1, col] = value;
            }
            if (row - 1 > 0)
            {
                this.locked[row - 1, col] = true;
                this.array[row - 1, col] = value;
            }

            if (col + 1 < this.cols)
            {
                this.locked[row, col + 1] = true;
                this.array[row, col + 1] = value;
            }
            if (col - 1 > 0)
            {
                this.locked[row, col - 1] = true;
                this.array[row, col - 1] = value;
            }

            return true;
        }

        //returns false if the point was already locked
        public bool lockPoint(int row, int col, int value)
        {
            if (locked[row, col])
                return false;
            this.array[row, col] = value;
            this.locked[row, col] = true;
            return true;
        }


        //default smoother
        public void smooth()
        {
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

}
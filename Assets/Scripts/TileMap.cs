using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {

	public TileType[] tileTypes;

	int[,] tiles;

	public int tileSizeX;
	public int tileSizeZ;

	void Start () {
        GenerateMapData();

        //L shaped mountain
        tiles[1,1] = 2;
		tiles[1,2] = 2;
		tiles[1,3] = 2;
		tiles[1,4] = 2;
		tiles[1,5] = 2;
		tiles[2,5] = 2;
		tiles[3,5] = 2;
        //O shaped swamp
		tiles[8,8] = 1;
		tiles[8,7] = 1;
		tiles[7,8] = 1;
		tiles[7,7] = 1;
		tiles[7,5] = 1;
		tiles[7,6] = 1;
		tiles[8,6] = 1;

		//Prefabs instantiation
		GenerateMapVisual();

	}
	
	void GenerateMapVisual () {
		for (int x = 0; x < tileSizeX; x++) 
		{
			for (int z = 0; z < tileSizeZ; z++) 
			{
				GameObject newTile = Instantiate(tileTypes[tiles[x,z]].tileVisualPrefab, new Vector3(x, 0, z), Quaternion.identity) as GameObject;
				newTile.transform.parent = transform;
			}
		}
	}

    void GenerateMapData()
    {
        //Tile map Allocation
        tiles = new int[tileSizeX, tileSizeZ];

        //Tile map Initialization
        for (int x = 0; x < tileSizeX; x++)
        {
            for (int z = 0; z < tileSizeZ; z++)
            {
                tiles[x, z] = 0;
            }
        }
    }

}

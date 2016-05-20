using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {

	TileType[] tileTypes;

	int[,] tiles;

	public int tileSizeX;
	public int tileSizeZ;

	void Start () {
		//Tile map Allocation
		tiles = new int[ tileSizeX, tileSizeZ];

		//Tile map Initialization
		for (int x = 0; x < tileSizeX; x++) 
		{
			for (int z = 0; z < tileSizeZ; z++) 
			{
				tiles[x,z] = 0;
			}
		}
	}
	
	void Update () {
	
	}
}

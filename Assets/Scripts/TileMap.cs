using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour {
    public GameObject selectedUnit;
	public TileType[] tileTypes;

	int[,] tiles;
    Node[,] graph;

    public int tileSizeX;
	public int tileSizeZ;

	void Start () {
        GenerateMapData();
        GenerateRandomMap();
        GeneratePathfindingGraph();
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
                ClickableTile newTileCT = newTile.GetComponent<ClickableTile>();
                newTileCT.Set(x, z);
                newTileCT.tileMap = this;
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
    
    void GenerateRandomMap()
    {
        for (int x = 0; x < tileSizeX; x++)
        {
            for (int z = 0; z < tileSizeZ; z++)
            {
                int rand = Mathf.FloorToInt(Random.Range(0, 4));
                if (rand == 1)
                {
                    tiles[x, z] = 1;
                } else {
                    if (rand == 2)
                    {
                        tiles[x, z] = 2;
                    }
                }
                
            }
        }
    }

    public Vector3 TileToWorldCoord(int x, int z)
    {
        return new Vector3(x, selectedUnit.transform.position.y, z);
    }


    public void moveSelevtedUnit(int x, int z)
    {
        /*
        selectedUnit.GetComponent<Unit>().SetCoord(x, z);
        selectedUnit.transform.position = TileToWorldCoord(x, z);
        */
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        //List of unvivisted nodes
        List<Node> unvisited = new List<Node>();

        Node source = graph[selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileZ];
        dist[source] = 0;
        prev[source] = null;
        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }
        while (unvisited.Count > 0)
        {
            Node u = unvisited.OrderBy(n => dist[n]).First();
            unvisited.Remove(u);
            foreach(Node v in u.neighbours)
            {

            }
        }
    }


    class Node
    {
        public List<Node> neighbours;
        public Node()
        {
            neighbours = new List<Node>();
        }
    }

    

    void GeneratePathfindingGraph()
    {
        graph = new Node[tileSizeX, tileSizeZ];
        for (int x = 0; x < tileSizeX; x++)
        {
            for (int z = 0; z < tileSizeZ; z++)
            {
                //4-way connection
                if (x > 0)
                    graph[x, z].neighbours.Add(graph[x - 1, z]);
                
                if (x < tileSizeX)
                    graph[x, z].neighbours.Add(graph[x + 1, z]);
                if (z > 0)
                    graph[x, z].neighbours.Add(graph[x, z - 1]);
                if (z < tileSizeZ)
                    graph[x, z].neighbours.Add(graph[x, z + 1]);
            }
        }
    }
}

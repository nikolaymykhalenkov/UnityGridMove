using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour {
    public static TileMap Instance;
    public bool DiagonalMovementEnabled = true;
    public GameObject selectedUnit;
	public TileType[] tileTypes;

	int[,] tiles;
    Node[,] graph;
    List<Node> currentPath;

    public int tileSizeX;
	public int tileSizeZ;

    void Awake () {
        TileMap.Instance = this;
    } 

	void Start () {
        GenerateMapData();
        GenerateRandomMap();
        GenerateMapVisual();
        GeneratePathfindingGraph();
	}
	
	void GenerateMapVisual () {
		for (int x = 0; x < tileSizeX; x++) {
			for (int z = 0; z < tileSizeZ; z++) {
				GameObject newTile = Instantiate(tileTypes[tiles[x,z]].tileVisualPrefab, new Vector3(x, 0, z), Quaternion.identity) as GameObject;
				newTile.transform.parent = transform;
                ClickableTile newTileCT = newTile.GetComponent<ClickableTile>();
                newTileCT.Set(x, z);
                newTileCT.tileMap = this;
			}
		}
	}

    public int CostToEnter (int x, int z) {
        TileType tt = tileTypes[tiles[x,z]];
        return tt.enterCost;
    }

    void GenerateMapData() {
        //Tile map Allocation
        tiles = new int[tileSizeX, tileSizeZ];
        //Tile map Initialization
        for (int x = 0; x < tileSizeX; x++)
            for (int z = 0; z < tileSizeZ; z++)
                tiles[x, z] = 0;
    }
    
    void GenerateRandomMap() {
        for (int x = 0; x < tileSizeX; x++) {
            for (int z = 0; z < tileSizeZ; z++) {
                int rand = Mathf.FloorToInt(Random.Range(0, 4));
                if (rand == 1) {
                    tiles[x, z] = 1;
                } else {
                    if (rand == 2) {
                        tiles[x, z] = 2;
                    }
                }
            }
        }
    }

    public Vector3 TileToWorldCoord(int x, int z) {
        return new Vector3(x, selectedUnit.transform.position.y, z);
    }


    public void GeneratePathTo(int x, int z) {
        selectedUnit.GetComponent<Unit>().currentPath = null;
        currentPath = null;
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        //List of unvivisted nodes
        List<Node> unvisited = new List<Node>();
        Node source = graph[selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileZ];
        Node target = graph[x, z];
        dist[source] = 0;
        prev[source] = null;
        foreach (Node v in graph) {
            if (v != source) {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }
        while (unvisited.Count > 0) {
            Node u = null;
            foreach (Node possibleU in unvisited) {
                if (u == null || dist[possibleU] < dist[u]) {
                    u = possibleU;
                }
            }
            if (u == target)
                break;
            unvisited.Remove(u);
            foreach (Node v in u.neighbours) {
                //float alt = dist[u] + u.DistanceTo(v);
                float alt = dist[u] + CostToEnter(v.x, v.z);
                if (alt < dist[v]) {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
        if(prev[target] == null) {
            Debug.Log("No route!");
            return;
        } else {
            currentPath = new List<Node>();
            Node curr = target;
            while(curr != null) {
                currentPath.Add(curr); //Route from target to source
                curr = prev[curr];
            }
            currentPath.Reverse(); //Route from sourve to Target
            selectedUnit.GetComponent<Unit>().currentPath = currentPath;
        }
    }

    void GeneratePathfindingGraph() {
        graph = new Node[tileSizeX, tileSizeZ];
        for (int x = 0; x < tileSizeX; x++) 
            for (int z = 0; z < tileSizeZ; z++) 
                graph[x, z] = new Node(x, z);
        for (int x = 0; x < tileSizeX; x++) {
            for (int z = 0; z < tileSizeZ; z++) {
                //4-way connection
                if (x > 0)
                    graph[x, z].neighbours.Add(graph[x - 1, z]);
                if (x < (tileSizeX - 1))
                    graph[x, z].neighbours.Add(graph[x + 1, z]);
                if (z > 0)
                    graph[x, z].neighbours.Add(graph[x, z - 1]);
                if (z < (tileSizeZ - 1))
                    graph[x, z].neighbours.Add(graph[x, z + 1]);
                //4-way diagonals
                if (DiagonalMovementEnabled) {
                    if (x > 0 && z > 0)
                        graph[x, z].neighbours.Add(graph[x - 1, z - 1]);
                    if (x < (tileSizeX - 1) && z > 0)
                        graph[x, z].neighbours.Add(graph[x + 1, z - 1]);
                    if (x > 0 && z < (tileSizeZ - 1))
                        graph[x, z].neighbours.Add(graph[x - 1, z + 1]);
                    if (x < (tileSizeX - 1) && z < (tileSizeZ - 1))
                        graph[x, z].neighbours.Add(graph[x + 1, z + 1]);
                }
            }
        }
    }
}

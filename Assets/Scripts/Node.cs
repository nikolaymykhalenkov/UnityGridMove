using UnityEngine;
using System.Collections.Generic;

public class Node {
    public List<Node> neighbours;
    public int x;
    public int z;
    public Node() {
        neighbours = new List<Node>();
        x = 0;
        z = 0;
    }
    public Node(int dx, int dy) {
        neighbours = new List<Node>();
        x = dx;
        z = dy;
    }
    public void SetPosition(int dx, int dz) {
        x = dx;
        z = dz;
    }
    public float DistanceTo(Node n) {
        return Vector2.Distance(new Vector2(x, z), new Vector2(n.x, n.z));
    }
}

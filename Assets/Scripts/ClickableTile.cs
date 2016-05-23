using UnityEngine;
using System.Collections;

public class ClickableTile : MonoBehaviour {

    int tileX;
    int tileZ;
    public TileMap tileMap;

	void OnMouseUp ()
    {
        tileMap.moveSelevtedUnit(tileX, tileZ);
    }

    public void Set(int x, int z)
    {
        tileX = x;
        tileZ = z;
    }
}

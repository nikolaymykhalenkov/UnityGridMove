using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    public int tileX;
    public int tileZ;
    public void SetCoord (int x, int z)
    {
        tileX = x;
        tileZ = z;
    }

}

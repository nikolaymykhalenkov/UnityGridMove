using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

    public int tileX;
    public int tileZ;
    public float visualMoveSpeed = 100;
    public bool isVisualyMoving = false;
    public List<Node> currentPath = null;

    public void SetCoord(int x, int z) {
        tileX = x;
        tileZ = z;
    }
    
    void Start() {
        tileX = Mathf.FloorToInt(transform.position.x);
        tileZ = Mathf.FloorToInt(transform.position.z);
    }

    void Update() {
        if (currentPath != null) {
            int currNode = 0;
            while (currNode < currentPath.Count - 1) {
                Vector3 start = TileMap.Instance.TileToWorldCoord(currentPath[currNode].x, currentPath[currNode].z) - new Vector3(0, 0.44f, 0);
                Vector3 end = TileMap.Instance.TileToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode + 1].z) - new Vector3(0, 0.44f, 0);
                Debug.DrawLine(start, end, Color.red);
                currNode++;
            }
        }
        if (Input.GetButtonDown("Jump") && isVisualyMoving == false) {
            MoveToNextTile();
        }
    }

    public void MoveToNextTile () {
        if (currentPath == null)
            return;
        currentPath.RemoveAt(0);
        //transform.position = TileMap.Instance.TileToWorldCoord(currentPath[0].x, currentPath[0].z);
        tileX = currentPath[0].x;
        tileZ = currentPath[0].z;
        StartCoroutine(Move(transform.position, new Vector3(currentPath[0].x, transform.position.y, currentPath[0].z), visualMoveSpeed));
        if (currentPath.Count == 1) {
            currentPath = null;
        }
    }

    IEnumerator Move(Vector3 start, Vector3 end, float time) {
        isVisualyMoving = true;
        float t = 0;
        while (t < 1f) {
            t += Time.deltaTime / time; // sweeps from 0 to 1 in time seconds
            transform.position = Vector3.Lerp(start, end, t); // set position proportional to t
            yield return 0; // leave the routine and return here in the next frame
        }
        transform.position = end;
        isVisualyMoving = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCell : MonoBehaviour {
    Mesh mesh;

	void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
	}
	
	void Update () {
		
	}
}

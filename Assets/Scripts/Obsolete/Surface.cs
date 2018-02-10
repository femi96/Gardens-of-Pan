using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour {
	// Surface: Controller that handles garden's surface mesh
	
	// Usage:
	//			Function(); purpose
	

	// Variables:
	private Transform chunk;
	private Mesh mesh;
	
	public int gardenSize = 10;			// Garden size in units
	public int mapSize = 51;			// Garden size in height map points
	public float pointSize;
	public float[,] heightMap;			// 2D array of height, range from -1 to 1
	public float[,] typeMap;			// 2D array of height, range from -1 to 1

	private List<Vector3> newVertices = new List<Vector3>();
	private List<int> newTriangles = new List<int>();
	// private List<Vector2> newUV = new List<Vector2>();


	// Unity MonoBehavior Functions:
	void Awake() {
		chunk = this.gameObject.transform.GetChild(0);
		mesh = chunk.GetComponent<MeshFilter>().mesh;
	}

	void Start() {
		heightMap = new float[mapSize, mapSize];
		for(int z = 0; z < mapSize; z++) {
			for(int x = 0; x < mapSize; x++) {
				heightMap[x, z] = 0;
			}
		}
		pointSize = gardenSize/(mapSize - 1f);
		UpdateMesh();
	}


	// Functions:
	public void Dig(Vector3 v) {
		float g = gardenSize/2f;

		float vx = v.x + g;
		float vz = v.z + g;

		int cx = Mathf.RoundToInt(vx/pointSize);
		int cz = Mathf.RoundToInt(vz/pointSize);

		int r = Mathf.RoundToInt(0.5f/pointSize);

		for(int z = cz-r; z <= cz+r; z++) {
			for(int x = cx-r; x <= cx+r; x++) {
				if(x >= 0 && x < mapSize && z >= 0 && z < mapSize) {
					float d = 0;
					d = Mathf.Sqrt(Mathf.Pow(vx - (x*pointSize), 2) + Mathf.Pow(vz - (z*pointSize), 2));
					d = d - 0.2f;
					d = d * 5f/3f;
					d = Mathf.Max(0, d);
					heightMap[x, z] = Mathf.Min(heightMap[x, z], d - 0.5f);
				}
			}
		}
		UpdateMesh();
	}

	public void UpdateMesh() {
		float p = pointSize;
		float g = gardenSize/2f;

		newVertices.Clear();
		newTriangles.Clear();

		int[,] verticesMap = new int[mapSize, mapSize];

		// Upper surface mesh
		for(int z = 0; z < mapSize; z++) {
			for(int x = 0; x < mapSize; x++) {
				verticesMap[x, z] = newVertices.Count;
				newVertices.Add(new Vector3(x*p-g, heightMap[x, z], z*p-g));
			}
		}
		
		for(int z = 0; z < mapSize-1; z++) {
			for(int x = 0; x < mapSize-1; x++) {
				newTriangles.Add(verticesMap[x  ,z  ]);
				newTriangles.Add(verticesMap[x+1,z+1]);
				newTriangles.Add(verticesMap[x+1,z  ]);

				newTriangles.Add(verticesMap[x  ,z  ]);
				newTriangles.Add(verticesMap[x  ,z+1]);
				newTriangles.Add(verticesMap[x+1,z+1]);
			}
		}

		int[,] lowerVerticesMap = new int[mapSize, mapSize];

		// Edge surface mesh
		for(int x = 0; x < mapSize; x++) {
			int z; z = 0;
			lowerVerticesMap[x, z] = newVertices.Count;
			newVertices.Add(new Vector3(x*p-g, -1, z*p-g));
			z = mapSize-1;
			lowerVerticesMap[x, z] = newVertices.Count;
			newVertices.Add(new Vector3(x*p-g, -1, z*p-g));
		}
		for(int z = 0; z < mapSize; z++) {
			int x; x = 0;
			lowerVerticesMap[x, z] = newVertices.Count;
			newVertices.Add(new Vector3(x*p-g, -1, z*p-g));
			x = mapSize-1;
			lowerVerticesMap[x, z] = newVertices.Count;
			newVertices.Add(new Vector3(x*p-g, -1, z*p-g));
		}
		for(int x = 0; x < mapSize-1; x++) {
			int z; z = 0;
			newTriangles.Add(lowerVerticesMap[x, z]);
			newTriangles.Add(verticesMap[x  ,z  ]);
			newTriangles.Add(verticesMap[x+1,z  ]);
			newTriangles.Add(lowerVerticesMap[x, z]);
			newTriangles.Add(verticesMap[x+1,z  ]);
			newTriangles.Add(lowerVerticesMap[x+1, z]);

			z = mapSize-1;
			newTriangles.Add(lowerVerticesMap[x, z]);
			newTriangles.Add(verticesMap[x+1,z  ]);
			newTriangles.Add(verticesMap[x  ,z  ]);
			newTriangles.Add(lowerVerticesMap[x, z]);
			newTriangles.Add(lowerVerticesMap[x+1, z]);
			newTriangles.Add(verticesMap[x+1,z  ]);
		}
		for(int z = 0; z < mapSize-1; z++) {
			int x; x = 0;
			newTriangles.Add(lowerVerticesMap[x, z]);
			newTriangles.Add(verticesMap[x  ,z+1]);
			newTriangles.Add(verticesMap[x  ,z  ]);
			newTriangles.Add(lowerVerticesMap[x, z]);
			newTriangles.Add(lowerVerticesMap[x, z+1]);
			newTriangles.Add(verticesMap[x  ,z+1]);

			x = mapSize-1;
			newTriangles.Add(lowerVerticesMap[x, z]);
			newTriangles.Add(verticesMap[x  ,z  ]);
			newTriangles.Add(verticesMap[x  ,z+1]);
			newTriangles.Add(lowerVerticesMap[x, z]);
			newTriangles.Add(verticesMap[x  ,z+1]);
			newTriangles.Add(lowerVerticesMap[x, z+1]);
		}
		
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.RecalculateNormals();

		chunk.GetComponent<MeshCollider>().sharedMesh = null;
		chunk.GetComponent<MeshCollider>().sharedMesh = mesh;
	}
}

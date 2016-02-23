
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonMesh : MonoBehaviour 
{
	public List<Vector3> points;
	public Material material;

	public Mesh mesh;
	PolygonCollider2D polyCol;

	// Use this for initialization
	void Awake () {
		CreateMesh ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CreateMesh()
	{
		mesh = new Mesh ();
		mesh.vertices = points.ToArray ();

		List<Vector2> vector2Points = new List<Vector2>();
		
		for(int i = 0; i < points.Count; i++)
		{
			vector2Points.Add(new Vector2(points[i].x, points[i].y));
		}

		mesh.uv = vector2Points.ToArray();
		mesh.triangles = Triangulate (points.Count).ToArray();

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		MeshRenderer renderer = gameObject.AddComponent<MeshRenderer> ();
		MeshFilter filter = gameObject.AddComponent<MeshFilter> ();
		filter.mesh = mesh;
		renderer.material = material;
		polyCol = gameObject.AddComponent<PolygonCollider2D>();

		polyCol.points = vector2Points.ToArray ();

	}

	/*-----------------------------------------------------
	 * Creates 2 triangles out of a group of 4 points
	 * vertices go top bottom top bottom, so imagine:
	 * 
	 *       0____2           
	 *       |\   |
	 *       | \T2|
	 *       |  \ |
	 *       |T1 \|
	 *       1____3   
	 * ---------------------------------------------------*/
	
	public List<int> Triangulate (int count)
	{
		List<int> indices = new List<int> ();

		int c1 = 0;
		int c2 = 1;
		int c3 = 2;
		for(int i = 0 ; i < count-2; i++)
		{
			indices.Add(c1);
			indices.Add (c2);
			indices.Add (c3);

			c2++;
			c3++;
		}

		return indices;
	}

	public Vector2 getRightMostVertice()
	{
		int index = 0;

		for(int i = 1; i < mesh.vertices.Length; i++)
		{
			if(mesh.vertices[index].x < mesh.vertices[i].x)
			{
				index = i;
			}
		}

		return new Vector2(mesh.vertices[index].x, mesh.vertices[index].y);
	}

	public Vector2 getLeftMostVertice()
	{
		int index = 0;
		
		for(int i = 1; i < mesh.vertices.Length; i++)
		{
			if(mesh.vertices[index].x > mesh.vertices[i].x)
			{
				index = i;
			}
		}
		
		return new Vector2(mesh.vertices[index].x, mesh.vertices[index].y);
	}
}

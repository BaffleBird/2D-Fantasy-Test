using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UVSetter : MonoBehaviour
{
	[SerializeField] int rows = 0;
	[SerializeField] int columns = 0;
	[SerializeField] Vector2 index;

	[ContextMenu("Create UV")]
	void SetMeshUV()
	{
		Mesh mesh = new Mesh();
		
		#if UNITY_EDITOR
		MeshFilter meshfilter = GetComponent<MeshFilter>();
		Mesh meshCopy = Instantiate(meshfilter.sharedMesh) as Mesh;
		mesh = meshfilter.mesh = meshCopy;
		#else
		mesh = GetComponent<MeshFilter>().mesh;
		#endif
	}

	[ContextMenu("Set UV")]
	void SetSharedMeshUV()
	{
		if (index.x > rows) index.x = rows;
		if (index.x < 1) index.x = 1;
		if (index.y > columns) index.y = columns;
		if (index.y < 1) index.y = 1;

		Mesh mesh = new Mesh();
		mesh = GetComponent<MeshFilter>().sharedMesh;

		Vector2[] uvs = mesh.uv;
		float xfactor = 1f / rows;
		float yfactor = 1f / columns;

		//Bottom Left
		uvs[0].x = (index.x - 1) * xfactor;
		uvs[0].y = (index.y - 1) * yfactor;

		//Bottom Right
		uvs[1].x = (index.x - 1) * xfactor + xfactor;
		uvs[1].y = (index.y - 1) * yfactor;

		//Top Left
		uvs[2].x = (index.x - 1) * xfactor;
		uvs[2].y = (index.y - 1) * yfactor + yfactor;

		//Top Right
		uvs[3].x = (index.x - 1) * xfactor + xfactor;
		uvs[3].y = (index.y - 1) * yfactor + yfactor;

		mesh.uv = uvs;
	}

#if UNITY_EDITOR
	//Okay now make a button for saving dat new mesh
	[ContextMenu("Create Mesh")]
	void CreateMesh()
	{
		string filePath = EditorUtility.SaveFilePanelInProject("Save Procedural Mesh", "New Mesh", "asset", "");
		if (filePath == "") return;
		AssetDatabase.CreateAsset(GetComponent<MeshFilter>().sharedMesh, filePath);
	}
#endif
}

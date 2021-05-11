#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PrefabBrush : EditorWindow
{
	List<GameObject> prefabs = new List<GameObject>() { null };
	Transform parent = null;

	bool painting;
	bool selectObject = false;
	bool flipObject = false;
	bool randomFlip = false;

	Vector2 prefabScrollPosition;
	int selectedToolbar = 0;

	int amount = 1;
	float minRange = 1, maxRange = 1.5f;
	float offsetX, offsetY, offsetZ;

	string selectionSequence = "";

	[MenuItem("Window/PrefabBrush")]
	public static void ShowWindow() { GetWindow(typeof(PrefabBrush)); }

	private void OnEnable() { SceneView.duringSceneGui += OnSceneGUI;}

	private void OnDisable() { SceneView.duringSceneGui += OnSceneGUI; }

	// GUI Window
	private void OnGUI()
	{
		// Scroll through Prefabs
		EditorGUILayout.LabelField("Prefabs");
		prefabScrollPosition = GUILayout.BeginScrollView(prefabScrollPosition, false, true, GUILayout.MinHeight(10), GUILayout.MaxHeight(100), GUILayout.ExpandHeight(true));
		for (int i = 0; i < prefabs.Count; i++)
		{
			prefabs[i] = EditorGUILayout.ObjectField(prefabs[i], typeof(GameObject), false) as GameObject;
		}
		GUILayout.EndScrollView();

		// Add/Remove Prefabs
		if (GUILayout.Button("Add Item"))
		{
			prefabs.Add(null);
		}

		if (GUILayout.Button("Remove Item"))
		{
			if (prefabs.Count > 1)
			{
				prefabs.RemoveAt(prefabs.Count - 1);
			}
		}

		selectionSequence = EditorGUILayout.TextField("Selected Prefabs", selectionSequence);

		// Tranform Parent for Prefabs
		parent = EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true) as Transform;

		// Tool Bar
		selectedToolbar = GUILayout.Toolbar(selectedToolbar, new string[] { "Options", "Transformations" });
		if (selectedToolbar == 0)
		{

			EditorGUILayout.Space();
			amount = EditorGUILayout.IntField("Amount Per Press", amount);
			amount = Mathf.Clamp(amount, 1, int.MaxValue);

			if (amount > 1)
			{
				minRange = EditorGUILayout.FloatField("Min Range", minRange);
				maxRange = EditorGUILayout.FloatField("Max Range", maxRange);
			}

			EditorGUILayout.Space();
			selectObject = EditorGUILayout.Toggle("Select Object", selectObject);
			

			if (!painting)
			{
				if (GUILayout.Button("Start Painting"))
				{
					painting = true;
				}
			}
			else
			{
				if (GUILayout.Button("Stop Painting"))
				{
					painting = false;
				}
			}
		}
		else
		{
			//Transformation Settings
			EditorGUILayout.Space();
			flipObject = EditorGUILayout.Toggle("Flip Object", flipObject);
			randomFlip = EditorGUILayout.Toggle("Randomize Flip", randomFlip);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Offset", EditorStyles.boldLabel);

			offsetX = EditorGUILayout.FloatField("Offset X", offsetX);
			offsetY = EditorGUILayout.FloatField("Offset Y", offsetY);
			offsetZ = EditorGUILayout.FloatField("Offset Z", offsetZ);
		}
	}


	//The Actual Brush Mechanics
	void OnSceneGUI(SceneView c)
	{
		Event currentEvent = Event.current;
		if (painting && currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.P)
		{
			Vector3 originalPosition = Camera.current.ScreenToWorldPoint(new Vector3(currentEvent.mousePosition.x, -currentEvent.mousePosition.y + Camera.current.pixelHeight));
			for (int i = 0; i < amount; i++)
			{
				Vector3 newPos = originalPosition;
				if (i > 0) newPos += RandomCircle(minRange, maxRange);

				//********************Picks prefabs at random. Do something else*****************
				GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
				//Check if selectionSequence is empty
				//If not, parse it. Randomly pick an int from the resulting sequence and grab it from the prefabs (index-1 btw)

				if (selectionSequence != "")
				{
					int[] sequence = InterpretSequence(selectionSequence);
					prefab = prefabs[sequence[Random.Range(0, sequence.Length)] - 1];
				}

				newPos.z = newPos.y;	

				if (prefab != null)
				{
					var o = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
					o.transform.SetParent(parent);
					o.transform.position = newPos;
					o.transform.position = new Vector3(o.transform.position.x + offsetX, o.transform.position.y + offsetY, o.transform.position.y + offsetZ);

					if (selectObject)
					{
						GameObject[] go = { o };
						Selection.objects = go;
					}

					if(flipObject)
					{
						if (randomFlip)
						{
							int ran = Random.value < .5 ? 1 : -1;
							o.transform.localScale = new Vector3(ran * o.transform.localScale.x, o.transform.localScale.y, o.transform.localScale.z);
						}
						else
							o.transform.localScale = new Vector3(-o.transform.localScale.x, o.transform.localScale.y, o.transform.localScale.z);
					}

					Undo.RegisterCreatedObjectUndo(o, "Paint Prefab " + o.name);
					break;
				}
			}
		}
	}

	//Randomization circle
	public Vector3 RandomCircle(float minRange, float maxRange)
	{
		Vector3 position;
		float angle = Random.value * 360;
		position.x = (Random.Range(minRange, maxRange) * Mathf.Sin(angle * Mathf.Deg2Rad)) + offsetX;
		position.y = (Random.Range(minRange, maxRange) * Mathf.Cos(angle * Mathf.Deg2Rad)) + offsetY;
		position.z = position.y + offsetZ;

		return position;
	}

	//Parses the selection sequence by commas
	int[] InterpretSequence(string sequence)
	{
		string[] splitString = sequence.Split(',');
		int[] intSequence = new int[splitString.Length];
		for(int i = 0; i < splitString.Length; i++)
		{
			int.TryParse(splitString[i], out intSequence[i]);
		}

		return intSequence;
	}
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationManager : MonoBehaviour
{
	static VegetationManager _instance;
	public static VegetationManager Instance { get { return _instance; } }
	public readonly float activeRange = 17.5f;

	List<GameObject> foilage = new List<GameObject>();

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	private void Update()
	{
		if (foilage.Count > 0)
		{
			for (int i = foilage.Count - 1; i >= 0; i--)
			{
				if (Vector3.Distance(foilage[i].transform.position, Camera.main.transform.position) < activeRange)
				{
					foilage[i].SetActive(true);
					foilage.RemoveAt(i);
				}
			}
		}
	}

	public void ReturnFoilage(GameObject foilageObject)
	{
		foilage.Add(foilageObject);
		foilageObject.SetActive(false);
	}
}

public static class SpotChecker
{
	public static bool IsVisible(Vector3 pos, Vector3 boundSize, Camera camera)
	{
		var bounds = new Bounds(pos, boundSize * 4);
		var planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, bounds);
	}
}

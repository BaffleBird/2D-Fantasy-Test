using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEdit : MonoBehaviour
{
	[SerializeField]TextMeshProUGUI textMesh = null;
	[SerializeField]Status status = null;

    // Start is called before the first frame update
    void Start()
    {
		textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (status)
		{
			textMesh.text = "" + status.height;
		}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform target = null;
	public Vector3 offset;
	public float smoothing = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
		if (target != null)
		{
			Vector3 targetPosition = target.position + offset;
			Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.fixedDeltaTime);
			transform.position = smoothPosition;
		}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foilage : MonoBehaviour
{
	Mesh mesh;
	CircleCollider2D circleCol;

	Vector2 entryDirection;
	Vector2 targetPoint;

	Vector2 currentPoint;
	Vector2 velocityVector;
	Vector2 accelerationVector;

	[SerializeField] float BEND_FACTOR = 2f;
	[SerializeField] float SPRING_CONSTANT = 5;
	[SerializeField] float DAMPENING = 2f;

	private void Awake()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		circleCol = GetComponent<CircleCollider2D>();
	}

	private void Update()
	{
		if (Vector3.Distance(transform.position, Camera.main.transform.position) > VegetationManager.Instance.activeRange)
		{
			VegetationManager.Instance.ReturnFoilage(gameObject);
		}

		//Constantly move towards targetPoint - Redfine those setVert functions (multiply y values by 1.414f?
		//Perhaps split up rebound and bending states?
		//Find a way to diminish total velocity over time
		//Add Sway by adding a Sin function to the x of said point
		accelerationVector = (SPRING_CONSTANT * (currentPoint - targetPoint)) + velocityVector * DAMPENING;
		velocityVector -= (accelerationVector * Time.deltaTime);
		currentPoint += velocityVector * Time.deltaTime;

		velocityVector.x += Mathf.Sin(Time.time + transform.position.y + transform.position.x) * Time.deltaTime * DAMPENING;

		if (accelerationVector.magnitude < 0.00005f)
			setVertOffset(targetPoint);
		else
			setVertOffset(currentPoint); // Try lerping here

	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		//Get the inital direction and distance of entry
		var start = (Vector2)col.transform.position - col.offset;
		var destination = (Vector2)circleCol.transform.position + circleCol.offset;
		entryDirection = (destination - start).normalized;
	}

	private void OnTriggerStay2D(Collider2D col)
	{
		//Calculate a point Based on the radius to the center of the circle collider
		//Where the player stands determines how and where things should bend
		//Map that to -1 to 1 for radial standardization
		//Radius to zero is the true range
		//Distance is the value
		//Keep it from crossing zero
		var start = (Vector2)col.transform.position - col.offset;
		var destination = (Vector2)circleCol.transform.position + circleCol.offset;
		float offset = Vector2.Distance(start, destination);
		offset = MathHelper.ScaleValue(circleCol.radius - offset, 0, circleCol.radius, 0, 1);

		var newTarget = Vector2.zero + (destination - start).normalized * offset;
		if (newTarget != targetPoint) targetPoint = newTarget;

		//if (Mathf.Sign(targetPoint.x) != Mathf.Sign(entryDirection.x)) targetPoint.x = 0;
		//if (Mathf.Sign(targetPoint.y) > 0) targetPoint.y *= -1;
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		//Reset
		targetPoint = Vector2.zero;
	}

	//Hard set position of vertices
	void setVertHorizontalOffset(float offset)
	{
		float newOffset = (offset * BEND_FACTOR) / transform.localScale.x;
		Vector3[] verts = mesh.vertices;

		verts[2].x = -0.5f + newOffset;
		verts[3].x = 0.5f + newOffset;

		mesh.vertices = verts;
	}

	
	void setVertOffset(Vector2 targetPos)
	{
		Vector3[] verts = mesh.vertices;
		float minHeight = 0.1f;

		float newOffset = targetPos.x * BEND_FACTOR * 2;
		verts[2].x = -0.5f + newOffset;
		verts[3].x = 0.5f + newOffset;

		newOffset = Mathf.Abs(targetPos.y * BEND_FACTOR);
		verts[2].y = 0.5f - newOffset;
		verts[3].y = 0.5f - newOffset;

		if (verts[2].y < minHeight)
		{
			verts[2].y = minHeight;
			verts[3].y = minHeight;
		}

		mesh.vertices = verts;
	}
}

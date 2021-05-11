using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	[SerializeField]Status _characterStatus = null;
	public Status CharacterStatus { get { return _characterStatus; } }

	[SerializeField]InputHandler _inputSource = null;
	public InputHandler InputSource { get { return _inputSource; } }

	[SerializeField]SpriteController _entitySpriteController = null;
	public SpriteController EntitySprite { get { return _entitySpriteController; } }

	[SerializeField] AnimatorRadio _animatorRadio = null;
	public AnimatorRadio EntityRadio{ get { return _animatorRadio; } }

	[SerializeField]Rigidbody2D _entityRigidbody = null;
	public Rigidbody2D EntityRigidbody { get { return _entityRigidbody; } }

	[SerializeField]Transform _entityTransform = null;
	public Transform EntityTransform { get { return _entityTransform; } }

	BoxCollider2D _entityCollider = null;
	public BoxCollider2D EntityCollider { get { return _entityCollider; } }

	protected State currentState = null;
	protected Dictionary<string, State> stateList = new Dictionary<string, State>();

	enum RampDirection { None, Up, Left, Right };
	RampDirection rampDirection = RampDirection.None;

	protected virtual void Awake()
    {
		//Grab Components
		if (_entityRigidbody == null)
			_entityRigidbody = GetComponent<Rigidbody2D>();
		_entityTransform = GetComponent<Transform>();
		_entitySpriteController = GetComponent<SpriteController>();
		_entityCollider = GetComponent<BoxCollider2D>();

		//Reset data (For scene prototyping. A real game would probably have load/save functions)
		CharacterStatus.height = 0;
		CharacterStatus.adjustHeight = 0;

		//Add all possible States to the dictionary
		//Set a Default State
	}

	protected void Update()
    {
		if (currentState != null)
			currentState.UpdateState();
	}


	protected void FixedUpdate()
	{
		if (currentState != null)
			currentState.FixedUpdateState();

		//Get and apply movement values
		Vector2 motionUpdate = currentState.MotionUpdate();
		
		//Alter movement based on environment
		LayerMask mask = LayerMask.GetMask("Wall");
		RaycastHit2D rampHit = Physics2D.BoxCast(EntityTransform.position, EntityCollider.bounds.size, 0, Vector2.up * motionUpdate.normalized, motionUpdate.magnitude * 2.5f, mask);
		//BoxDebug(EntityTransform.position, EntityCollider.bounds.size, Vector2.up * motionUpdate.normalized, motionUpdate.magnitude * 2.5f);

		switch (rampDirection)
		{
			case RampDirection.Up:
				CharacterStatus.adjustHeight += motionUpdate.y;
				motionUpdate.y = 0;
				break;
			case RampDirection.Left:
				if (rampHit.collider != null)
				{
					motionUpdate.y = 0;;
				}
				CharacterStatus.adjustHeight -= motionUpdate.x * .75f;
				break;
			case RampDirection.Right:
				if (rampHit.collider != null)
				{
					motionUpdate.y = 0;
				}
				CharacterStatus.adjustHeight += motionUpdate.x * .75f;
				break;
		}
		EntityRigidbody.MovePosition(EntityRigidbody.position + motionUpdate);
	}

	void BoxDebug(Vector2 origin, Vector2 size, Vector2 direction, float distance)
	{
		Vector2[] corners = new Vector2[4];
		corners[0] = Vector2.Scale((size * 0.5f), new Vector2(-1,1)) + origin + (direction.normalized * distance);
		corners[1] = Vector2.Scale((size * 0.5f), new Vector2(1, 1)) + origin + (direction.normalized * distance);
		corners[2] = Vector2.Scale((size * 0.5f), new Vector2(1, -1)) + origin + (direction.normalized * distance);
		corners[3] = Vector2.Scale((size * 0.5f), new Vector2(-1, -1)) + origin + (direction.normalized * distance);

		Debug.DrawLine(corners[0], corners[1]);
		Debug.DrawLine(corners[1], corners[2]);
		Debug.DrawLine(corners[2], corners[3]);
		Debug.DrawLine(corners[3], corners[0]);
	}

	private void LateUpdate()
	{
		Vector3 pos = EntityRigidbody.transform.position;
		pos.z = pos.y;
		EntityRigidbody.transform.position = pos;

		pos.y = EntityRigidbody.transform.position.y + CharacterStatus.height + CharacterStatus.adjustHeight;
		EntityTransform.position = pos;
	}

	public void SwitchState(string stateName)
	{
		if (stateList.ContainsKey(stateName))
			SwitchState(stateList[stateName]);
		else
			Debug.LogError("This state machine does not contain the state: " + stateName);
	}

	protected void SwitchState(State newState)
	{
		if (newState != null)
		{
			currentState.EndState();
			currentState = newState;
			currentState.StartState();
		}
	}


	private void OnTriggerStay2D(Collider2D collision)
	{
		//If colliding into a Ramp at point, set motion behavior
		if (rampDirection == RampDirection.None && collision.OverlapPoint(EntityTransform.position))
		{
			switch (collision.tag)
			{
				case "VerticalRamp":
					rampDirection = RampDirection.Up;
					break;
				case "LeftRamp":
					rampDirection = RampDirection.Left;
					break;
				case "RightRamp":
					rampDirection = RampDirection.Right;
					break;
			}
		}
		else if (rampDirection != RampDirection.None && !collision.OverlapPoint(EntityTransform.position))
		{
			if (rampDirection == RampDirection.Up)
				CharacterStatus.adjustHeight = MathHelper.SnapValue(CharacterStatus.adjustHeight, collision.bounds.size.y);
			else
				CharacterStatus.adjustHeight = MathHelper.SnapValue(CharacterStatus.adjustHeight, 0.36f);

			rampDirection = RampDirection.None;
			CharacterStatus.height += CharacterStatus.adjustHeight;
			CharacterStatus.adjustHeight = 0;
		}
	}
}

public abstract class State
{
	protected StateMachine stateMachine;

	public State(StateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
	}

	public abstract void StartState();

	public abstract void UpdateState();

	public abstract void FixedUpdateState();

	public abstract Vector3 MotionUpdate();

	public abstract void EndState();
}

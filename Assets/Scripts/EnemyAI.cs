using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D rb;
	[SerializeField]
	private PlayerDetector playerDetector;

	[Header("Pathfinding")]

	[SerializeField]
	private Transform target;
	[SerializeField]
	private float pathUpdateSeconds = 0.5f;
	[SerializeField]
	private float nextWaypointDistance = 0.5f;

	private float distance;
	private Path path;
	private int currentWaypoint = 0;
	private Seeker seeker;

	public UnityEvent OnShoot = new UnityEvent();
	public UnityEvent<Vector2> OnMoveBody = new UnityEvent<Vector2>();
	public UnityEvent<Vector2> OnMoveTurret = new UnityEvent<Vector2>();

	private void Awake()
	{
		rb = GetComponentInChildren<Rigidbody2D>();
		target = GameObject.Find("Player").transform;
		seeker = GetComponentInChildren<Seeker>();
		InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
	}

	void Update()
	{
		if (playerDetector.Target != null && playerDetector.TargetVisible)
		{
			OnMoveBody?.Invoke(Vector2.zero);
			OnMoveTurret?.Invoke(playerDetector.Target.position);
			OnShoot?.Invoke();
		}
		else
		{
			// pathfinding stuff
			if (path == null || target == null)
			{
				return;
			}

			if (currentWaypoint >= path.vectorPath.Count)
			{
				return;
			}

			distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

			if (distance < nextWaypointDistance)
			{
				currentWaypoint++;
			}

			Vector2 movementVector = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);
			var dotProduct = Vector2.Dot(transform.up, movementVector.normalized);

			if (dotProduct < 0.98f)
			{
				var crossProduct = Vector3.Cross(transform.up, movementVector.normalized);
				int rotationResult = crossProduct.z >= 0 ? -1 : 1;
				OnMoveBody?.Invoke(new Vector2(rotationResult, 1));
			}
			else
			{
				OnMoveBody?.Invoke(Vector2.up);
			}
		}
	}



	// PATHFINDING STUFF

	private void UpdatePath()
	{
		if (target != null && seeker.IsDone())
		{
			seeker.StartPath(rb.position, target.position, OnPathComplete);
		}
	}

	private void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
	}
}

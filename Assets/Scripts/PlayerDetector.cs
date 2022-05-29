using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
	[Range(1, 15)]
	[SerializeField]
	private float targetRange = 5;
	[SerializeField]
	private float checkFrequency = 0.1f;
	[SerializeField]
	private Transform target;
	[SerializeField]
	private LayerMask playerLM;
	[SerializeField]
	private LayerMask visibilityLM;

	[field: SerializeField]
	public bool TargetVisible { get; private set;  }
	public Transform Target
	{
		get => target;
		set
		{
			target = value;
			TargetVisible = false;
		}
	}

	public float TargetRange { get => targetRange; set => targetRange = value; }

	private void Start()
	{
		StartCoroutine(DetectionCoroutine());
	}

	private void Update()
	{
		if (Target != null)
		{
			TargetVisible = CheckTargetVisibility();
		}
	}

	private bool CheckTargetVisibility()
	{
		var result = Physics2D.Raycast(transform.position, Target.position - transform.position, TargetRange, visibilityLM);
		if (result.collider != null)
		{
			return (playerLM & (1 << result.collider.gameObject.layer)) != 0;
		}
		return false;
	}


	private void DetectTarget()
	{
		if (Target == null)
		{
			CheckIfPlayerInRange();
		}
		else if (Target != null)
		{
			DetectIfOutOfRange();
		}
	}

	private void DetectIfOutOfRange()
	{
		if (Target == null || Target.gameObject.activeSelf == false || Vector2.Distance(transform.position, Target.position) > TargetRange)
		{
			Target = null;
		}
	}

	private void CheckIfPlayerInRange()
	{
		Collider2D collision = Physics2D.OverlapCircle(transform.position, TargetRange, playerLM);
		if (collision != null)
		{
			Target = collision.transform;
		}
	}

	IEnumerator DetectionCoroutine()
	{
		yield return new WaitForSeconds(checkFrequency);
		DetectTarget();
		StartCoroutine(DetectionCoroutine());
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, TargetRange);
	}
}

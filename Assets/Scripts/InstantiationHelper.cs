using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationHelper : MonoBehaviour
{
	public GameObject goPrefab;
	

	public void Create()
	{
		GameObject go = Instantiate(goPrefab);
		go.transform.position = transform.position;
	}
}

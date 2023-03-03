using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisObject : MonoBehaviour
{
	public void SetRotation(float z)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
	}
}

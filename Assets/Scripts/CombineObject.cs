using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineObject : MonoBehaviour
{
    public virtual void SetRotation(float x, float y, float z)
	{
		transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
	}
}

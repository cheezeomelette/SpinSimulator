using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAxisObject : MonoBehaviour
{
    public void SetRotation(float x)
	{
		transform.rotation = Quaternion.Euler(new Vector3(x, 0, 0));
	}
}
 
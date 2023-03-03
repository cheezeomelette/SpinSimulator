using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    Camera cam;

	private void Start()
	{
		cam = GetComponent<Camera>();

		cam.fieldOfView = 60 * cam.rect.height;
	}
}

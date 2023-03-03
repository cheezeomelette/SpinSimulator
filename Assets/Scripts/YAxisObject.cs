using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisObject : MonoBehaviour
{
    public void SetRotation(float y)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, y, 0));
    }
}

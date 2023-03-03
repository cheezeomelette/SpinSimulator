using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisManager : MonoBehaviour
{
	[SerializeField] AngleDisplay angleDisplay;
	[SerializeField] CombineObject combineObject;
	[SerializeField] XAxisObject xAxisObject;
	[SerializeField] YAxisObject yAxisObject;
	[SerializeField] ZAxisObject zAxisObject;
	float mXAxis;
	float mYAxis;
	float mZAxis;

    public float xAxis
	{
		get
		{
			return mXAxis;
		}
		set
		{
			mXAxis = value;
			angleDisplay.SetAxisXText(mXAxis);
			xAxisObject.SetRotation(mXAxis);
			combineObject.SetRotation(mXAxis, mYAxis, mZAxis);
		}
	}
	public float yAxis
	{
		get
		{
			return mYAxis;
		}
		set
		{
			mYAxis = value;
			angleDisplay.SetAxisYText(mYAxis);
			yAxisObject.SetRotation(mYAxis);
			combineObject.SetRotation(mXAxis, mYAxis, mZAxis);
		}
	}
	public float zAxis
	{
		get
		{
			return mZAxis;
		}
		set
		{
			mZAxis = value;
			angleDisplay.SetAxisZText(mZAxis);
			zAxisObject.SetRotation(mZAxis);
			combineObject.SetRotation(mXAxis, mYAxis, mZAxis);
		}
	}

	public void SetAllAxis(float x, float y, float z)
	{
		xAxis = x;
		yAxis = y;
		zAxis = z;
	}
}

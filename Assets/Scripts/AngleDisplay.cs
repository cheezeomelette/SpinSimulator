using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AngleDisplay : MonoBehaviour
{
    [SerializeField] AxisManager axisManager;

    [SerializeField] Text xAxisText;
    [SerializeField] Text yAxisText;
    [SerializeField] Text zAxisText;

    public void SetAxisXText(float xValue)
    {
        xAxisText.text = xValue.ToString();
    }
    public void SetAxisYText(float yValue)
    {
        yAxisText.text = yValue.ToString();
    }
    public void SetAxisZText(float zValue)
    {
        zAxisText.text = zValue.ToString();
    }
}

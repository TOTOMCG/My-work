using UnityEngine;
using UnityEngine.UI;

public class TimeScaleUI : MonoBehaviour
{
    public Text timeScaleText;
    
    void Update()
    {
        timeScaleText.text = "Time Scale: " + Time.timeScale.ToString("F2");
    }
}
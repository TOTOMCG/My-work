using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    public Slider timeScaleSlider;
    public Text timeScaleText;

    void Start()
    {
        timeScaleSlider.value = Time.timeScale;
        timeScaleSlider.onValueChanged.AddListener(UpdateTimeScale);
    }

    void UpdateTimeScale(float value)
    {
        Time.timeScale = value;
        timeScaleText.text = "Time Scale: " + value.ToString("F2");
    }
}
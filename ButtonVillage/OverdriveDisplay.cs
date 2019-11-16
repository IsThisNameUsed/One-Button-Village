using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Display overdrive gauge
public class OverdriveDisplay : MonoBehaviour
{
    // Public shiny parameters
    public Color StartColor = Color.green;
    public Color EndColor = Color.red;
    public float AnimationLength = 0.4f;

    // Image to fill
    private Image _overdriveGauge;
    
	void Awake ()
    {
        _overdriveGauge = transform.Find("Overdrive").GetComponent<Image>();
        _overdriveGauge.color = StartColor;
        _overdriveGauge.fillAmount = 1f;
	}
	
    // Set gauge by value parameter
	internal void SetGauge(float value)
    {
        // Tween is used to change value over time, if the function is called again, the old tween is deleted
        if(gameObject != null) gameObject.Tween("OverDriveFill", _overdriveGauge.fillAmount, value, AnimationLength, TweenScaleFunctions.QuadraticEaseOut, t =>
        {
            _overdriveGauge.fillAmount = t.CurrentValue;
        });

        Color target = Color.Lerp(EndColor, StartColor, value);
        gameObject.Tween("OverDrivecolor", _overdriveGauge.color, target, AnimationLength, TweenScaleFunctions.QuadraticEaseOut, t =>
        {
            _overdriveGauge.color = t.CurrentValue;
        });
    }
}

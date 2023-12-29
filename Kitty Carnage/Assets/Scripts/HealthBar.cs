using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
	[SerializeField]
	private Gradient gradient;
    [SerializeField]
    private Image fill;

	// Awake is called before Start
	private void Awake()
	{
		slider = this.GetComponent<Slider>();
	}

    public void SetMaxHealth(float health)
    {
        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;

            fill.color = gradient.Evaluate(1f);
        }
    }

    public void SetHealth(float health)
    {
        if (slider != null)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(slider.normalizedValue);
		}
    }
}

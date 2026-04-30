using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hungerbar : MonoBehaviour
{
    public Slider hungerSlider;

    public float hunger;
    float maxHunger = 100f;

    void Start()
    {
        hunger = maxHunger;
    }

    // Update is called once per frame
    void Update()
    {
        hungerSlider.value = hunger;

        hunger -= 2f * Time.deltaTime;

        if (hunger >= 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                hunger -= 0.5f * Time.deltaTime;
            }
        }


    }
}
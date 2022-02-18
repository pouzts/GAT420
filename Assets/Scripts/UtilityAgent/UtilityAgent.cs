using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityAgent : Agent
{
    Need[] needs;
    [SerializeField] MeterUI meter;

    public float happiness
    {
        get 
        {
            float totalMotive = 0; 
            foreach (var need in needs)
            {
                totalMotive += need.motive;
            }

            return totalMotive / needs.Length;
        }
    }

    void Start()
    {
        needs = GetComponentsInChildren<Need>();
        meter.text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        meter.slider.value = happiness;
        meter.worldPosition = transform.position + Vector3.up * 2;
    }

/*    private void OnGUI()
    {
        Vector2 screen = Camera.main.WorldToScreenPoint(transform.position);

        GUI.color = Color.black;
        int offset = 0;
        foreach (var need in needs)
        {
            GUI.Label(new Rect(screen.x + 20, Screen.height - screen.y - offset, 300, 20), need.type.ToString() + ": " + need.motive);
            offset += 20;
        }
        //GUI.Label(new Rect(screen.x + 20, Screen.height - screen.y - offset, 300, 20), mood.ToString());
    }*/
}

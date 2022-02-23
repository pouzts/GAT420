using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtilityAgent : Agent
{
    [SerializeField] Perception perception;
    [SerializeField] MeterUI meter;
    
    Need[] needs;
    UtilityObject activeUtilityObject = null;

    public float happiness
    {
        get 
        {
            float totalMotive = 2; 
            foreach (var need in needs)
            {
                totalMotive -= need.motive;
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

        if (activeUtilityObject == null)
        {
            var gameObjects = perception.GetGameObjects();
            List<UtilityObject> utilityObjects = new List<UtilityObject>();

            foreach (var go in gameObjects)
            {
                if (go.TryGetComponent<UtilityObject>(out UtilityObject utilityObject))
                {
                    utilityObject.visible = true;
                    utilityObject.score = GetUtilityObjectScore(utilityObject);
                    utilityObjects.Add(utilityObject);
                }
            }
        }
    }

    private void LateUpdate()
    {
        meter.slider.value = happiness;
        meter.worldPosition = transform.position + Vector3.up * 3;
    }

    float GetUtilityObjectScore(UtilityObject utilityObject)
    {
        float score = 0;

        foreach (var effector in utilityObject.effectors)
        {
            Need need = GetNeedByType(effector.type);
            if (need != null)
            {
                float futureNeed = need.GetMotive(need.input + effector.change);
                score += need.motive - futureNeed;
            }
        }

        return score;
    }

    Need GetNeedByType(Need.Type type)
    {
        return needs.First(need => need.type == type);
    }
}

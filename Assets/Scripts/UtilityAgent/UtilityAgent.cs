using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtilityAgent : Agent
{
    [SerializeField] Perception perception;
    [SerializeField] MeterUI meter;
    
    const float MIN_SCORE = 0.2f;
    
    Need[] needs;
    UtilityObject activeUtilityObject = null;
    public bool isUsingUtilityObject { get { return activeUtilityObject != null; } }

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
                    if (utilityObject.score > MIN_SCORE) utilityObjects.Add(utilityObject);
                }
            }

            // set active utility object to the first utility object
            //activeUtilityObject = (utilityObjects.Count == 0) ? null : utilityObjects[0];

            // set the active utility object to highest utility object
            //activeUtilityObject = (utilityObjects.Count == 0) ? null : GetHighestUtilityObject(utilityObjects.ToArray());

            // set the active utility object to a random utility object
            activeUtilityObject = GetRandomUtilityObject(utilityObjects.ToArray());

            if (activeUtilityObject != null)
            {
                StartCoroutine(ExecuteUtilityObject(activeUtilityObject));
            }
        }
    }

    private void LateUpdate()
    {
        meter.slider.value = happiness;
        meter.worldPosition = transform.position + Vector3.up * 3;
    }

    IEnumerator ExecuteUtilityObject(UtilityObject utilityObject)
    {
        // go to location
        movement.MoveTowards(utilityObject.location.position);
        while (Vector3.Distance(transform.position, utilityObject.location.position) > 0.25)
        {
            Debug.DrawLine(transform.position, utilityObject.location.position);
            print(Vector3.Distance(transform.position, utilityObject.location.position));
            yield return null;
        }

        print("start effect");

        // start effect
        if (utilityObject.effect != null) utilityObject.effect.SetActive(true);

        // wait duration
        yield return new WaitForSeconds(utilityObject.duration);
        
        // stop effect
        if (utilityObject.effect != null) utilityObject.effect.SetActive(false);

        // apply object
        ApplyUtilityObject(utilityObject);

        activeUtilityObject = null;

        print("stop effect");

        yield return null;
    }

    void ApplyUtilityObject(UtilityObject utilityObject)
    {
        foreach (var effector in utilityObject.effectors)
        {
            Need need = GetNeedByType(effector.type);
            if (need != null)
            {
                need.input += effector.change;
                need.input = Mathf.Clamp(need.input, -1, 1);
            }
        }
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

    UtilityObject GetHighestUtilityObject(UtilityObject[] utilityObjects)
    {
        UtilityObject highestUtilityObject = null;
        float highestScore = MIN_SCORE;
        foreach (var utilityObject in utilityObjects)
        {
            float score = utilityObject.score;
            if (score > highestScore)
            {
                highestScore = score;
                highestUtilityObject = utilityObject;
            }
        }

        return highestUtilityObject;
    }

    UtilityObject GetRandomUtilityObject(UtilityObject[] utilityObjects)
    {
        // evaluate all utility objects
        float[] scores = new float[utilityObjects.Length];
        float totalScore = 0;
        for (int i = 0; i < utilityObjects.Length; i++)
        {
            scores[i] = utilityObjects[i].score;
            totalScore += utilityObjects[i].score;
        }

        // select random utility object based on score
        // the higher the score, the greater the chance of being random selected

        float random = Random.Range(0, totalScore);
        for (int i = 0; i < scores.Length; i++)
        {
            if (random < scores[i]) return utilityObjects[i];
            
            random -= scores[i];
        }

        return null;
    }
}

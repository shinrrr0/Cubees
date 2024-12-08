using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;


public class PlatformController : MonoBehaviour
{
    public List<Transform> route = new List<Transform>();
    public List<AnimationCurve> curves = new List<AnimationCurve>();

    public bool toSendSingal = true;
    public GameObject controller;

    private int currentTarget = 1;
    private float currentTime = 0f;

    void Start()
    {
        StartMoving();
    }

    void Update()
    {
        
    }

    public void StartMoving(){
        AnimationCurve currCurve;
        if (currentTarget == 0) currCurve = curves[curves.Count - 1];
        else currCurve = curves[currentTarget - 1];
        StartCoroutine(MovingCor(transform.position, route[currentTarget].position, currCurve.keys[currCurve.keys.Length - 1].time));
    }

    public void StopMoving(){

    }

    IEnumerator MovingCor(Vector3 startPoint, Vector3 endPoint, float totalTime){
        yield return null;
        currentTime += Time.deltaTime;

        AnimationCurve currCurve;
        if (currentTarget == 0) currCurve = curves[curves.Count - 1];
        else currCurve = curves[currentTarget - 1];

        transform.position = Vector3.Lerp(startPoint, endPoint, currCurve.Evaluate(currentTime));

        if (currentTime < totalTime){
            StartCoroutine(MovingCor(startPoint, endPoint, totalTime));
        }
        else{
            transform.position = endPoint;
            currentTime = 0;
            currentTarget++;
            if (currentTarget >= route.Count) currentTarget = 0;

            if (toSendSingal){
                CallContext callContext = new CallContext(gameObject);
                controller.GetComponent<Controller>().Action(callContext);
            }
        }
    }
}

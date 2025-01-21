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
    private Vector3 delta;

    private Vector3 startPoint, endPoint;
    private float totalTime;

    private bool isActive = false, wasBroken = false;

    public void StartMoving(){
        if (!wasBroken){
            AnimationCurve currCurve;
            if (currentTarget == 0) currCurve = curves[curves.Count - 1];
            else currCurve = curves[currentTarget - 1];
            startPoint = transform.position; endPoint = route[currentTarget].position;
            totalTime = currCurve.keys[currCurve.keys.Length - 1].time;
        }

        isActive = true;
        StartCoroutine(MovingCor());
    }

    public void StopMoving(){
        isActive = false;
    }

    IEnumerator MovingCor(){
        yield return null;
        currentTime += Time.deltaTime;

        AnimationCurve currCurve;
        if (currentTarget == 0) currCurve = curves[curves.Count - 1];
        else currCurve = curves[currentTarget - 1];

        transform.position = Vector3.Lerp(startPoint, endPoint, currCurve.Evaluate(currentTime));

        if (currentTime < totalTime && isActive){
            StartCoroutine(MovingCor());
        }
        else if (!isActive) wasBroken = true;
        else if (isActive){
            transform.position = endPoint;
            wasBroken = false;
            currentTime = 0;
            currentTarget++;
            if (currentTarget >= route.Count) currentTarget = 0;

            if (toSendSingal){
                Context context = new Context(_objectForActing: gameObject);
                controller.GetComponent<Controller>().Action(context);
            }
        }
    }
}

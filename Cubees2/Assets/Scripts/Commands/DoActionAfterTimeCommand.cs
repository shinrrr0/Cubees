using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;

public class DoActionAfterTimeCommand : MonoBehaviour, IControllerCommand
{
    public float time;
    public GameObject controller;
    [HideInInspector] public bool stopAction = false;

    public void Act(Context context){
        StartCoroutine(Delay(context));
    }

    IEnumerator Delay(Context context){
        yield return new WaitForSeconds(time);
        if (!stopAction){
            controller.GetComponent<Controller>().Action(context);
        }
        else stopAction = false;
    }
}

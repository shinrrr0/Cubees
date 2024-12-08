using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;

public class DoActionAfterTimeCommand : MonoBehaviour, IControllerCommand
{
    public float time;
    public GameObject controller;

    public void Act(Context context){
        StartCoroutine(Delay(context));
    }

    IEnumerator Delay(Context context){
        yield return new WaitForSeconds(time);
        CallContext callContext = new CallContext(context.objectForActing);
        controller.GetComponent<Controller>().Action(callContext);
    }
}

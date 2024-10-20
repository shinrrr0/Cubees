using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;

public class DoNActionsCommand : MonoBehaviour, IControllerCommand
{
    public int n;
    public GameObject controller;

    public void Act(Context context){
        CallContext callContext = new CallContext(context.objectForActing);
        controller.GetComponent<Controller>().changeA(1);
        for (int i = 0; i < n; i++) controller.GetComponent<Controller>().Action(callContext);
        controller.GetComponent<Controller>().changeA(-1);
    }
}

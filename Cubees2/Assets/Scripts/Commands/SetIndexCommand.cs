using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;


public class SetIndexCommand : MonoBehaviour, IControllerCommand
{
    public int n;
    public GameObject controller;
    public bool toDoAction = false;

    public void Act(Context context){
        controller.GetComponent<Controller>().setIndex(n);
        if (toDoAction) {
            controller.GetComponent<Controller>().Action(context);
        }
    }
}

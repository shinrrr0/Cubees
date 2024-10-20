using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;


public class SetIndexCommand : MonoBehaviour, IControllerCommand
{
    public int n;
    public GameObject controller;

    public void Act(Context context){
        controller.GetComponent<Controller>().setIndex(n);
    }
}

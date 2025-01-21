using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;

public class SetVariableCommand : MonoBehaviour
{
    public string variableName;
    public int variableValue;
    public GameObject controller;

    public void Act(Context context){
        controller.GetComponent<Controller>().variables.Add(variableName, variableValue);
    }
}

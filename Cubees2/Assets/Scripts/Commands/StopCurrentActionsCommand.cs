using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;

public class StopCurrentActionsCommand : MonoBehaviour, IControllerCommand
{
    public List<GameObject> actions = new List<GameObject>();

    public void Act(Context context){
        foreach (GameObject action in actions) action.GetComponent<DoActionAfterTimeCommand>().stopAction = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;

public class MovePlatformCommand : MonoBehaviour, IControllerCommand
{
    public GameObject platform;
    public enum _type {start=1, stop=2};
    public _type whatToDo;

    public void Act(Context context){
        if (whatToDo == _type.start) platform.GetComponent<PlatformController>().StartMoving();
        else {platform.GetComponent<PlatformController>().StopMoving();}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;

public class MovePlatformCommand : MonoBehaviour, IControllerCommand
{
    public GameObject platform;

    public void Act(Context context){
        CallContext callContext = new CallContext(context.objectForActing);
        platform.GetComponent<PlatformController>().StartMoving();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;


public class SetPlatformACtiveCommand : MonoBehaviour, IControllerCommand
{
    public GameObject platform;
    public bool doItActive;

    public void Act(Context context){
        if (doItActive) {
            if (!platform.GetComponent<MovePlatforms>().isMoving) {
                platform.GetComponent<MovePlatforms>().StartCoroutine("Move");
            }
            platform.GetComponent<MovePlatforms>().active = true;
        }
        else {
            platform.GetComponent<MovePlatforms>().active = false;
        }
    }
}

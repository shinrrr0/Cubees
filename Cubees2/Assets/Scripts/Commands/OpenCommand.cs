using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;


public class OpenCommand : MonoBehaviour, IControllerCommand
{
    public List<GameObject> doors;

    public void Act(Context context){
        foreach (GameObject door in doors) door.GetComponent<DoorController>().Open();
    }
}

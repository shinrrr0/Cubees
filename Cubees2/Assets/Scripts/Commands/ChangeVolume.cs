using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandsNamespace;
using DataSpace;

public class ChangeVolume : MonoBehaviour, IControllerCommand
{
    [Range(0,1)] public float change_to = 1;
    private JsonFile save = new JsonFile();

    public void Act(Context context) {
        GameObject Player = context.objectForActing;
        AudioListener.volume = change_to;
        save.changeVolumeValue(change_to);
    }
}

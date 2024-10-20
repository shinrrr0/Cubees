using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using CommandsNamespace;

public class TeleportCommand : MonoBehaviour, IControllerCommand
{
    public enum TypeOfTeleportation { InScene, ToAnotherScene };
    public TypeOfTeleportation type;

    [HideInInspector] public Transform nextPosition;
    [HideInInspector] public string nextScene;

    [CustomEditor(typeof(TeleportCommand))]
    public class TeleportationDetails : Editor
    {
        public override void OnInspectorGUI(){
            base.OnInspectorGUI();
            TeleportCommand teleportCommand = (TeleportCommand)target;

            if (teleportCommand.type == TypeOfTeleportation.InScene) 
                teleportCommand.nextPosition = EditorGUILayout.ObjectField("Next position", teleportCommand.nextPosition, typeof(Transform), true) as Transform;
            if (teleportCommand.type == TypeOfTeleportation.ToAnotherScene) 
                teleportCommand.nextScene = EditorGUILayout.TextField("Next scene", teleportCommand.nextScene);
        }
    }

    public void Act(Context context){
        GameObject objectToTeleport = context.objectForActing;
        if (type == TypeOfTeleportation.InScene && objectToTeleport.tag == "cube"){
            objectToTeleport.GetComponent<Moving>().StopMoving(nextPosition.position, 0); 
        }

        else if (type == TypeOfTeleportation.InScene && objectToTeleport.tag == "clone"){
            // objectToTeleport.GetComponent<CloneControll>().StopMoving(nextPosition.position, 0);
            objectToTeleport.transform.position = nextPosition.position;
        }

        else if (type == TypeOfTeleportation.ToAnotherScene) SceneManager.LoadScene(nextScene);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CommandsNamespace;


namespace CommandsNamespace {
    public struct Context{
        public GameObject objectForActing;

        public Context(GameObject _objectForActing) { objectForActing = _objectForActing; }
    }

    public struct CallContext{
        public GameObject causingObject;

        public CallContext(GameObject _causingObject) { causingObject = _causingObject; }
    }
}


public class Controller : MonoBehaviour
{

    public List<GameObject> commands = new List<GameObject>();
	[HideInInspector] public int a = 0;

    public void Action(CallContext callContext){
        Context context = new Context(callContext.causingObject);
        commands[a].GetComponent<IControllerCommand>().Act(context);
        a++;
        if (a >= commands.Count) a = 0;
    }

    public void changeA(int n) {
        a += n;
    }

    public void setIndex(int n){
        a = n;
    }

    // public void Action() {
    //     string[] arr = ReadCommand(commands[a]).ToArray();
    //     switch(arr[0]) {
    //         case "Open": Open(arr[1]); break;
    //         case "Close": Close(arr[1]); break;
    //         case "SetActivePlatform": SetActivePlatform(arr[1], arr[2]); break;
    //         case "Pass": Pass(); break;
    //         case "Transfer": Transfer(Convert.ToInt32(arr[1]), Convert.ToInt32(arr[2])); break;
    //         case "IfIsActive": IfIsActive(arr[1..arr.Length]); break;
    //     }
    //     a++;
    //     if (a >= commands.Count) a = 0;
    // }

    // private List<string> ReadCommand(string s) {
    //     List<string> commands = new List<string>();
    //     string argument = "";
    //     bool a = false;
    //     for (int i = 0; i < s.Length; i++) {
    //         if (s[i] == '(') {a = true; continue;}
    //         else if (s[i] == ')') {a = false; commands.Add(argument); argument = ""; continue;}

    //         if (a) argument += s[i];
    //     }
    //     return commands;
    // }

    // void Pass() {}

    // void SetActivePlatform(string name, string _bool) {
    // 	if (_bool == "true") {
    // 		if (!GameObject.Find(name).GetComponent<MovePlatforms>().isMoving) {
    // 			GameObject.Find(name).GetComponent<MovePlatforms>().StartCoroutine("Move");
    // 		}
    // 		GameObject.Find(name).GetComponent<MovePlatforms>().active = true;
    // 	}
    // 	else if (_bool == "false") {
    // 		GameObject.Find(name).GetComponent<MovePlatforms>().active = false;
    // 	}
    // }

    // void Transfer(int n, int count) {
    //     a = n;
    //     for (int i = 0; i < count; i++) Action();
    //     a--;
    // }

    // void Open(string name) {
    //     GameObject.Find(name).GetComponent<DoorController>().Open();
    // }

    // void Close(string name) {
    //     GameObject.Find(name).GetComponent<DoorController>().Close();
    // }

    // void IfIsActive(string[] arr) {
    //     bool flag = true;
    //     foreach (string i in arr){
    //         if (!GameObject.Find(i).GetComponent<ButtonController>().active) flag = false;
    //     }
    //     if (flag) {a++; Action();}
    //     else {a += 2; Action(); a--;}

    //     if (a >= commands.Count) a = 0;
    // }
}

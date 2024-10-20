using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CommandsNamespace;


public class ButtonController : MonoBehaviour
{
    public GameObject controller;
    [SerializeField] AnimationCurve changing;
    [HideInInspector] public float speed;
    [HideInInspector] public bool active = false;
    public float inDelay;

    public enum filter {cube = 1, clone = 2, cube_and_clone = 3};
    public filter typeOfFilter;

    public bool doActivateEnter = false;
    public bool doActivateExit = false;

    public AudioSource audio;

    private float currentTime, totalTime;
    private Renderer rend;
    private Color startColor = new Color(0, 1, 1, 1), endColor = new Color(1, 1, 0.5f, 1);

    void Start(){
        totalTime = changing.keys[changing.keys.Length - 1].time;
        rend = GetComponent<Renderer>();
    }


    void OnTriggerEnter(Collider collision) {
        if (typeOfFilter == filter.cube && collision.gameObject.tag != "cube") return;
        if (typeOfFilter == filter.clone && collision.gameObject.tag != "clone") return;
        if (collision.gameObject.tag == "clone" && !collision.gameObject.GetComponent<CloneControll>().isMoving) return;

        active = true;
        if (audio) audio.Play();
        StartCoroutine(ActivationCor(inDelay, collision));
        // if (doActivateEnter) controller.GetComponent<Controller>().Action();
        currentTime = 0;
        startColor = new Color(0, 1, 1, 1); endColor = new Color(1, 1, 0.5f, 1);
        StartCoroutine("ChangeColor");
    }

    void OnTriggerExit(Collider collision) {
        if (typeOfFilter == filter.cube && collision.gameObject.tag != "cube") return;
        if (typeOfFilter == filter.clone && collision.gameObject.tag != "clone") return;
        if (collision.gameObject.tag == "clone" && !collision.gameObject.GetComponent<CloneControll>().isMoving) return;
        
        active = false;
        CallContext callContext = new CallContext(collision.gameObject);
        if (doActivateExit) controller.GetComponent<Controller>().Action(callContext);
        currentTime = 0;
        startColor = new Color(1, 1, 0.5f, 1); endColor = new Color(0, 1, 1, 1);
        StartCoroutine("ChangeColor");
    }

    IEnumerator ActivationCor(float time, Collider collision) {
        yield return new WaitForSeconds(time);
        CallContext callContext = new CallContext(collision.gameObject);
        if (doActivateEnter && active) controller.GetComponent<Controller>().Action(callContext);
    }

    void OpenTheDoor(GameObject door) => door.GetComponent<DoorController>();

    IEnumerator ChangeColor()
    {
        yield return null;
        currentTime += Time.deltaTime * speed;
        rend.material.color = Color.Lerp(startColor, endColor, changing.Evaluate(currentTime));
        if (currentTime < totalTime) StartCoroutine("ChangeColor");
    }
}
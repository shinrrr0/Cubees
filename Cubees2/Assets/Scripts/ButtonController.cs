using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CommandsNamespace;
using ColorsNamespace;


public class ButtonController : MonoBehaviour
{
    public GameObject controller;
    [SerializeField] AnimationCurve changing;
    public float speed;
    [HideInInspector] public bool active = false;
    public float inDelay;

    public enum filter {cube = 1, clone = 2, cube_and_clone = 3};
    public filter typeOfFilter;

    public bool doActivateEnter = false;
    public bool doActivateExit = false;

    public AudioSource audio;
    private Renderer rend;

    private ChangingColor color;

    void Start(){
        rend = GetComponent<Renderer>();

        color = gameObject.AddComponent<ChangingColor>();
        color.SetParameters(rend.material.color, new Color(1, 1, 0.5f, 1), changing);
    }


    void OnTriggerEnter(Collider collision) {
        if (typeOfFilter == filter.cube && collision.gameObject.tag != "cube") return;
        if (typeOfFilter == filter.clone && collision.gameObject.tag != "clone") return;
        if (collision.gameObject.tag == "clone" && !collision.gameObject.GetComponent<CloneControll>().isMoving) return;

        active = true;
        if (audio) audio.Play();
        StartCoroutine(ActivationCor(inDelay, collision));
        color.ChangeColorForOpposite(gameObject);
    }

    void OnTriggerExit(Collider collision) {
        if (typeOfFilter == filter.cube && collision.gameObject.tag != "cube") return;
        if (typeOfFilter == filter.clone && collision.gameObject.tag != "clone") return;
        if (collision.gameObject.tag == "clone" && !collision.gameObject.GetComponent<CloneControll>().isMoving) return;

        active = false;
        CallContext callContext = new CallContext(collision.gameObject);
        if (doActivateExit) controller.GetComponent<Controller>().Action(callContext);
        color.ChangeColorForOpposite(gameObject);
    }

    IEnumerator ActivationCor(float time, Collider collision) {
        yield return new WaitForSeconds(time);
        CallContext callContext = new CallContext(collision.gameObject);
        if (doActivateEnter && active) controller.GetComponent<Controller>().Action(callContext);
    }

    void OpenTheDoor(GameObject door) => door.GetComponent<DoorController>();
}


namespace ColorsNamespace{
    public class ChangingColor : MonoBehaviour
    {
        private Color firstColor, secondColor;
        private AnimationCurve curve;
        private int currentCoroutineIndex = 0;
        private float currentTime, totalTime;

        public ChangingColor(){}

        public ChangingColor(Color startColor, Color endColor, AnimationCurve _curve) => SetParameters(startColor, endColor, _curve);

        public void SetParameters(Color startColor, Color endColor, AnimationCurve _curve){
            firstColor = startColor; secondColor = endColor;
            curve = _curve;
            totalTime = curve.keys[curve.keys.Length - 1].time;
            currentTime = totalTime;
        }

        public void ChangeColorForOpposite(GameObject objectToChange){
            currentCoroutineIndex++;
            currentTime = Mathf.Clamp(totalTime - currentTime, 0, totalTime);
            StartCoroutine(ChangingColorCoroutine(currentCoroutineIndex, objectToChange, firstColor, secondColor));
            (firstColor, secondColor) = (secondColor, firstColor);
        }

        IEnumerator ChangingColorCoroutine(int index, GameObject objectToChange, Color from, Color to){
            yield return null;
            if (index == currentCoroutineIndex && currentTime <= totalTime && currentTime >= 0){
                currentTime += Time.deltaTime;
                objectToChange.GetComponent<Renderer>().material.color = Color.Lerp(from, to, curve.Evaluate(currentTime));
                StartCoroutine(ChangingColorCoroutine(index, objectToChange, from, to));
            }
        }
    }
}

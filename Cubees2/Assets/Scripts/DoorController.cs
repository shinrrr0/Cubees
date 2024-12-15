using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] AnimationCurve changing;
    private Color startColor = new Color(0.55f, 0.86f, 0.94f, 0), endColor = new Color(0.55f, 0.86f, 0.94f, 1);
    private Renderer rend;
    private float currentTime, totalTime;
    private bool isChangingColor = false;
    private int factor = 1;

    void Start(){
        totalTime = changing.keys[changing.keys.Length - 1].time;
        currentTime = totalTime;
        rend = GetComponent<Renderer>();
    }

    public void Open(){
        factor = -1;
        if (!isChangingColor) StartCoroutine("OpenOrClose");
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void Close(){
        factor = 1;
        if (!isChangingColor) StartCoroutine("OpenOrClose");
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator OpenOrClose(){
        isChangingColor = true;
        yield return null;
        currentTime += factor * Time.deltaTime;
        rend.material.color = Color.Lerp(startColor, endColor, changing.Evaluate(currentTime));
        if (currentTime > 0 && currentTime < totalTime) StartCoroutine("OpenOrClose");
        else { isChangingColor = false; currentTime = Mathf.Clamp(currentTime, 0, totalTime); }
    }
}
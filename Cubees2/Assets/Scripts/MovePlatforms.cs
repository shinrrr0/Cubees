using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatforms : MonoBehaviour
{
	[SerializeField] private AnimationCurve _moving;
    // [HideInInspector] 
    public List<GameObject> objects = new List<GameObject>();
	public enum _filter {can_enter_when_move=1, cant_enter_when_move=2};
	public _filter filter;
	public enum _type {can_move_without_button=1, cant_move_without_button=2, cant_move_without_button_with_stops=3};
	public _type type; 
	public Transform posA, posB;
	public bool active = false, isMoving = false;
	private Vector3 buffer, postPosition, nowPosition, delta;
	private float currentTime = 0, totalTime;
	private bool isOnPlatform = false;
	private GameObject cube;
	public float speed = 1, stopTime = 1;

    void Start() {
    	if (filter == _filter.can_enter_when_move) GetComponent<BoxCollider>().enabled = false;
    	totalTime = _moving.keys[_moving.keys.Length - 1].time;
    	if (type == _type.can_move_without_button) {
    		active = true;
			StartCoroutine("Move");
    	}
	}
    IEnumerator Move()
    {
    	postPosition = transform.position;
        currentTime += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(posA.position, posB.position, _moving.Evaluate(currentTime));
        nowPosition = transform.position;
        if (isOnPlatform) {
        	delta = postPosition - nowPosition;
        	for (int i = 0; i < objects.Count; i++) objects[i].transform.position -= delta;
        }
       	if (currentTime >= totalTime) {
        	buffer = posA.position;
        	posA.position = posB.position;
        	posB.position = buffer;
        	currentTime = 0;
        	if (filter == _filter.cant_enter_when_move) GetComponent<BoxCollider>().enabled = false;
        	yield return new WaitForSeconds(stopTime);
       		isMoving = false;
       	}
       	else {
       		yield return null;
       		isMoving = true;
       	}
        if (!isOnPlatform && filter == _filter.cant_enter_when_move && isMoving) GetComponent<BoxCollider>().enabled = true;
        switch (type) {
        	case _type.can_move_without_button: StartCoroutine("Move"); break;
        	case _type.cant_move_without_button: if (active || isMoving) StartCoroutine("Move"); break;
        	case _type.cant_move_without_button_with_stops: if (active) StartCoroutine("Move"); break;
        }
    }
    void OnTriggerEnter(Collider collision) {
    	if (collision.transform.tag == "cube" || collision.transform.tag == "clone") {
    		cube = collision.gameObject;
    		isOnPlatform = true;
    		objects.Add(cube);
    		// collision.transform.SetParent(transform);
    	}
    }
    void OnTriggerExit(Collider collision) {
    	if (collision.transform.tag == "cube" || collision.transform.tag == "clone") {
    		// collision.transform.SetParent(null);
    		objects.Remove(collision.gameObject);
    		if (objects.Count == 0) isOnPlatform = false;
    	}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRecording : MonoBehaviour
{
    private bool last_marker;

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "cube") {
            collision.gameObject.GetComponent<Moving>().canEndRecording = collision.gameObject.GetComponent<Moving>().isRecording;
            collision.gameObject.GetComponent<Moving>().canStartRecording = false;
        }
    }  
    void OnTriggerExit(Collider collision) {
        if (collision.gameObject.tag == "cube") {
            collision.gameObject.GetComponent<Moving>().canEndRecording = true;
            collision.gameObject.GetComponent<Moving>().canStartRecording = true;
        }
    }  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Vector3 lastPos;
    private Vector3 moving;

    public bool a = false;


    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        moving = transform.position - lastPos;
        lastPos = transform.position;

        // if (a) print(moving);
    }

    public Vector3 GetMoving(){
        return moving;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DataSpace;

public class Moving : CommonClass
{
    public AnimationCurve movingCurve;
    [HideInInspector] public GameObject clone, allClones;

    [HideInInspector] public List<MovingParameters> moves = new List<MovingParameters>();
    [HideInInspector] public List<float> delays = new List<float>();
    [HideInInspector] public bool isRecording = false;
    [HideInInspector] public bool isMoving = false, canMove = true;
    [HideInInspector] public MovingParameters moveRight, moveLeft, moveForward, moveBackward, bufferAction, currentAction;
    [HideInInspector] public bool canStartRecording = true;
    [HideInInspector] public bool canEndRecording = true;

    public GameObject recPicture;

    private AudioSource sound;
    [HideInInspector] public float currentTime;
    private float totalTime, shareOfCurrentTime = 0, delay = 0;
    private bool haveBufferAction = false;
    private Vector3 startPosition;

    private JsonFile save = new JsonFile();
    private PlayerData data;

    void Start(){
        sound = gameObject.GetComponent<AudioSource>();
        moveRight.set(new Vector3 (1f, 0, 0), new Vector3(0, 0, -90f));
        moveLeft.set(new Vector3 (-1f, 0, 0), new Vector3(0, 0, 90f));
        moveForward.set(new Vector3 (0, 0, 1f), new Vector3(90f, 0, 0));
        moveBackward.set(new Vector3 (0, 0, -1f), new Vector3(-90f, 0, 0));
        totalTime = movingCurve.keys[movingCurve.keys.Length - 1].time;

        data = save.getPlayerData();
        AudioListener.volume = data.volume_value;
    }

    void Update()
    {
        if (canMove && !isMoving) {
            if (haveBufferAction) {
                move(bufferAction); 
                haveBufferAction = false;
            }
            else {
                if (!Input.GetKey(KeyCode.None)) {

                }
                if (Input.GetKey(KeyCode.RightArrow)) { move(moveRight); currentAction = moveRight; }
                else if (Input.GetKey(KeyCode.LeftArrow)) { move(moveLeft); currentAction = moveLeft; }
                else if (Input.GetKey(KeyCode.UpArrow)) { move(moveForward); currentAction = moveForward; }
                else if (Input.GetKey(KeyCode.DownArrow)) { move(moveBackward); currentAction = moveBackward; }
            }
        }

        if (isRecording && !isMoving) delay += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            if (!canStartRecording && !isRecording == true) {delay = 0;}
            else {
                isRecording = !isRecording;
            }
            if (isRecording)
            {
                // Start
                if (recPicture && !recPicture.GetComponent<RecUI>().isAnimationPlaying) recPicture.GetComponent<RecUI>().startChangingColor();
                moves = new List<MovingParameters>();
                delays = new List<float>();
                startPosition = transform.position;
                foreach (Transform i in allClones.transform) i.GetComponent<CloneControll>().ChangeColor();
            }
            else if (canEndRecording)
            {
                print(1);
                // Finish
                delays.Add(delay);
                GameObject newClone = Instantiate(clone, startPosition, Quaternion.identity, allClones.transform);
                newClone.GetComponent<CloneControll>().setParameters(moves, delays, movingCurve);
                newClone.GetComponent<Collider>().enabled = false;
                foreach (Transform i in allClones.transform) i.GetComponent<CloneControll>().ChangeColor();
                
            }
            delay = 0;
        }

        if (isMoving && shareOfCurrentTime >= 0.5f) {
            if (Input.GetKey(KeyCode.RightArrow) && (shareOfCurrentTime >= (currentAction == moveRight ? 0.75f : 0))) { bufferAction = moveRight; haveBufferAction = true;}
            else if (Input.GetKey(KeyCode.LeftArrow) && (shareOfCurrentTime >= (currentAction == moveLeft ? 0.75f : 0))) { bufferAction = moveLeft; haveBufferAction = true;} 
            else if (Input.GetKey(KeyCode.UpArrow) && (shareOfCurrentTime >= (currentAction == moveForward ? 0.75f: 0))) { bufferAction = moveForward; haveBufferAction = true;} 
            else if (Input.GetKey(KeyCode.DownArrow) && (shareOfCurrentTime >= (currentAction == moveBackward ? 0.75f : 0))) { bufferAction = moveBackward; haveBufferAction = true;}
            //if () // RECORD BUFFER HERE
        }
    }

    void move(MovingParameters par){
        RaycastHit hitForDown, hitFor, hitDown;
        if (Physics.Raycast(transform.position + par.getPos(), Vector3.down, out hitForDown, 1f, 1, QueryTriggerInteraction.Ignore) && !Physics.Raycast(transform.position, par.getPos(), out hitFor, 1f, 1, QueryTriggerInteraction.Ignore)) {
            Physics.Raycast(transform.position, Vector3.down, out hitDown, 1f, 1, QueryTriggerInteraction.Ignore);
            BlockController blockControllerDown = hitDown.transform.gameObject.GetComponent<BlockController>();
            BlockController blockControllerForDown = hitForDown.transform.gameObject.GetComponent<BlockController>();

            bool condition1 = blockControllerDown == null && (blockControllerForDown == null || blockControllerForDown.GetMoving().magnitude == 0);
            bool condition2 = blockControllerForDown == null && (blockControllerDown == null || blockControllerDown.GetMoving().magnitude == 0);
            bool condition3 = blockControllerDown && blockControllerForDown && blockControllerDown.GetMoving() == blockControllerForDown.GetMoving();

            if (!(condition1 || condition2 || condition3)) return;

            if (isRecording) {
                moves.Add(par);
                delays.Add(delay);
                delay = 0;
            }

            sound.Play();
            isMoving = true;

            StartCoroutine(MovingCor(new Vector3(0, 0, 0), par.getPos(),
             transform.rotation, Quaternion.Euler(par.getRot().x, par.getRot().y, par.getRot().z)));
        }
    }

    IEnumerator MovingCor(Vector3 deltaBackward, Vector3 deltaForward, Quaternion rot1, Quaternion rot2){
        if (currentTime >= totalTime || !isMoving){
            currentTime = 0;
            if (isMoving) transform.position = transform.position + deltaForward;
            isMoving = false;
            transform.position += new Vector3(0, GetHeight(movingCurve, totalTime) - GetHeight(movingCurve, currentTime - Time.deltaTime), 0);
            transform.rotation = rot1;
            yield break;
        }

        currentTime += Time.deltaTime;
        shareOfCurrentTime = currentTime / totalTime;

        Vector3 deltaPosition = transform.position;
        transform.position = Vector3.Lerp(transform.position + deltaBackward, transform.position + deltaForward, movingCurve.Evaluate(currentTime));
        deltaPosition -= transform.position;

        transform.position += new Vector3(0, GetHeight(movingCurve, currentTime) - GetHeight(movingCurve, currentTime - Time.deltaTime), 0);
        transform.rotation = Quaternion.Lerp(rot1, rot2, movingCurve.Evaluate(currentTime));

        yield return null;
        StartCoroutine(MovingCor(deltaBackward + deltaPosition, deltaForward + deltaPosition, rot1, rot2));
    }

    public void StopMoving() => isMoving = false;
    public void StopMoving(Vector3 newPosition, float time) => StartCoroutine(StopMovingCor(newPosition, time));

    IEnumerator StopMovingCor(Vector3 newPosition, float time){
        yield return new WaitForSeconds(time);
        isMoving = false;
        transform.position = newPosition;
    }

    public float getTotalTime() { return totalTime; }
}
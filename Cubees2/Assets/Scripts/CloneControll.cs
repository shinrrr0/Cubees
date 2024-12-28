using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneControll : CommonClass
{
    [HideInInspector] private AnimationCurve movingCurve;
    [SerializeField] private AnimationCurve changing;

    [SerializeField] public List<MovingParameters> moves = new List<MovingParameters>();
    [HideInInspector] public List<float> delays = new List<float>();
    [HideInInspector] public GameObject cube;
    [HideInInspector] public bool isMoving = false;
    private Color startColor = new Color(0.8f, 0.8f, 0.8f, 0.5f), endColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    private float currentTime = 0, totalTime, colorCurrentTime = 0, colorTotalTime;
    private Quaternion rot1, rot2;
    private int i = 0, factor = -1;
    private Vector3 startPosition;
    private Renderer rend;
    private bool isChangingColor = false;

    private Material opaqueMaterial, fadeMaterial;

    void Start(){
        colorTotalTime = changing.keys[changing.keys.Length - 1].time;
        colorCurrentTime = colorTotalTime;
        startPosition = transform.position;
        // startColor = new Color(1, 1, 1, 0.5f);
    }

    void Awake(){
        rend = GetComponent<Renderer>();
        opaqueMaterial = Resources.Load("Materials/CloneMaterials/Opaque") as Material;
        fadeMaterial = Resources.Load("Materials/CloneMaterials/Fade") as Material;
    }

    public void setParameters(List<MovingParameters> _moves, List<float> _delays, AnimationCurve _movingCurve){
        foreach (MovingParameters i in _moves) moves.Add(i);
        foreach (float i in _delays) delays.Add(i);
        movingCurve = _movingCurve;
        totalTime = movingCurve.keys[movingCurve.keys.Length - 1].time;
    }

    void OnEnable() {
        RaycastHit hit;
        if ((transform.position == cube.transform.position && !cube.GetComponent<Moving>().isRecording) || Physics.Raycast(transform.position + new Vector3(0, 1f, 0), Vector3.down, out hit, 1f)) {Destroy(gameObject);}
    }

    IEnumerator MakeDelay(){
        yield return new WaitForSeconds(delays[i]);
        if (!isMoving) yield break;
        if (i == moves.Count) Respawn();
        else Move(moves[i]);
    }

    IEnumerator MovingCor(Vector3 deltaBackward, Vector3 deltaForward, Quaternion rot1, Quaternion rot2){
        if (currentTime >= totalTime || !isMoving){
            currentTime = 0;
            transform.position = transform.position + deltaForward;
            transform.position += new Vector3(0, GetHeight(movingCurve, currentTime) - GetHeight(movingCurve, currentTime - Time.deltaTime), 0);
            transform.rotation = rot1;
            i++;
            StartCoroutine(MakeDelay());
            yield break;
        }

        currentTime += Time.deltaTime;

        Vector3 deltaPosition = transform.position;
        transform.position = Vector3.Lerp(transform.position + deltaBackward, transform.position + deltaForward, movingCurve.Evaluate(currentTime));
        deltaPosition -= transform.position;

        transform.position += new Vector3(0, GetHeight(movingCurve, currentTime) - GetHeight(movingCurve, currentTime - Time.deltaTime), 0);
        transform.rotation = Quaternion.Lerp(rot1, rot2, movingCurve.Evaluate(currentTime));

        yield return null;
        StartCoroutine(MovingCor(deltaBackward + deltaPosition, deltaForward + deltaPosition, rot1, rot2));
    }

    void Update(){
        if (moves.Count != 0 && Input.GetKey(KeyCode.Q) && !isMoving){
            isMoving = true;
            i = 0;
            StartCoroutine(MakeDelay());
        }
    }

    void Move(MovingParameters par){
        RaycastHit hit;
        if (Physics.Raycast(transform.position + par.getPos(), Vector3.down, out hit, 1f) && !Physics.Raycast(transform.position + par.getPos() + new Vector3(0, 1f, 0), Vector3.down, out hit, 1f)) 
            StartCoroutine(MovingCor(new Vector3(0, 0, 0), par.getPos(),
             transform.rotation, Quaternion.Euler(par.getRot().x, par.getRot().y, par.getRot().z)));
        else Respawn();
    }

    public void Respawn(){
        transform.position = startPosition;
        isMoving = false; 
    }

    public void ChangeColor(){
        factor *= -1;
        if (factor == -1) rend.material = fadeMaterial;
        if (!isChangingColor) StartCoroutine(ChangeColorCor());
        gameObject.GetComponent<Collider>().enabled = !gameObject.GetComponent<Collider>().enabled;
    }

    public IEnumerator ChangeColorCor(){
        isChangingColor = true;
        yield return null;
        colorCurrentTime += factor * Time.deltaTime;
        rend.material.color = Color.Lerp(startColor, endColor, changing.Evaluate(colorCurrentTime));
        if (colorCurrentTime > 0 && colorCurrentTime < colorTotalTime) StartCoroutine(ChangeColorCor());
        else { 
            isChangingColor = false;
            colorCurrentTime = Mathf.Clamp(colorCurrentTime, 0, colorTotalTime);
            if (factor == 1) rend.material = opaqueMaterial;
        }
    }

    void OnCollisionEnter(Collision collision) { 
        if (collision.gameObject.tag == "clone") { collision.gameObject.GetComponent<CloneControll>().Respawn(); Respawn(); } 
        if (collision.gameObject.tag == "cube") {
            if (isMoving) Respawn();
            else Destroy(gameObject);
        }
        if (collision.gameObject.tag == "platform") {
            if (collision.gameObject.GetComponent<MovePlatforms>().isMoving) Destroy(gameObject);
        }
    }
}
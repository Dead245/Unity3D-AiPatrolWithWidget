using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{

    [SerializeField] GameObject traversingObject;
    [SerializeField] bool shouldLoop;
    Animator objectAnim;
    float walkSpeed = 2f;
    int currentTarget = 0;
    //Gizmos must be turned on for the positions to be updated as you move the gameObjects for the points in the scene.
    [SerializeField]
    List<Point> points = new List<Point>();

    void Start() {
        objectAnim = traversingObject.GetComponent<Animator>();
        if (shouldLoop)
        {
            StartCoroutine(loopingPatrol());
        }
        else
        {
            StartCoroutine(nonLoopingPatrol());
        }
    }

    void Update() {
        
    }

    IEnumerator loopingPatrol()
    {
        while (true) //condition can be changed for when character needs to deviate from the patrol
        {
            if (Vector3.Distance(traversingObject.transform.position, points[currentTarget].position) < 0.2f)
            {
                if (points[currentTarget].waitTime > 0f)
                {
                    objectAnim.SetBool("shouldMove", false);
                    yield return new WaitForSeconds(points[currentTarget].waitTime);
                    objectAnim.SetBool("shouldMove", true);
                }

                currentTarget++;

                //check if currentTarget might go out of bounds of array, if so, go back to starting point
                if (currentTarget == points.Count)
                {
                    currentTarget = 0;
                }
            }

            if (Vector3.Distance(traversingObject.transform.position, points[currentTarget].position) > 0.2f)
            {
                traversingObject.transform.LookAt(points[currentTarget].position, Vector3.up);
                traversingObject.transform.position = Vector3.MoveTowards(traversingObject.transform.position, points[currentTarget].position, walkSpeed * Time.deltaTime);
                objectAnim.SetBool("shouldMove", true);
            }
            yield return null;
        }
    }

    IEnumerator nonLoopingPatrol() {
        int movementDir = 1; //positive for moving 1-8, negative for moving 8-1

        while (true) //condition can be changed for when character needs to deviate from the patrol
        {
            if (Vector3.Distance(traversingObject.transform.position, points[currentTarget].position) < 0.2f)
            {
                if (points[currentTarget].waitTime > 0f) {
                    objectAnim.SetBool("shouldMove", false);
                    yield return new WaitForSeconds(points[currentTarget].waitTime);
                    objectAnim.SetBool("shouldMove", true);
                }

                currentTarget += movementDir;

                //check if currentTarget might go out of bounds of array, if so, swap direction
                if (currentTarget == points.Count) {
                    movementDir = -1;
                    currentTarget = points.Count - 2;
                }
                if (currentTarget == -1) {
                    movementDir = 1;
                    currentTarget = 1;
                }
            }

            if (Vector3.Distance(traversingObject.transform.position, points[currentTarget].position) > 0.2f) {
                traversingObject.transform.LookAt(points[currentTarget].position, Vector3.up);
                traversingObject.transform.position = Vector3.MoveTowards(traversingObject.transform.position, points[currentTarget].position, walkSpeed * Time.deltaTime);
                objectAnim.SetBool("shouldMove", true);
            }
            yield return null;
        }
    }

    //Creates point when you click on the "Add Point" button on the component in the editor (requires MovementManagerEditor)
    public void GeneratePoint() {
        GameObject point = Instantiate(transform.GetChild(0).gameObject,transform.GetChild(transform.childCount - 1).position, Quaternion.identity, transform);
        point.name = "Move Point #" + transform.childCount;
        Point newPoint = new Point(point.transform.position, point.transform.forward, 0);
        points.Add(newPoint);

    }

    //Draws lines between the points of the path and wire spheres on the points while in the editor
    private void OnDrawGizmos() {
        //Draws lines between children of what GameObject this script is on
        Gizmos.color = Color.white;
        Vector3 firstPoint = transform.GetChild(0).position;
        for (int i = 1; i < transform.childCount; i++) { 
            Vector3 secondPoint = transform.GetChild(i).position;
            Gizmos.DrawLine(firstPoint, secondPoint);
            firstPoint = secondPoint; 
        }
        if (shouldLoop) {
            Gizmos.DrawLine(transform.GetChild(0).position, transform.GetChild(transform.childCount-1).position);
        }

        //Adds small wire sphere at each point with label showing its number in the hierarchy
        for (int i = 0; i < transform.childCount; i++) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).position, 0.1f);
            
            Handles.Label(transform.GetChild(i).position + Vector3.up*0.5f,(i+1).ToString());

            //Additionally updates the position of the point relating to the gameobject if it has changed
            if (transform.GetChild(i).transform.hasChanged && points.Count > 0) {
                points[i].position = transform.GetChild(i).position;
                points[i].forward = transform.GetChild(i).forward;
                transform.GetChild(i).transform.hasChanged = false;
            }
        }
    }
}

[Serializable]
public class Point {
    //position is relative to the parent
    public Vector3 position;
    public Vector3 forward;
    public float waitTime;
    public Point() { //default
        position = Vector3.zero;
        forward = Vector3.zero;
        waitTime = 0;
    }

    public Point(Vector3 position, Vector3 forward, float waitTime)
    {
        this.position = position;
        this.forward = forward;
        this.waitTime = waitTime;
    }
}

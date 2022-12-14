using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{

    [SerializeField] GameObject traversingObject;
    [SerializeField] bool shouldLoop;

    [SerializeField]
    List<Point> points = new List<Point>();

    void Start()
    {

    }
    void Update()
    {
        
    }


    public void GeneratePoint() {
        GameObject point = Instantiate(transform.GetChild(0).gameObject,transform.GetChild(transform.childCount - 1).position, Quaternion.identity, transform);
        point.name = "Move Point #" + transform.childCount;
        Point newPoint = new Point(point.transform.position, point.transform.forward, 0);
        points.Add(newPoint);

    }

    //Draws lines between the points of the path and wire spheres on the points while in the editor
    private void OnDrawGizmos()
    {
        //Draws lines between children of what GameObject this script is on
        Gizmos.color = Color.white;
        Vector3 firstPoint = gameObject.transform.GetChild(0).position;
        for (int i = 1; i < gameObject.transform.childCount; i++) { 
            Vector3 secondPoint = gameObject.transform.GetChild(i).position;
            Gizmos.DrawLine(firstPoint, secondPoint);
            firstPoint = secondPoint; 
        }
        if (shouldLoop) {
            Gizmos.DrawLine(gameObject.transform.GetChild(0).position, gameObject.transform.GetChild(gameObject.transform.childCount-1).position);
        }

        //Adds small wire sphere at each point
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.GetChild(i).position, 0.1f);

            //Additionally updates the position of the point relating to the gameobject if it has changed
            if (gameObject.transform.GetChild(i).transform.hasChanged && points.Count > 0) {
                points[i].position = gameObject.transform.GetChild(i).position;
                points[i].forward = gameObject.transform.GetChild(i).forward;
                gameObject.transform.GetChild(i).transform.hasChanged = false;
            }
        }
    }
}

[Serializable]
public class Point
{
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

class Anchor
{ //For using with curved paths
    Vector3 position;
    Vector3 handleAPos;
    Vector3 handleBPos;
}

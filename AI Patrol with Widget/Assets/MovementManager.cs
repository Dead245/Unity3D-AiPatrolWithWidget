using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    List<Transform> movePoints = new List<Transform>();

    [SerializeField] GameObject traversingObject;

    [SerializeField]
    bool shouldLoop;

    [SerializeField]
    List<Point> points = new List<Point>();

    void Start()
    {
        int children = gameObject.transform.childCount;

        //Adds all children (which should be move points) to list
        for (int i = 0; i < children; i++) {
            Transform child = gameObject.transform.GetChild(i);

            Point newPoint = new Point(child.position,child.forward,0,null);

            points.Add(newPoint);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
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
    }
}

[Serializable]
public class Point
{
    public Vector3 position;
    public Vector3 forward;
    public float waitTime;
    public AnimationClip animationToPlay;

    public Point() { //default
        position = Vector3.zero;
        forward = Vector3.zero;
        waitTime = 0;
        animationToPlay = new AnimationClip();
    }

    public Point(Vector3 position, Vector3 forward, float waitTime, AnimationClip animationToPlay)
    {
        this.position = position;
        this.forward = forward;
        this.waitTime = waitTime;
        this.animationToPlay = animationToPlay;
    }
}

class Anchor
{ //For using with curved paths
    Vector3 position;
    Vector3 handleAPos;
    Vector3 handleBPos;
}

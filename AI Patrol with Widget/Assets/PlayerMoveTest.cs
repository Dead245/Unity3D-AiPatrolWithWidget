using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    [SerializeField]
    GameObject movePoint;
    [SerializeField]
    private bool shouldMove;

    [SerializeField]
    Animator anim;

    int speed = 2;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("shouldMove", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) { 
            shouldMove = !shouldMove;
        }

        if (shouldMove) {
            transform.LookAt(movePoint.transform, Vector3.up);
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position, step);
            anim.SetBool("shouldMove", true);
        }
        if (Vector3.Distance(transform.position, movePoint.transform.position) < 0.1f) { 
            shouldMove = false;
            anim.SetBool("shouldMove", false);
        }
    }
}

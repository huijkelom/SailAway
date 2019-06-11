using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoatControl : MonoBehaviour
{
    public Direction currentDirection;
    public Vector2 frontPos;
    public Vector2 backPos;

    private float stepSpeed;
    private Vector3 endPos;
    public bool keepMoving;

    // Start is called before the first frame update
    void Start()
    {
        currentDirection = Direction.West;
        //transform.Rotate(0, 0, 90);
        stepSpeed = 0.05f;
        endPos = transform.position;
        keepMoving = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (keepMoving)
        {
            Move();
            if (endPos == transform.position)
            {
                keepMoving = false;
            }
        }
    }

    //Responsible for calculating the player and setting the player up for moving
    public void MovePlayer(Vector2 destination)
    {
        Vector3 difference = Vector3.zero;
        switch (currentDirection)
        {
            case Direction.West:
                if (destination.x > backPos.x)
                {
                    difference.x = destination.x - backPos.x;
                }
                else
                {
                    difference.x = destination.x - frontPos.x;
                }
                break;
            case Direction.East:
                if (destination.x > backPos.x)
                {
                    difference.x = destination.x - frontPos.x;
                }
                else
                {
                    difference.x = destination.x - backPos.x;
                }
                break;
            case Direction.North:
                if (destination.y > backPos.y)
                {
                    difference.y = destination.y - frontPos.y;
                }
                else
                {
                    difference.y = destination.y - backPos.y;
                }
                break;
            case Direction.South:
                if (destination.y > backPos.y)
                {
                    difference.y = destination.y - backPos.y;
                }
                else
                {
                    difference.y = destination.y - frontPos.y;
                }
                break;
        }
        //transform.position += difference;
        keepMoving = true;
        endPos = transform.position + difference;
        frontPos.x += difference.x;
        frontPos.y += difference.y;
        backPos.x += difference.x;
        backPos.y += difference.y;
    }

    /// <summary>
    /// Moves the player a little everytime it gets called
    /// </summary>
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, stepSpeed);
    }
}

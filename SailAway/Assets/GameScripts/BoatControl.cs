using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    West,
    South,
    East,
    North
}

public class BoatControl : MonoBehaviour
{
    public Direction currentDirection = Direction.East;
    public Vector2 frontPos;
    public Vector2 backPos;

    private float stepSpeed;
    private Vector3 endPos;
    public bool keepMoving;

    // Start is called before the first frame update
    void Start()
    {
        //FaceRandomDirection();
        /*frontPos = new Vector2(0, 0);
        backPos = new Vector2(0, 0);*/
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
            if(endPos == transform.position)
            {
                keepMoving = false;
            }
        }
    }

    public Direction FaceRandomDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                currentDirection = Direction.East;
                transform.Rotate(0, 0, 270);
                break;
            case 1:
                currentDirection = Direction.North;
                transform.Rotate(0, 0, 0);
                transform.position = new Vector2(0, -0.5f);
                break;
            case 2:
                currentDirection = Direction.West;
                transform.Rotate(0, 0, 90);
                break;
            case 3:
                currentDirection = Direction.South;
                transform.Rotate(0, 0, 180);
                transform.position = new Vector2(0, -0.5f);
                break;
        }
        return currentDirection;
    }

    public Direction FaceFirstBoatDirection(int determinedNumber)
    {
        switch (determinedNumber)
        {
            case 0:
                currentDirection = Direction.North;
                transform.Rotate(0, 0, 0);
                transform.position = new Vector2(0, -0.5f);
                break;
            case 1:
                currentDirection = Direction.South;
                transform.Rotate(0, 0, 180);
                transform.position = new Vector2(0, -0.5f);
                break;
        }
        return currentDirection;
    }

    public void MoveBoat(Vector2 destination)
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

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, stepSpeed);
    }
}


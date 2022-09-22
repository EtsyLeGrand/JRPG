using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacter : MonoBehaviour
{
    [SerializeField] private float timeToReachSquare;
    [SerializeField] private float timeToWaitAtSquare;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private Queue<IEnumerator> movementQueue = new Queue<IEnumerator>();
    private const int QUEUE_MAX_SIZE = 3;

    private new BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        Vector2 targetBeforeMovement = targetPosition;

        #region Input Managing (WASD)
        if (movementQueue.Count < QUEUE_MAX_SIZE)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetPosition = new Vector2(targetPosition.x - 2, targetPosition.y);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                targetPosition = new Vector2(targetPosition.x + 2, targetPosition.y);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                targetPosition = new Vector2(targetPosition.x, targetPosition.y + 2);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                targetPosition = new Vector2(targetPosition.x, targetPosition.y - 2);
            }
        }
        #endregion

        if (targetBeforeMovement != targetPosition)
        {
            movementQueue.Enqueue(LerpMovement(targetPosition));
        }

        HandleQueue();
    }

    private void HandleQueue()
    {
        // Monster Check + dungeon check


        if (movementQueue.Count != 0 && !isMoving)
        {
            StartCoroutine(movementQueue.Peek());
            movementQueue.Dequeue();
        }

    }

    private IEnumerator LerpMovement(Vector2 target)
    {
        isMoving = true;
        Vector2 targetDirection = Vector2.zero;

        #region Change Rotation and Get Direction
        Vector2 startPos = transform.position;

        if (startPos.x > target.x) // Going Left
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            targetDirection = Vector2.left;
        }
        else if (startPos.x < target.x) // Going Right
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            targetDirection = Vector2.right;
        }

        if (startPos.y > target.y) // Going Down
        {
            targetDirection = Vector2.down;
        }
        else if (startPos.y < target.y) // Going Up
        {
            targetDirection = Vector2.up;

        }

        Debug.Log(targetDirection);

        #endregion

        #region Check for collision

        collider.offset = 0.16f * targetDirection;
        if (targetDirection == Vector2.right)
        {
            collider.offset = -collider.offset;
        }

        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        if (Physics2D.OverlapCollider(collider, filter, results) > 0)
        {
            switch (LayerMask.LayerToName(results[0].gameObject.layer))
            {
                #region Walls
                case "Walls":
                case "Default":
                    collider.offset = Vector2.zero;
                    targetPosition = transform.position;
                    isMoving = false;
                    yield break;

                #endregion

                #region Water
                case "Water": // À PRÉVOIR LE BOAT
                    collider.offset = Vector2.zero;
                    targetPosition = transform.position;
                    isMoving = false;
                    yield break;

                    #endregion
            }

            Debug.Log(LayerMask.LayerToName(results[0].gameObject.layer));
        }

        collider.offset = Vector2.zero;
        #endregion

        #region Lerp
        float timer = 0.0f;
        while (timer < timeToReachSquare)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, target, timer / timeToReachSquare);
            yield return null;
        }
        yield return new WaitForSeconds(timeToWaitAtSquare);
        isMoving = false;
        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacter : MonoBehaviour
{
    [SerializeField] private float timeToReachSquare;
    [SerializeField] private float timeToWaitAtSquare;
    private bool isMoving = false;
    private bool haltQueue = false;
    private Queue<IEnumerator> movementQueue = new Queue<IEnumerator>();
    private const int QUEUE_MAX_SIZE = 3;
    private const float ENCOUNTER_CHANCE_BY_STEP = 0.5f;
    private float totalEncounterChance = 0.0f;

    private new BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        #region Input Managing (WASD)
        if (movementQueue.Count < QUEUE_MAX_SIZE)
        {
            Vector2 direction = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.A))
            {
                direction = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction = Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                direction = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = Vector2.down;
            }

            if (direction != Vector2.zero)
            {
                movementQueue.Enqueue(LerpMovement(direction));
            }
        }
        #endregion

        HandleQueue();
    }

    private void HandleQueue()
    {
        // Monster Check + dungeon check
        if (movementQueue.Count != 0 && !isMoving && !haltQueue)
        {
            StartCoroutine(movementQueue.Peek());
            movementQueue.Dequeue();
            
        }

    }

    private IEnumerator LerpMovement(Vector2 direction)
    {
        isMoving = true;

        #region Change Rotation and Get Direction
        Vector2 startPos = transform.position;

        if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (direction == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        }

        #endregion

        #region Check for collision

        collider.offset = 0.16f * direction;
        if (direction == Vector2.right)
        {
            collider.offset = -collider.offset;
        }

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask =~ LayerMask.NameToLayer("Bounds");
        List<Collider2D> results = new List<Collider2D>();
        if (Physics2D.OverlapCollider(collider, filter, results) > 0) // COLLIDED
        {
            collider.offset = Vector2.zero;
            isMoving = false;
            switch (LayerMask.LayerToName(results[0].gameObject.layer))
            {
                #region Walls
                case "Walls":
                case "Default":
                    
                    yield break;

                #endregion

                #region Water
                case "Water": // � PR�VOIR LE BOAT
                    
                    yield break;
                #endregion
            }

            Debug.Log(LayerMask.LayerToName(results[0].gameObject.layer));
        }

        collider.offset = Vector2.zero;
        #endregion

        #region Lerp
        float timer = 0.0f;
        Vector2 targetPos = new Vector2(transform.position.x, transform.position.y) + 2 * direction;
        while (timer < timeToReachSquare)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, targetPos, timer / timeToReachSquare);
            yield return null;
        }
        yield return new WaitForSeconds(timeToWaitAtSquare);

        #endregion
        /*
        #region Fight
        totalEncounterChance += ENCOUNTER_CHANCE_BY_STEP;
        float rand = UnityEngine.Random.Range(0, 101);

        if (totalEncounterChance >= rand)
        {
            //Encounter
            haltQueue = true;
            SceneManager.ChangeScene("Fight", 1, 1);
        }
        #endregion
        */
        isMoving = false;
    }
}
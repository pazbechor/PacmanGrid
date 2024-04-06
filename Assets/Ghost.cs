using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private bool isMoving = false;
    public float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartRandomContinuousMovement();
    }


    private Vector2 GetRandomDirection()
    {
        int randomIndex = Random.Range(0, 4);
        // Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        // int randomIndex = Random.Range(0, 8);
        Vector2[] directions = {
        // Vector2.up, Vector2.down, Vector2.left, Vector2.right,
        new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1) // Diagonal directions
    };
        return directions[randomIndex];
    }
    
    // Start random continuous movement
    public void StartRandomContinuousMovement()
    {
        if (!isMoving)
        {
            Vector2 randomDirection = GetRandomDirection();
            StartCoroutine(MoveContinuously(randomDirection));
            Vector2 randomDirection2 = GetRandomDirection();
            // while (randomDirection2 == randomDirection){
                // randomDirection2 = GetRandomDirection();
            // }
            // StartCoroutine(MoveContinuously(randomDirection2));
        }
    }

    // Coroutine to move the ghost continuously in a specified direction
    private IEnumerator MoveContinuously(Vector2 direction)
    {
        isMoving = true;
        while (true)
        {
            Vector2 movement = new Vector2(direction.x, direction.y) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
            yield return null;
        }
    }

    public void StopContinuousMovement()
    {
        isMoving = false;
    }



}

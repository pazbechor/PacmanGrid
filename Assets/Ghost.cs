using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Ghost : MonoBehaviour
{
    private bool isMoving = false;
    public float moveSpeed = 5f;
    Vector2 direction;
    public GridManager gridManager;
    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        direction = GetRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        StartRandomContinuousMovement();
    }


    private static Vector2 GetRandomDirection()
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
        if (isMoving)
        {
            StartCoroutine(MoveContinuously());
        }
        else
        {
            direction = GetDifferentDirection(direction);
            StartCoroutine(MoveContinuously());
        }
    }

    // Coroutine to move the ghost continuously in a specified direction
    private IEnumerator MoveContinuously()
    {
        isMoving = true;
        Vector2 movement = new Vector2(direction.x, direction.y) * moveSpeed * Time.deltaTime;
        Vector2 startPosition = transform.position;
        // adding twice direction as frame is always blue
        Vector2 endPosition = startPosition + direction + direction;

        
        // 1 - Touching bounderies
        ValidMove isValid = gridManager.IsValidMove(
            new Vector2((int)startPosition.x, (int)startPosition.y),
            new Vector2((int)endPosition.x, (int)endPosition.y));
        if (isValid == ValidMove.InvalidOutOfBoundries){
            isMoving = false;
            yield return null;
        }

        // We are in bounderies
        // // 2 - Touching Blue (in progress OR blue no)
        endPosition = startPosition + direction;
        Vector2 temp = new Vector2((int)endPosition.x, (int)endPosition.y);
        Tile tile = gridManager.tiles[temp];
        if (tile.isBlue){ 
            if (tile.inProgress){
                // Debug.Log("In Progress");
                isMoving = false;
                yield return null;
                
                // ToDo;
            }else{
                // Debug.Log("Not In Progress");
                isMoving = false;
                yield return null;
                // ToDo
            }
        }

        transform.Translate(movement);
        yield return null;
        
    }

    public void StopContinuousMovement()
    {
        isMoving = false;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        // Check if the collision involves the Pacman object
        // if (collision.gameObject.CompareTag("Pacman"))
        // {
            // Perform actions specific to the collision with Pacman
            Debug.Log("Collision with Pacman detected!");
            
            // For example, decrease Pacman's health, restart the game, etc.
        // }
    }


    private Vector2 GetDifferentDirection(Vector2 direction){
        Vector2 tempDirection = GetRandomDirection();
        while(tempDirection == direction){
            tempDirection = GetRandomDirection();
        }
        return tempDirection;
    }


}

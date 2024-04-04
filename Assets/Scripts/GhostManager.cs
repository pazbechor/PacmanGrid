using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public Ghost ghostPrefab;
    public Dictionary<Vector2, Ghost> ghosts;

    public int amount = 1;
    public int movesCount = 0;
    public float moveSpeed = 5f;

    private bool isMoving = false;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private float moveDuration = 0.1f;

    public GridManager gridManager;
    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        ghosts = new Dictionary<Vector2, Ghost>();
        for (int i = 0; i < amount; i++) {
            
            int x = Random.Range(1, gridManager.GetWidth()-1);
            int y = Random.Range(1, gridManager.GetHeight()-1);
            var spawnedTile = Instantiate(ghostPrefab, new Vector3(x, y), Quaternion.identity);
            ghosts[new Vector2(x, y)] = spawnedTile;
            
        }

    }

    // Update is called once per frame
    private void Update() {
            foreach(KeyValuePair<Vector2, Ghost> entry in ghosts) {
                StartRandomContinuousMovement(entry);
            }
        // foreach(KeyValuePair<Vector2, Ghost> entry in ghosts) {
        //     if (!isMoving) {
        //         // Generate a random integer between 0 and 3 inclusive
                
        //         int randomDirection = Random.Range(0,4);

        //         Vector2 direction = Vector2.zero;
        //         switch (randomDirection) {
        //             case 0:
        //                 direction = Vector2.up;
        //                 break;
        //             case 1:
        //                 direction = Vector2.down;
        //                 break;
        //             case 2:
        //                 direction = Vector2.left;
        //                 break;
        //             case 3:
        //                 direction = Vector2.right;
        //                 break;
        //         }
        //         while (movesCount < 10){
        //             StartCoroutine(Move(direction, entry));
        //         }
        //         movesCount = 0;
        //     }
        // }
    }
    

    // Get a random direction (up, down, left, or right)
    private Vector2 GetRandomDirection()
    {
        int randomIndex = Random.Range(0, 4);
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        return directions[randomIndex];
    }
    
    // Start random continuous movement
    public void StartRandomContinuousMovement(KeyValuePair<Vector2, Ghost> kvp)
    {
        if (!isMoving)
        {
            Vector2 randomDirection = GetRandomDirection();
            StartCoroutine(MoveContinuously(kvp, randomDirection));
        }
    }

    // Coroutine to move the ghost continuously in a specified direction
    private IEnumerator MoveContinuously(KeyValuePair<Vector2, Ghost> kvp, Vector2 direction)
    {
        isMoving = true;

        // while (true)
        // {
            while (true)
            {
                Vector2 movement = new Vector2(direction.x, direction.y) * moveSpeed * Time.deltaTime;
                // Vector2 endPosition = kvp.Key + movement;
                // Vector2 roundedEndPosition = new Vector2(Mathf.RoundToInt(endPosition.x), Mathf.RoundToInt(endPosition.y));

                // ValidMove isValidMove = gridManager.IsValidMove(kvp.Key, roundedEndPosition);
                // if (isValidMove == ValidMove.InvalidOutOfBoundries){
                    // isMoving = false;
                    // yield break;
                // }
                kvp.Value.transform.Translate(movement);
                yield return null;
            }
            // direction = GetRandomDirection();
        // }
        
    }

    // Stop continuous movement
    public void StopContinuousMovement()
    {
        isMoving = false;
    }


    private IEnumerator Move(Vector2 direction, KeyValuePair<Vector2, Ghost> kvp) {
        Vector2 startPosition = kvp.Key;
        Vector2 endPosition = startPosition + (direction * gridSize);
        
        ValidMove isValidMove = gridManager.IsValidMove(startPosition,endPosition);
        
        if (isValidMove == ValidMove.InvalidOutOfBoundries)
        {
        yield break;  
        }


        // Ghost can't be on blue
        if (gridManager.IsBlueBox(endPosition)){
            yield break;
        }
        
        // StartCoroutine(moveGhostSmoothly(kvp, startPosition, endPosition));
        movesCount++;

    }  

}

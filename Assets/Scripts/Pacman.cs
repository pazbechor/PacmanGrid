using System.Collections;
using UnityEngine;

public class Pacman : MonoBehaviour {
  // Allows you to hold down a key for movement.
  public GridManager gridManager;
  [SerializeField] private bool isRepeatedMovement = true;
  // Time in seconds to move between one grid position and the next.
  [SerializeField] private float moveDuration = 0.1f;
  // The size of the grid
  [SerializeField] private float gridSize = 1f;

Vector2 pDirection;
  private bool shouldRepeatMove = false;
  private bool isMoving = false;
  System.Func<KeyCode, bool> lastKey;

  private void Start(){
    gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
  }
  // Update is called once per frame
  private void Update() {
    // Only process on move at a time.
    if (!isMoving) {
      // Accomodate two different types of moving.
      System.Func<KeyCode, bool> inputFunction;
      if (isRepeatedMovement) {
        // GetKey repeatedly fires.
        inputFunction = Input.GetKey;
      } else {
        // GetKeyDown fires once per keypress
        inputFunction = Input.GetKeyDown;
      }

      // If the input function is active, move in the appropriate direction.
      if (inputFunction(KeyCode.UpArrow)) {
        pDirection = Vector2.up;
        StartCoroutine(Move(Vector2.up));
      } else if (inputFunction(KeyCode.DownArrow)) {
        pDirection = Vector2.down;
        StartCoroutine(Move(Vector2.down));
      } else if (inputFunction(KeyCode.LeftArrow)) {
        pDirection = Vector2.left;
        StartCoroutine(Move(Vector2.left));
        
      } else if (inputFunction(KeyCode.RightArrow)) {
        pDirection = Vector2.right;
        StartCoroutine(Move(Vector2.right));
      }else {
        
        StartCoroutine(Move(pDirection, true));
      }
    }
  }

  // Smooth movement between grid positions.
  private IEnumerator Move(Vector2 direction, bool isRepeated=false) {
    Vector2 startPosition = transform.position;
    Vector2 endPosition = startPosition + (direction * gridSize);
    
    ValidMove isValidMove = gridManager.IsValidMove(startPosition,endPosition);
    
    if (isValidMove == ValidMove.InvalidOutOfBoundries)
    {
      yield break;  
    }


    // ToDo: should be OnCollision instead - Pacman VS Tile in progress
    if (isValidMove == ValidMove.InvalidBlueInProgress || isValidMove == ValidMove.InvalidGhost)
    {
        // Set player to 0,0
        handleRestart(); 
        yield break;
    }

    if (isRepeated && !shouldRepeatMove){
      yield break;
    }
    if (isRepeated && gridManager.IsBlueBox(endPosition)){
        shouldRepeatMove = false;
        StartCoroutine(movePlayerSmoothly(startPosition, endPosition));
        yield break;
    }
    if (!isRepeated){
      shouldRepeatMove = true;


    }
    
    StartCoroutine(movePlayerSmoothly(startPosition, endPosition));
    

  }

  private IEnumerator movePlayerSmoothly(Vector2 startPosition, Vector2 endPosition){
    // Record that we're moving so we don't accept more input.
    isMoving = true;

    
    // Smoothly move in the desired direction taking the required time.
    float elapsedTime = 0;
    while (elapsedTime < moveDuration) {
      elapsedTime += Time.deltaTime;
      float percent = elapsedTime / moveDuration;
      transform.position = Vector2.Lerp(startPosition, endPosition, percent);
      yield return null;
    }

    // Make sure we end up exactly where we want.
    transform.position = endPosition;

    // Change the current grid color    
    gridManager.SetTileInProgress(endPosition);

  
    // We're no longer moving so we can accept another move input.
    isMoving = false;
  }
  
  private void handleRestart(){
    transform.position = new Vector2(0,0);
    gridManager.RestartInProgresBoard();
  }

  private void OnCollisionEnter2D(Collision2D other) {  
        // Check if the collision involves the Pacman object
        // if (collision.gameObject.CompareTag("Ghost"))
        // { 
            // Perform actions specific to the collision with Pacman
            Debug.Log("Collision with Ghost detected!");
            
            // For example, decrease Pacman's health, restart the game, etc.
        // }
    }

}
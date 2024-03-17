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

  private bool isMoving = false;
  private int lifeCount = 3;

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
        StartCoroutine(Move(Vector2.up));
      } else if (inputFunction(KeyCode.DownArrow)) {
        StartCoroutine(Move(Vector2.down));
      } else if (inputFunction(KeyCode.LeftArrow)) {
        StartCoroutine(Move(Vector2.left));
      } else if (inputFunction(KeyCode.RightArrow)) {
        StartCoroutine(Move(Vector2.right));
      }
    }
  }

  // Smooth movement between grid positions.
  private IEnumerator Move(Vector2 direction) {
    Vector2 startPosition = transform.position;
    Vector2 endPosition = startPosition + (direction * gridSize);
    
    ValidMove isValidMove = gridManager.IsValidMove(endPosition);
    
    if (isValidMove == ValidMove.InvalidOutOfBoundries)
    {
      yield break;  
    }

    if (isValidMove == ValidMove.InvalidBlueInProgress)
    {
      lifeCount--;
      if (lifeCount == 0){
        // handleGameOver();
        yield break;
      }
      else 
      {
        // Set player to 0,0
        handleRestart(); 
        startPosition = transform.position;
        endPosition = transform.position;
        // yield return null;
      }
    }

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

    handleMovementEnd();
  }

  private void handleRestart(){
    transform.position = new Vector2(0,0);
    gridManager.RestartInProgresBoard();
  }

  private void handleMovementEnd(){

  }

}
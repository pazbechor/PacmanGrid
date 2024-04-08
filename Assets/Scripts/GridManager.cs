using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random=UnityEngine.Random;


public class GridManager : MonoBehaviour {
    private int width = 40;
    private int height = 30;

    public Tile tilePrefab;
    public Transform cam;

    public Dictionary<Vector2, Tile> tiles;

    public int score = 0;
    public int lifes = 3;


    public int currentTilesOn = 0;

    public int GetHeight(){return height;}
    public int GetWidth(){return width;}

    // public GhostManager ghostManager;
    Dictionary<int, Ghost> ghosts;
    public Ghost ghostPrefab;

    int ghostsAmount = 1;
    public Text scoreAsText;
    public Text lifesAsText;

    public int GetLifes() {return lifes;}
    void Start() {
        // ghostManager = GameObject.FindGameObjectWithTag("GhostManager").GetComponent<GhostManager>();
        GenerateGrid();
    }

    void decreaseLife(){
        lifes--;
        if (lifes == 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        lifesAsText.text = "Lifes: " + lifes.ToString();
        

    }
    void GenerateGrid() {
        
        // Generate tiles
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                bool shouldBeBlue = x == 0 || y ==0 || x == width -1 || y == height - 1;
                spawnedTile.Init(shouldBeBlue);

                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        // Generate ghosts: 
        ghosts = new Dictionary<int, Ghost>();
        
        for (int i = 0; i < ghostsAmount; i++) {
            
            int x = Random.Range(1, width-1);
            int y = Random.Range(1, height-1);
            var spawnedGhost = Instantiate(ghostPrefab, new Vector3(x, y), Quaternion.identity);
            ghosts[i] = spawnedGhost;
        }



        cam.transform.position = new Vector3((float)width/2 -0.5f, (float)height / 2 - 0.5f,-10);
    }


      public bool IsBlueBox(Vector2 currentPosition){
        return tiles[currentPosition].isBlue;
      }
      public ValidMove IsValidMove(Vector2 previousPosition, Vector2 playerPosition) {
            if (playerPosition.x < 0 ||
                playerPosition.x > width -1 || 
                playerPosition.y < 0 ||
                playerPosition.y > height-1)
            {
                return ValidMove.InvalidOutOfBoundries;
            }

         
            // ToDo: Delete. should be OnCollision instead - Ghost VS Pacman
            foreach(KeyValuePair<int, Ghost> entry in ghosts) {
                if (new Vector2((int)entry.Value.transform.position.x, (int)entry.Value.transform.position.y) == 
                        new Vector2((int)playerPosition.x, (int)playerPosition.y)
                    ){
                    // decreaseLife();
                    return ValidMove.InvalidGhost;
                }
            }


            // ToDo: Delete. should be OnCollision instead - Pacman VS Tile in progress
            if (IsTileInProgress(playerPosition)){
                // decreaseLife();
                return ValidMove.InvalidBlueInProgress;
            }


            if (IsCompletedMove(previousPosition, playerPosition)){
                HandleCloseArea();
                scoreAsText.text = "Score: " + score.ToString() + "%";
                return ValidMove.ValidStepCompleted;
            }

            return ValidMove.Valid;
      }

      public void SetTileInProgress(Vector2 currentPlayerPosition){
        Tile tile = tiles[currentPlayerPosition];
        if (!tile.isBlue){
            tile.SetColor(true, true);
        }
      }

      public bool IsTileInProgress(Vector2 currentPlayerPosition){
        return tiles[currentPlayerPosition].inProgress;
      }
      public bool IsCompletedMove(Vector2 prevPosition, Vector2 currentPosition){
        Tile tile = tiles[currentPosition];
        Tile prevTile = tiles[prevPosition];
        return !tile.inProgress && tile.isBlue && prevTile.inProgress;
      }

      public void RestartInProgresBoard(){
        foreach (KeyValuePair<Vector2, Tile> item in tiles)
        {
            Vector2 key = item.Key;
            Tile tile = item.Value;
            if (tile.inProgress){
                tile.SetColor(false,false);
            }
        }
    }


    private void HandleCloseArea(){
        foreach(KeyValuePair<int, Ghost> entry in ghosts) {
            float curGhostX = entry.Value.transform.position.x;
            float curGhostY =  entry.Value.transform.position.y;
            getTilesSomethingRecursive(curGhostX, curGhostY, "");
        }

        int blueTiles = 0;
        foreach(KeyValuePair<Vector2, Tile> entry in tiles)
        {
            entry.Value.inProgress = false;
            entry.Value.visited = false;
            
            if (!entry.Value.needToFill){
                if (entry.Value.isBlue){
                    blueTiles +=1;
                }
                entry.Value.needToFill = true;
                continue;
            }
            entry.Value.isBlue = true;
            blueTiles +=1;
            entry.Value.m_SpriteRenderer.sprite = entry.Value.blue;
        }

        int frame = (2*width) + (2*height) - 4;
        score = (int)Math.Round(100*(float)((float)(blueTiles - frame) / (tiles.Count - frame)));
    }


    void getTilesSomethingRecursive(float curGhostX, float curGhostY, string forbiddenDirection) {
        Vector2 currentPos = new Vector2((int)curGhostX, (int) curGhostY);
        if (tiles[currentPos].visited) { 
            return;
        }
        tiles[currentPos].visited = true;
        if (tiles[currentPos].inProgress || tiles[currentPos].isBlue) { 
            return;
        }
        
        tiles[currentPos].needToFill = false;

        if (forbiddenDirection != "right"){
            getTilesSomethingRecursive(curGhostX+1, curGhostY, "left");
        }

        if (forbiddenDirection != "up"){
            getTilesSomethingRecursive(curGhostX, curGhostY+1, "down");
        }

        
        if(forbiddenDirection != "left"){
            getTilesSomethingRecursive(curGhostX-1, curGhostY, "right");
        }

        
        if (forbiddenDirection != "down"){
           getTilesSomethingRecursive(curGhostX, curGhostY-1, "up");
        }
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GridManager : MonoBehaviour {
    private int width = 40;
    private int height = 30;

    public Tile tilePrefab;
    public Transform cam;

    public Dictionary<Vector2, Tile> tiles;

    public float score = 0;

    public int currentTilesOn = 0;

    public int GetHeight(){return height;}
    public int GetWidth(){return width;}

    public GhostManager ghostManager;

    void Start() {
        ghostManager = GameObject.FindGameObjectWithTag("GhostManager").GetComponent<GhostManager>();
        GenerateGrid();
    }

    void GenerateGrid() {
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
        cam.transform.position = new Vector3((float)width/2 -0.5f, (float)height / 2 - 0.5f,-10);
    }


      public ValidMove IsValidMove(Vector2 previousPosition, Vector2 playerPosition) {
            if (playerPosition.x < 0 ||
                playerPosition.x > width -1 || 
                playerPosition.y < 0 ||
                playerPosition.y > height-1)
            {
                return ValidMove.InvalidOutOfBoundries;
            }

         
            foreach(KeyValuePair<Vector2, Ghost> entry in ghostManager.ghosts) {
                if (entry.Key == playerPosition){
                    return ValidMove.InvalidGhost;
                }
            }


            if (IsTileInProgress(playerPosition)){
                return ValidMove.InvalidBlueInProgress;
            }


            if (IsCompletedMove(previousPosition, playerPosition)){
                HandleCloseArea();
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
        foreach(KeyValuePair<Vector2, Ghost> entry in ghostManager.ghosts) {
            float curGhostX = entry.Key.x;
            float curGhostY = entry.Key.y;
            getTilesSomethingRecursive(curGhostX, curGhostY, "");
        }

        foreach(KeyValuePair<Vector2, Tile> entry in tiles)
        {
            entry.Value.inProgress = false;
            entry.Value.visited = false;
            
            if (!entry.Value.needToFill){
                entry.Value.needToFill = true;
                continue;
            }
            entry.Value.isBlue = true;
            entry.Value.m_SpriteRenderer.sprite = entry.Value.blue;
        }
    }


    void getTilesSomethingRecursive(float curGhostX, float curGhostY, string forbiddenDirection) {
        Vector2 currentPos = new Vector2(curGhostX, curGhostY);
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GridManager : MonoBehaviour {
    private int width = 30;
    private int height = 20;

    public Tile tilePrefab;
    public Transform cam;

    public Dictionary<Vector2, Tile> tiles;

    public float score = 0;

    public int currentTilesOn = 0;

    void Start() {
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


      public ValidMove IsValidMove(Vector2 playerPosition) {
            if (playerPosition.x < 0 ||
                playerPosition.x > width -1 || 
                playerPosition.y < 0 ||
                playerPosition.y > height-1)
            {
                return ValidMove.InvalidOutOfBoundries;
            }


            if (IsTileInProgress(playerPosition)){
                return ValidMove.InvalidBlueInProgress;
            }


            if (IsCompletedMove(playerPosition)){
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
      public bool IsCompletedMove(Vector2 currentPlayerPosition){
        Tile tile = tiles[currentPlayerPosition];
        return !tile.inProgress && tile.isBlue;
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
        
    }
}
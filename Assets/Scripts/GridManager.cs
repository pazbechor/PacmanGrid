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


      public bool IsValidMove(Vector2 playerPosition) {
            if (playerPosition.x < 0 ||
                playerPosition.x > width -1 || 
                playerPosition.y < 0 ||
                playerPosition.y > height-1)
            {
                return false;
            }

            return true;
      }

}
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
    private void Update() {}
}

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
    {}

    // Update is called once per frame
    private void Update() {}
}

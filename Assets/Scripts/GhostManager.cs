using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GhostManager : MonoBehaviour
{
    public Ghost ghostPrefab;
    public Dictionary<Vector2, Ghost> ghosts;

    public int amount = 1;
    public GridManager gridManager;
    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        ghosts = new Dictionary<Vector2, Ghost>();
        for (int i = 0; i < amount; i++) {
            Random r = new Random();
            int x = r.Next(1, gridManager.GetWidth()-1);
            int y = r.Next(1, gridManager.GetHeight()-1);
            var spawnedTile = Instantiate(ghostPrefab, new Vector3(x, y), Quaternion.identity);
            ghosts[new Vector2(x, y)] = spawnedTile;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

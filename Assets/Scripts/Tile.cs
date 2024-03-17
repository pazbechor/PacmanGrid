using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Tile : MonoBehaviour {
 
    public SpriteRenderer m_SpriteRenderer;
    public Sprite blue;
    public Sprite green;

    public bool visited;

    // Default state is green
    public bool isBlue;

    // If in progress - it turned to blue from green, but state is not final
    public bool inProgress;

    public void Init(bool isOnInit) {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (isOnInit) {
            m_SpriteRenderer.sprite = blue;
            isBlue = true;
        } else {
            m_SpriteRenderer.sprite = green;
        }
    }

}
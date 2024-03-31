using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Tile : MonoBehaviour {
 
    public SpriteRenderer m_SpriteRenderer;
    public Sprite blue;
    public Sprite green;
    public bool needToFill;

    public bool visited;

    // Default state is green
    public bool isBlue;

    // If in progress - it turned to blue from green, but state is not final
    public bool inProgress;

    public void Init(bool isOnInit) {
        needToFill = true;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (isOnInit) {
            m_SpriteRenderer.sprite = blue;
            isBlue = true;
        } else {
            m_SpriteRenderer.sprite = green;
        }
    }


    public void SetColor(bool setBlue, bool isInProgress){
        inProgress = isInProgress;
        isBlue = setBlue;
        if(isBlue){
            m_SpriteRenderer.sprite = blue;
        } else{
            m_SpriteRenderer.sprite = green;
        }

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlanetUI : MonoBehaviour
{

    public Color selectedColor;
    public SpriteRenderer highlightSprite;

    public UnityEvent onMouseEnter;
    public UnityEvent onMouseExit;
    public UnityEvent onMouseClick;

    bool mouseClick = false;
    bool selected = false;
    bool active = false;

    private bool isSelectionAviable
    {
        get
        {
            if (GameManager.instance.isGamePaused) return false;
            if (!active)
            {
                if (GameManager.instance.isPlanetsSelected)
                    return true;
                return false;
            }
            return true;
        }
    }



    private void OnMouseEnter()
    {
        if (isSelectionAviable && !selected)
        {
            highlightSprite.gameObject.SetActive(true);
            onMouseEnter?.Invoke();
        }  
    }

    private void OnMouseExit()
    {
        mouseClick = false;
        if (isSelectionAviable && !selected)
        {
            highlightSprite.gameObject.SetActive(false);
            onMouseExit?.Invoke();
        }  
    }

    private void OnMouseDown()
    {
        if (isSelectionAviable)
            mouseClick = true;
    }

    private void OnMouseUp()
    {
        if (mouseClick)
        {
            selected = !selected;
            onMouseClick?.Invoke();
        } 
    }

    public void Select()
    {
        if (GetComponent<Planet>().owner == GameCommon.Owner.PLAYER)
        {
            Color col = Color.white;

            if (selected)
                col = selectedColor;

            col.a = 0.5f;
            highlightSprite.color = col;

            GameManager.instance.SelectPlanet(GetComponent<Planet>());
        } else
        {
            HideSelection();
            GameManager.instance.AttackPlanet(GetComponent<Planet>());
            onMouseExit?.Invoke();
        }
    }

    public void HideSelection()
    {
        selected = false;
        Color col = Color.white;
        col.a = 0.5f;
        highlightSprite.color = col;
        highlightSprite.gameObject.SetActive(false);
    }

    public void SetActive(bool active = true)
    {
        this.active = active;
    }

}

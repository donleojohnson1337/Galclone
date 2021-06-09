using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlanetUI : MonoBehaviour
{

    public Color selectedColor;
    public SpriteRenderer highlightSprite;

    bool mouseClick = false;
    bool selected = false;
    bool active = false;


    //ѕроверка возможности выбрать/подсветить планету
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


    //ѕодсветка планеты при наведении
    private void OnMouseEnter()
    {
        if (isSelectionAviable && !selected)
        {
            highlightSprite.gameObject.SetActive(true);
        }  
    }

    private void OnMouseExit()
    {
        mouseClick = false;
        if (!selected)
        {
            highlightSprite.gameObject.SetActive(false);
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
            Select();
        } 
    }

    //ќбработка клика по планете: выделение или его сн€тие дл€ своих планет, выбор планет дл€ десантировани€
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
        } else if (isSelectionAviable)
        {
            HideSelection();
            GameManager.instance.AttackPlanet(GetComponent<Planet>());
        } else
        {
            HideSelection();
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
        if (selected && !active)
        {
            HideSelection();
            GameManager.instance.SelectPlanet(GetComponent<Planet>());
        }
    }

}

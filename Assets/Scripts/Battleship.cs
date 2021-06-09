using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameCommon;

public class Battleship : MonoBehaviour
{

    public float speed = 1f;

    private Owner _owner = Owner.NONE;
    public Owner owner
    {
        get
        {
            return _owner;
        }
    }

    private int _troops;
    public int troops
    {
        get
        {
            return _troops;
        }
    }

    private GameObject target;



    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void SetTarget(Owner owner, GameObject target, int troops)
    {
        _owner = owner;
        switch (owner)
        {
            case Owner.PLAYER:
                SetColor(PLAYER_COLOR);
                break;
            case Owner.AI:
                SetColor(AI_COLOR);
                break;
        }

        _troops = troops;
        this.target = target;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        transform.up = Vector3.Lerp(transform.up, (target.transform.position - transform.position), 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProcessCollision(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision.gameObject);
    }


    private void ProcessCollision(GameObject gameObject)
    {
        if (gameObject != target) return;

        Planet planet = gameObject.GetComponent<Planet>();

        if (planet)
        {
            planet.LandTroops(owner, troops);
            Destroy(this.gameObject);
        }

    }

}

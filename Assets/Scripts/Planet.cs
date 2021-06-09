using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameCommon;

[RequireComponent(typeof(PlanetUI))]
public class Planet : MonoBehaviour
{
    public TextMeshPro populationText;
    public SpriteRenderer planetColorSprite;

    private Owner _owner = Owner.NONE;
    public Owner owner
    {
        get
        {
            return _owner;
        }
    }

    private int _population;
    private int population
    {
        set
        {
            _population = Mathf.Clamp(value, 0, MAX_PLANET_POPULATION);
        }

        get
        {
            return _population;
        }

    }

    public PlanetUI ui
    {
        get
        {
            return GetComponent<PlanetUI>();
        }
    }

    private Coroutine lifeOnPlanet;

    private void StartLifeOnPlanet()
    {
        lifeOnPlanet = StartCoroutine(CreatePopulation());
        ui.SetActive(owner == Owner.PLAYER);
    }

    private void StopLifeOnPlanet()
    {
        StopCoroutine(lifeOnPlanet);
        lifeOnPlanet = null;
    }

    //Создание нового населения раз в секунду
    private IEnumerator CreatePopulation()
    {
        while (true)
        {
            if (owner != Owner.NONE && population<MAX_PLANET_POPULATION)
            {
                population += POPULATION_INCREMENT;
                UpdateUI();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    //Отправка десанта на другую планету
    public void SendTroops(Planet targetPlanet)
    {
        if (population < 2) return;
        int troopsCount = population / 2;
        population -= troopsCount;

        for (int i = 0; i < troopsCount; i++)
        {
            Vector3 spawn = transform.position;
            spawn.z = 10;
            Battleship b = Instantiate(GameManager.instance.battleshipPrefab, spawn, Quaternion.identity).GetComponent<Battleship>();
            b.SetTarget(owner, targetPlanet.gameObject, 1);
        }

        UpdateUI();
    }

    //Высадка десанта с другой планеты
    public void LandTroops(Owner troopsOwner, int count)
    {
        if (troopsOwner != _owner)
        {
            _population -= count;
            if (_population < 0)
            {
                _population *= -1;
                _owner = troopsOwner;
                StartLifeOnPlanet();
            } else if (_population == 0)
            {
                _owner = Owner.NONE;
                if (lifeOnPlanet != null)
                {
                    StopLifeOnPlanet();
                }
            }
        }
        else
        {
            population += count;
        }
        UpdateUI();
    }

    //Обновление индикатора населения и цвета планеты, в зависимости от владельца
    private void UpdateUI()
    {
        populationText.text = population.ToString();
        if (population == 0) planetColorSprite.color = Color.white;
        switch (owner)
        {
            case Owner.PLAYER:
                planetColorSprite.color = PLAYER_COLOR;
                break;
            case Owner.AI:
                planetColorSprite.color = AI_COLOR;
                break;
        }
    }

    public void KillPopulation(int count)
    {
        population -= count;
        UpdateUI();
    }

    public int GetPopulation()
    {
        return population;
    }

}

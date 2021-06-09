using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameCommon;

public class GameManager : Singleton<GameManager>
{

    public GameObject mainMenu;
    public GameObject menuBtn;

    public GameObject battleshipPrefab;
    public GameObject planetPrefab;

    //Размер игрового поля: макс. расстояние от центра до границы по X и Y
    public Vector3 mapSize = new Vector3(8, 4, 0);

    private List<Planet> planetsSelected = new List<Planet>();
    public bool isPlanetsSelected
    {
        get
        {
            return planetsSelected.Count > 0;
        }
    }

    private bool _isGamePaused = true;
    public bool isGamePaused
    {
        get
        {
            return _isGamePaused;
        }
    }

    public void ShowMenu()
    {
        mainMenu.SetActive(true);
        menuBtn.SetActive(false);
        Time.timeScale = 0.0f;
        _isGamePaused = true;
    }

    public void HideMenu()
    {
        mainMenu.SetActive(false);
        menuBtn.SetActive(true);
        Time.timeScale = 1.0f;
        _isGamePaused = false;
    }

    public void StartGame()
    {
        HideMenu();

        foreach (Planet p in FindObjectsOfType<Planet>())
            Destroy(p.gameObject);

        foreach (Battleship b in FindObjectsOfType<Battleship>())
            Destroy(b.gameObject);

        planetsSelected.Clear();

        GeneratePlanets();
    }

    //Генерация планет в рамках игрового поля с проверкой расстояния между ними
    //Расстояние между планетами должно быть не меньше суммы их радиусов
    private void GeneratePlanets()
    {
        List<Planet> createdPlanets = new List<Planet>();

        int i = 0;
        while (i<PLANETS_ON_LEVEL)
        {
            Vector3 position = new Vector3(
                Random.Range(-mapSize.x, mapSize.x),
                Random.Range(-mapSize.y, mapSize.y),
                0);
            float radius = Random.Range(0.5f, 1f);

            bool check = true;

            foreach (Planet p in createdPlanets)
            {
                Transform t = p.transform;
                float planetRadius = t.localScale.x / 2;
                if (Vector3.Distance(position, t.transform.position) < (planetRadius*2 + radius*2))
                {
                    check = false;
                    break;
                }
            }

            if (!check) continue;

            int startPopulation = (int)(radius * 100); ;
            Planet planet = Instantiate(planetPrefab, position, Quaternion.identity).GetComponent<Planet>();
            planet.gameObject.transform.localScale = new Vector3(radius * 2, radius * 2, 1);

            createdPlanets.Add(planet);

            if (i==0)
            {
                planet.LandTroops(Owner.PLAYER, 50);
                planet.ui.SetActive();
            }    
            else if (i == PLANETS_ON_LEVEL - 1)
                planet.LandTroops(Owner.AI, 50);
            else
                planet.LandTroops(Owner.NONE, startPopulation);
            i++;
        }
        AIBot.instance.Init(createdPlanets);
    }

    public void SelectPlanet(Planet p)
    {
        if (planetsSelected.Contains(p))
        {
            planetsSelected.Remove(p);
        } else
        {
            planetsSelected.Add(p);
        }
    }

    public void AttackPlanet(Planet targetPlanet)
    {
        foreach(Planet planet in planetsSelected)
        {
            planet.SendTroops(targetPlanet);
            planet.ui.HideSelection();
        }
        planetsSelected.Clear();
    }

    //Отрисовка границ игрового поля для редактора
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, mapSize * 2);
    }

}

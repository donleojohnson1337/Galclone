using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameCommon;

public class AIBot : Singleton<AIBot>
{
    public float workDelay = 2f;
    //Минимальное кол-во жителей на планете для создания десанта
    public int minExpandPopulation = 40;
    //Макс. кол-во планет отправляющих десант за раз
    public int maxExpansion = 2;

    private List<Planet> planets;

    public void Init(List<Planet> allPlanets)
    {
        planets = allPlanets;
        StartCoroutine(AIWork());
    }

    private IEnumerator AIWork()
    {
        new WaitForSeconds(workDelay);
        while (true)
        {
            SortPlanets();
            bool isAIdead = true;
            int isPlayerDead = 0;

            int expansions = 0;

            foreach (Planet planet in planets)
            {
                if (planet.owner == Owner.AI)
                {
                    isAIdead = false;
                    isPlayerDead++;
                    if (planet.GetPopulation() >= minExpandPopulation)
                    {
                        FindTarget(planet);
                        expansions++;
                        if (expansions >= maxExpansion) break;
                    }
                }
            }
            if (isAIdead)
            {
                //todo win
                break;
            }
            if (isPlayerDead == planets.Count)
            {
                //todo lose
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void FindTarget(Planet homeland)
    {
        foreach (Planet planet in planets)
        {
            if (planet.owner == Owner.AI) continue;
            homeland.SendTroops(planet);
            break;
        }
    }

    private void SortPlanets()
    {
        SortPlanets(0, planets.Count - 1);
    }

    private void SortPlanets(int L, int R)
    {
        int x = planets[L].GetPopulation();
        int i = L;
        int j = R;

        while (i <= j)
        {
            while (planets[i].GetPopulation() < x) i++;
            while (x < planets[j].GetPopulation()) j--;
            if (i<=j)
            {
                Planet p = planets[i];
                planets[i] = planets[j];
                planets[j] = p;
                i++;
                j--;
            }
        }

        if (L < j) SortPlanets(L,j);
        if (i < R) SortPlanets(i,R);

    }

}

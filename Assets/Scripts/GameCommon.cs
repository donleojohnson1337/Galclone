using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCommon
{
    public static Color PLAYER_COLOR = Color.blue;
    public static Color AI_COLOR = Color.red;

    public static int PLANETS_ON_LEVEL = 15;
    public static int POPULATION_INCREMENT = 5;
    public static int MAX_PLANET_POPULATION = 100;

    public enum Owner
    {
        PLAYER,
        AI,
        NONE
    };

}

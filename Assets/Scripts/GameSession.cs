using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : Singleton<GameSession>
{
    int score = 0;
    
    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int points)
    {
        score += points;
    }

    public void RestartScore()
    {
        score = 0;
    }
}

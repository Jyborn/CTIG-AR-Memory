using UnityEngine;

public class DifficultyManager
{
  

    private static DifficultyManager instance;

    [SerializeField] private Difficulty difficulty = Difficulty.NORMAL;

    public static DifficultyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DifficultyManager();
            }
            return instance;
        }
    }

    public float TimeAvailable
    {
        get { return GetTimeAvailable(); }
    }

    public Difficulty GetDifficulty
    {
        get { return difficulty; }
    }

    public void SetDifficulty(Difficulty newDifficulty)
    {
        difficulty = newDifficulty;
    }


    private float GetTimeAvailable()
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                return 120f;
            case Difficulty.NORMAL:
                return 60f;
            case Difficulty.HARD:
                return 30f;
            default:
                return 60f; // Default to Normal time if difficulty is not recognized
        }
    }
}

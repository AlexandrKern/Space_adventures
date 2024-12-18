using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instnace;
    public GameObject[] spaceshipPrefubs;
    public int[] spaceshipPrice = new int[] { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900 };
    private int currentSpaceshipIndex = 0;
    public int CurrentSpaceshipIndex => currentSpaceshipIndex;
    public GameObject currentSpaceship => spaceshipPrefubs[currentSpaceshipIndex];
    [HideInInspector] public Texture2D[] spaceshipTextures;
    [HideInInspector]public int curentLevelIndex = 0;
    [HideInInspector] public int numberMup = 1;

    private void Awake()
    {
        if (Instnace == null)
        {
            Instnace = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        currentSpaceshipIndex = Data.Instance.GetCurrentSpaceship();
    }

    public void ChangeCurrentSpaceship(int index)
    {
        currentSpaceshipIndex = index;
    }
}

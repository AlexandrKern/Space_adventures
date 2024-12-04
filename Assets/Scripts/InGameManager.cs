using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public Image healtImage;
    public float healthBarChangeTime;

    public GameObject deathScreen;
    public GameObject pauseScreen;
    public GameObject levelCompleteScreen;
    public GameObject audioScreen;
    public Button nextLevelButton;

    public Text AsteroiToKillText;
    public Text goldTextGameOver;
    public Text goldTextLevelCompleted;

    private int goldEarnedPerLevel;



    public void ChangeHealthBar(float currentHealth,float maxHealth)
    {
        if (currentHealth < 0)
            return;
        if (currentHealth == 0)
        {
            Invoke("OpenDeathMenu",healthBarChangeTime);
        }

        float healthPac = currentHealth / maxHealth;
        StartCoroutine(SmoothChangeHeailhBar(healthPac));

    }

    private IEnumerator SmoothChangeHeailhBar(float newFilAmt)
    {
        float elapsed = 0;
        float oldFilAmt = healtImage.fillAmount;

        while (elapsed <= healthBarChangeTime)
        {
            elapsed += Time.deltaTime;
            float currentFilAmt = Mathf.Lerp(oldFilAmt, newFilAmt, elapsed / healthBarChangeTime);
            healtImage.fillAmount = currentFilAmt;
            yield return null;
        }
    }

    public void OnFireButtonClicked()
    {
        playerManager.FireRockets();
    }

    public void OnMenuBattonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    public void OnQuitBattonClicked()
    {

        Application.Quit();
    }

    public void OnPauseBattonClicked()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        audioScreen.SetActive(false);
    }

    public void OnContinueBattonClicked()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }
    public void OnAudioScreen()
    {
        pauseScreen.SetActive(false);
        audioScreen.SetActive(true);
    }

    public void OnRestartBattonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenDeathMenu()
    {
        Time.timeScale = 0;
        goldTextGameOver.text = "+" + Data.Instance.GetGoldEarnedLevel().ToString();
        deathScreen.SetActive(true);
    }

    public void ChangeAsteroidKillCount(int toKill)
    {
        AsteroiToKillText.text = toKill.ToString();
    }

    public void OnLevelCompleteMenu()
    {
        Time.timeScale = 0;
        goldTextLevelCompleted.text = "+" + Data.Instance.GetGoldEarnedLevel().ToString();
        levelCompleteScreen.SetActive(true);
        int indexNextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 10 )
        {
            nextLevelButton.gameObject.SetActive(false);
        }
        if (!Utility.SceneExists(indexNextScene))
        {
            nextLevelButton.gameObject.SetActive(false);
        }
    }

    public void OnNextLevel()
    {
        Time.timeScale = 1;
        levelCompleteScreen.SetActive(false);
        GameManager.Instnace.curentLevelIndex++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Data.Instance.ResetCurrentGold();
    }

}

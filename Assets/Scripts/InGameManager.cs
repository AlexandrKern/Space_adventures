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

    public Text AsteroiToKillText;


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
    }

    public void OnContinueBattonClicked()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void OnRestartBattonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenDeathMenu()
    {
        Time.timeScale = 0;
        deathScreen.SetActive(true);
    }

    public void ChangeAsteroidKillCount(int toKill)
    {
        AsteroiToKillText.text = toKill.ToString();
    }

    public void OnLevelCompleteMenu()
    {
        Time.timeScale = 0;
        levelCompleteScreen.SetActive(true);
    }

}

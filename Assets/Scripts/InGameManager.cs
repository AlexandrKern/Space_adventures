using DG.Tweening;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public Image healthImage;
    public float healthBarChangeTime;
    public GameObject deathScreen;
    public GameObject pauseScreen;
    public GameObject levelCompleteScreen;
    public GameObject audioScreen;
    public Button nextLevelButton;
    public Text AsteroiToKillText;
    public Text goldTextGameOver;
    public Text goldTextLevelCompleted;
    public Image backgroundImage; 
    public float scaleMultiplier = 1.2f; 
    public Color fullHealthColor = Color.green; 
    public Color lowHealthColor = Color.red; 

    public void ChangeHealthBar(float currentHealth,float maxHealth)
    {
        if (currentHealth < 0)
            return;
        if (currentHealth == 0)
        {
            Invoke("OpenDeathMenu",healthBarChangeTime);
        }
        float healthPercent = currentHealth / maxHealth;
        Sequence healthSequence = DOTween.Sequence();
        healthSequence.Append(healthImage.DOFillAmount(healthPercent, healthBarChangeTime)
            .SetEase(Ease.OutQuad)); 
        Color targetColor = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
        healthSequence.Join(healthImage.DOColor(targetColor, healthBarChangeTime)
            .SetEase(Ease.Linear));
        backgroundImage.transform.DOScale(Vector3.one * scaleMultiplier, healthBarChangeTime / 2)
            .SetEase(Ease.OutBack);
        healthSequence.Append(backgroundImage.transform.DOScale(Vector3.one, healthBarChangeTime / 2)
            .SetEase(Ease.InBack));
        healthSequence.Play();
    }

    public void OnFireButtonClicked(Button button)
    {
        Transform buttonTransform = button.transform;
        Sequence fireButtonSequence = DOTween.Sequence();
        fireButtonSequence.Append(buttonTransform.DOScale(Vector3.one * 0.8f, 0.1f)
            .SetEase(Ease.OutQuad)); 
        fireButtonSequence.Append(buttonTransform.DOScale(Vector3.one, 0.1f)
            .SetEase(Ease.OutQuad)); 
        fireButtonSequence.Play();
        playerManager.FireRockets();
    }

    public void OnMenuBattonClicked()
    {
        SoundButtonClick();
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    public void OnQuitBattonClicked()
    {
        SoundButtonClick();
        Application.Quit();
    }

    public void OnPauseBattonClicked()
    {
        SoundButtonClick();
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        audioScreen.SetActive(false);
    }

    public void OnContinueBattonClicked()
    {
        SoundButtonClick();
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }
    public void OnAudioScreen()
    {
        SoundButtonClick();
        pauseScreen.SetActive(false);
        audioScreen.SetActive(true);
    }

    public void OnRestartBattonClicked()
    {
        SoundButtonClick();
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

    public void OnNextLevelButtonClicked()
    {
        SoundButtonClick();
        Time.timeScale = 1;
        levelCompleteScreen.SetActive(false);
        GameManager.Instnace.curentLevelIndex++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Data.Instance.ResetCurrentGold();
    }

    private void SoundButtonClick()
    {
        AudioManager.Instance.PlaySFX("Click");
    }
}

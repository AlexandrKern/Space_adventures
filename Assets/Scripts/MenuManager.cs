using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public Transform levelContainerMapOne;
    public Transform levelContainerMapTwo;
    public Transform levelContainerMapThree;
    public RectTransform menuContainer;
    private float screenWidth;
    private float screenHeight;
    public float trasitionTime;
    private int numberMup = 1;
    public Transform transformButtonParent;
    private GameObject currentScreenMap;
    public Button previousButton;
    public Button nextButton;

    public Color currentCevelButtonColor;
    public Color passedCevelButtonColor;
    public Color notPassedtCevelButtonColor;

    public RectTransform canvasRect;

    private GameObject currentSpaceshipPriview = null;

    public float rotationSpaceshipSpeed = 10;

    public Text goldText;


    private void Start()
    {
        previousButton.interactable = false;
        currentScreenMap = levelContainerMapOne.gameObject;
        InitButtonLevels(levelContainerMapOne,numberMup);
        screenWidth = canvasRect.rect.width;
        screenHeight = canvasRect.rect.height;
        InitShopButtons();
        UpdateSpaceshipPriview();
        UpdateGoldText();
    }

    private void Update()
    {
        if (currentSpaceshipPriview != null)
        {
            currentSpaceshipPriview.transform.Rotate(0, rotationSpaceshipSpeed * Time.deltaTime, 0);
        }
        
    }

    private void UpdateSpaceshipPriview()
    {
        if (currentSpaceshipPriview != null)
        {
            Destroy(currentSpaceshipPriview);
        }

        GameObject newSpaceship = GameManager.Instnace.currentSpaceship;
        Vector3 startRotationVector = new Vector3(0, 180, 0);
        currentSpaceshipPriview = Instantiate(newSpaceship, Vector3.zero, Quaternion.Euler(startRotationVector));
    }

    private void InitShopButtons()
    {
        int i = 0;
        foreach (Transform t in transformButtonParent)
        {
            int currentIndex = i;
            Button button = t.GetComponent<Button>();
            button.onClick.AddListener(() => OnShopButtonClicked(currentIndex));

            if (Data.Instance.IsSpaceshipOwned(currentIndex))
            {
                t.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                Text btnText = t.GetComponentInChildren<Text>();
                btnText.text = GameManager.Instnace.spaceshipPrice[currentIndex].ToString();
                button.image.color = Color.black;
            }

            i++;
        }
    }

    private void OnShopButtonClicked(int index)
    {
        AudioManager.Instance.PlaySFX("Click");
        Transform btnClicked = transformButtonParent.GetChild(index);
        Button button = btnClicked.GetComponent<Button>();
        if (Data.Instance.IsSpaceshipOwned(index))
        {
            GameManager.Instnace.ChangeCurrentSpaceship(index);
            UpdateSpaceshipPriview();
            AnimateShopButtonClick(button);
        }
        else
        {
            int costOfSpaceship = GameManager.Instnace.spaceshipPrice[index];
            int currentGold = Data.Instance.GetGold();
            if (currentGold >= costOfSpaceship)
            {
                Data.Instance.RemoveGold(costOfSpaceship);
                Data.Instance.PrchaseSpaceship(index);
                btnClicked.GetChild(0).gameObject.SetActive(false);
                button.image.color = Color.white;
                GameManager.Instnace.ChangeCurrentSpaceship(index);
                UpdateSpaceshipPriview();
                UpdateGoldText();
            }
        }

    }

    private void AnimateShopButtonClick(Button button)
    {
        button.transform.DOScale(1.2f, 0.2f)
            .SetEase(Ease.OutBack) 
            .OnComplete(() =>
            {
                button.transform.DOScale(1f, 0.2f).SetEase(Ease.InBack);
            });
        button.image.DOColor(Color.green, 0.2f)
            .OnComplete(() =>
            {
                button.image.DOColor(Color.white, 0.2f);
            });
    }

    private void InitButtonLevels(Transform levelContainer,int numberMup)
    {
        int lastLevelComplited = Data.Instance.GetLevelCompleted(numberMup);
        int i = 0;
        foreach (Transform t in levelContainer)
        {
            int currentIndex = i;
            Button button = t.GetComponent<Button>();
            if (currentIndex <= lastLevelComplited)
            {
                button.onClick.AddListener(() => OnLevelSelect(currentIndex,button));
                button.image.color = passedCevelButtonColor;

            }
            else if (currentIndex == lastLevelComplited + 1)
            {
                button.onClick.AddListener(() => OnLevelSelect(currentIndex,button));
                button.image.color = currentCevelButtonColor;
                
            }
            else
            {
                button.interactable = false;
                button.image.color = notPassedtCevelButtonColor;
            }
            i++;

        }
    }

    private void ChangeMenu(MenuType menuType)
    {
        Vector3 newPos;

        if (menuType == MenuType.Level)
            newPos = new Vector3(-canvasRect.rect.width, 0, 0);
        else if (menuType == MenuType.Shop)
            newPos = new Vector3(canvasRect.rect.width, 0, 0);
        else if (menuType == MenuType.Audio)
            newPos = new Vector3(0, canvasRect.rect.height, 0);
        else
            newPos = Vector3.zero;
        menuContainer.DOAnchorPos3D(newPos, trasitionTime).SetEase(Ease.InOutQuad);
    }

    private void OnLevelSelect(int index,Button button)
    {
        AudioManager.Instance.PlaySFX("Click");
        AnimateLevelButtonClick(button);
        GameManager.Instnace.curentLevelIndex = index;
        int levelIndex = index + 1;
        string levelName = $"Level_{levelIndex}_Map_{numberMup}";
        if (Utility.SceneExists(levelName))
        {
            Data.Instance.ResetCurrentGold();
            SceneManager.LoadScene(levelName);
        }
    }

    public void OnPlayButtonClicked(Button button)
    {
        AnimateMenuButtonClick(button);
        ChangeMenu(MenuType.Level);
    }

    public void OnMainMenuButtonClicked(Button button)
    {
        AnimateMenuButtonClick(button);
        ChangeMenu(MenuType.Main);
    }
    public void OnShopMenuButtonClicked(Button button)
    {
        AnimateMenuButtonClick(button);
        ChangeMenu(MenuType.Shop);
    }

    public void OnAudioMenuButtonClicked(Button button)
    {
        AnimateMenuButtonClick(button);
        ChangeMenu (MenuType.Audio);
    }

    private void UpdateGoldText()
    {
        goldText.text = Data.Instance.GetGold().ToString();
    }

    public void OnNextMupButtonClicked(Button button)
    {
        AnimateMenuButtonClick(button);
        int lastLevelComplited = Data.Instance.GetLevelCompleted(numberMup);
        if (lastLevelComplited < 4 )
        {
            return;
        }
        numberMup++;
        numberMup = Mathf.Clamp(numberMup, 1, 3);

        OnButtonInteracteble();

        GameManager.Instnace.numberMup = numberMup;

        UpdateCurrentMap();

    }
    public void OnPriviousMupButtonClicked(Button button)
    {
        AnimateMenuButtonClick(button);
        numberMup--;
        numberMup = Mathf.Clamp(numberMup, 1, 3);
        OnButtonInteracteble();
 
        Debug.Log(numberMup);
        GameManager.Instnace.numberMup = numberMup;

        UpdateCurrentMap();
    }

    private void AnimateMenuButtonClick(Button button)
    {
        AudioManager.Instance.PlaySFX("Click");
        Transform buttonTransform = button.transform;

        // Последовательность анимаций с использованием DOTween
        Sequence buttonSequence = DOTween.Sequence();

        buttonSequence
            // Уменьшение кнопки
            .Append(buttonTransform.DOScale(0.9f, 0.1f).SetEase(Ease.InOutQuad))
            // Увеличение с "прыжком"
            .Append(buttonTransform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBounce))
            // Возвращение к исходному размеру
            .Append(buttonTransform.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad))
            // Добавим небольшой поворот для акцента
            .Join(buttonTransform.DORotate(new Vector3(0, 0, 15f), 0.2f).SetEase(Ease.InOutQuad))
            .Append(buttonTransform.DORotate(Vector3.zero, 0.2f).SetEase(Ease.InOutQuad))
            // Меняем прозрачность (альфа-канал)
            .Join(button.image.DOFade(0.8f, 0.1f))
            .Append(button.image.DOFade(1f, 0.1f));

        // (Опционально) Воспроизведение звука нажатия кнопки
        // AudioManager.Instance.Play("ButtonClick");
    }

    private void UpdateCurrentMap()
    {
        if (currentScreenMap != null)
        {
            currentScreenMap.SetActive(false);
        }

        GameObject[] mapContainers = { levelContainerMapOne.gameObject, levelContainerMapTwo.gameObject, levelContainerMapThree.gameObject };

        if (numberMup > 0 && numberMup <= mapContainers.Length)
        {
            GameObject selectedMap = mapContainers[numberMup - 1];
            InitButtonLevels(selectedMap.transform, numberMup);
            selectedMap.SetActive(true);
            currentScreenMap = selectedMap;
        }
    }

    private enum MenuType
    {
        Main,
        Level,
        Shop,
        Audio
    }

    private void OnButtonInteracteble()
    {
        previousButton.interactable = !(numberMup == 1);
        nextButton.interactable = !(numberMup == 3);
    }

    private void AnimateLevelButtonClick(Button button)
    {
        Transform buttonTransform = button.transform;
        Sequence fireButtonSequence = DOTween.Sequence();

        fireButtonSequence.Append(buttonTransform.DOScale(Vector3.one * 0.1f, 0.1f)
            .SetEase(Ease.OutQuad));

        fireButtonSequence.Append(buttonTransform.DOScale(Vector3.one, 0.1f)
            .SetEase(Ease.OutQuad));
        fireButtonSequence.Play();
    }
}

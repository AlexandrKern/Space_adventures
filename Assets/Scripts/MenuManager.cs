using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Transform levelContainer;
    public RectTransform menuContainer;
    private int screenWidth;
    private int screenHeight;
    public float trasitionTime;

    public Transform transformButtonParent;

    private GameObject currentSpaceshipPriview = null;

    public float rotationSpaceshipSpeed = 10;

    public Text goldText;


    private void Start()
    {
        InitButtonLevels();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
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
            //Texture2D texture = GameManager.Instnace.spaceshipTextures[currentIndex];
            //Rect newRect = new Rect(0, 0, texture.width, texture.height);
            //Sprite newSprite = Sprite.Create(texture, newRect, new Vector2(0.5f, 0.5f));
            //t.GetComponent<Image>().sprite = newSprite;

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
        if (Data.Instance.IsSpaceshipOwned(index))
        {
            GameManager.Instnace.ChangeCurrentSpaceship(index);
            UpdateSpaceshipPriview();
        }
        else
        {
            int constOfSpaceship = GameManager.Instnace.spaceshipPrice[index];
            int currentGold = Data.Instance.GetGold();
            if (currentGold >= constOfSpaceship)
            {

                Data.Instance.RemoveGold(constOfSpaceship);
                Data.Instance.PrchaseSpaceship(index);
                Transform btnClicked = transformButtonParent.GetChild(index);
                btnClicked.GetChild(0).gameObject.SetActive(false);
                Button button = btnClicked.GetComponent<Button>();
                button.image.color = Color.white;
                GameManager.Instnace.ChangeCurrentSpaceship(index);
                UpdateSpaceshipPriview();
                UpdateGoldText();
            }
        
        }
       
    }

    private void InitButtonLevels()
    {
        int lastLevelComplited = Data.Instance.GetLevelCompleted();
        int i = 0;
        foreach (Transform t in levelContainer)
        {
            int currentIndex = i;
            Button button = t.GetComponent<Button>();
            if (currentIndex <= lastLevelComplited)
            {
                button.onClick.AddListener(() => OnLevelSelect(currentIndex));
                button.image.color = Color.white;
            }
            else if (currentIndex == lastLevelComplited + 1)
            {
                button.onClick.AddListener(() => OnLevelSelect(currentIndex));
                button.image.color = Color.green;
            }
            else
            {
                button.interactable = false;
                button.image.color = Color.gray;
            }
            i++;
        }
    }

    private void ChangeMenu(MenuType menuType)
    {
        Vector3 newPos;
        if (menuType == MenuType.Level)
        {
            newPos = new Vector3(-screenWidth, 0, 0);
        }
        else if (menuType == MenuType.Shop)
        {
            newPos = new Vector3(screenWidth, 0, 0);
        }
        else if (menuType == MenuType.Audio)
        {
            newPos = new Vector3(0, screenHeight, 0);
        }
        else
        {
            newPos = Vector3.zero;
        }

        StopAllCoroutines();
        StartCoroutine(ChangeMenuAnimation(newPos));
    }

    private IEnumerator ChangeMenuAnimation(Vector3 newPos)
    {
        float elapsed = 0;
        Vector3 oldPos = menuContainer.anchoredPosition3D;

        while (elapsed <= trasitionTime)
        {
            elapsed += Time.deltaTime;

            Vector3 currentPos = Vector3.Lerp(oldPos, newPos, elapsed / trasitionTime);
            menuContainer.anchoredPosition3D = currentPos;
            yield return null;
        }
    }

    private void OnLevelSelect(int index)
    {
        GameManager.Instnace.curentLevelIndex = index;

        int levelIndex = index + 1;
        string levelName = "Level_" + levelIndex.ToString();
        Debug.Log(levelIndex);
        SceneManager.LoadScene(levelName);

    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Играть");
        ChangeMenu(MenuType.Level);
    }

    public void OnMainMenuButtonClicked()
    {
        Debug.Log("Меню");
        ChangeMenu(MenuType.Main);
    }
    public void OnShopMenuButtonClicked()
    {
        ChangeMenu(MenuType.Shop);
    }

    public void OnAudioMenuButtonClicked()
    {
        ChangeMenu (MenuType.Audio);
    }

    public void OnNextMapButtonClicked()
    {
        Debug.Log("Следующая карта");
    }

    private void UpdateGoldText()
    {
        goldText.text = Data.Instance.GetGold().ToString();
    }
    private enum MenuType
    {
        Main,
        Level,
        Shop,
        Audio
    }


}

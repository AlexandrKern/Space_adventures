using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Transform levelContainer;
    public RectTransform menuContainer;
    private int screenWidth;
    public float trasitionTime;

    public Transform transformButtonParent;

    private GameObject currentSpaceshipPriview = null;

    public float rotationSpaceshipSpeed = 10;


    private void Start()
    {
        InitButtonLevels();
        screenWidth = Screen.width;
        InitShopButtons();
        UpdateSpaceshipPriview();
    }

    private void Update()
    {
        if (currentSpaceshipPriview != null)
        {
            currentSpaceshipPriview.transform.Rotate(0,rotationSpaceshipSpeed * Time.deltaTime, 0);
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
        currentSpaceshipPriview = Instantiate(newSpaceship,Vector3.zero,Quaternion.Euler(startRotationVector));
    }

    private void InitShopButtons()
    {
        int i = 0;
        foreach (Transform t in transformButtonParent)
        {
            int currentIndex = i;
            Texture2D texture = GameManager.Instnace.spaceshipTextures[currentIndex];
            Rect newRect = new Rect(0, 0, texture.width, texture.height);
            Sprite newSprite = Sprite.Create(texture, newRect,new Vector2(0.5f,0.5f));
            t.GetComponent<Image>().sprite = newSprite;

            Button button = t.GetComponent<Button>();
            button.onClick.AddListener(() => OnShopButtonClicked(currentIndex));

            i++;
        }
    }

    private void OnShopButtonClicked(int index)
    {
        GameManager.Instnace.ChangeCurrentSpaceship(index);
        UpdateSpaceshipPriview();
    }

    private void InitButtonLevels()
    {
        int i = 0;
        foreach (Transform t in levelContainer)
        {
            int currentIndex = i;
            Button button = t.GetComponent<Button>();
            button.onClick.AddListener(() => OnLevelSelect(currentIndex));
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

            Vector3 currentPos = Vector3.Lerp(oldPos,newPos,elapsed/trasitionTime);
            menuContainer.anchoredPosition3D = currentPos;
            yield return null;
        }
    }

    private void OnLevelSelect(int index)
    {
        Debug.Log(index);
        SceneManager.LoadScene("Level_1");
        
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
    public void OnShopMenuButtonClecked()
    {
        ChangeMenu(MenuType.Shop);
    }

    public void OnNextMapButtonClicked()
    {
        Debug.Log("Следующая карта");
    }

    private enum MenuType
    {
        Main,
        Level,
        Shop
    }


}

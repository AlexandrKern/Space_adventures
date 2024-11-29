using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerController activPlayerController;

    private void Awake()
    {
        if (GameManager.Instnace == null)
        {
            Debug.Log("GameManager.Instnace = null");
            GetFirstActivController();
        }
        else
        {
            SetCurrentSpaceship();
        }
        
    }

    private void SetCurrentSpaceship()
    {
        int currentSpaceshipIndex = GameManager.Instnace.CurrentSpaceshipIndex;

        int i = 0;

        foreach (Transform spaceship in transform)
        {
            int currentIndex = i;
            if (currentIndex == currentSpaceshipIndex)
            {
                spaceship.gameObject.SetActive(true);
                activPlayerController = spaceship.GetComponent<PlayerController>();
            }
            else
            {
                spaceship.gameObject.SetActive(false);
            }
            i++;
        }
    }

    public void FireRockets()
    {
        activPlayerController.FireRockets();
    }

    private void GetFirstActivController()
    {
        foreach (Transform spaceship in transform)
        {
            if (!spaceship.gameObject.activeSelf == true)
            {
                activPlayerController = spaceship.GetComponent<PlayerController>();
                return;
            }
        }
    }
}

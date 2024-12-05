using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject rocketPrefub;
    public Transform[] rocketSpawnPoints;
    public float fireInterval = 2;
    private bool canFire = true;

    public Joystick input;
    public float moveSpeed;
    private Rigidbody rb;

    public float maxRotation = 25;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    public float maxHealth = 4;
    private float currentHealth;

    public InGameManager inGameManager;

    private Vector3 raycastDirection = new Vector3(0, 0, 1);

    public float raycastDisnance;
    private int mask;

    public List<GameObject> priviousTargets = new List<GameObject>();


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetUpBoundries();
        currentHealth = maxHealth;
        mask = LayerMask.GetMask("EnemyRycastLayers");
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
        ColculateBoundries();
        RaycastForAsteroid();
    }

    private void RaycastForAsteroid()
    {
        List<GameObject> currentTargets = new List<GameObject>();

        foreach (Transform t in rocketSpawnPoints)
        {
            RaycastHit hit;
            Ray ray = new Ray(t.position, raycastDirection);
            if(Physics.Raycast(ray, out hit,raycastDisnance,mask))
            {
                GameObject target = hit.transform.gameObject;
                currentTargets.Add(target);
            }
        }

        bool listChanges = false;
        if (currentTargets.Count != priviousTargets.Count)
        {
            listChanges = true;
        }
        else
        {
            for (int i = 0; i < currentTargets.Count; ++i)
            {
                if (currentTargets[i] != priviousTargets[i])
                {
                    listChanges = true;
                }
            }
        }

        if (listChanges == true)
        {
            AsteroidManager.Instnace.UpdateAsteroid(currentTargets);
            priviousTargets = currentTargets;
        }
    }

    private void RotatePlayer()
    {
        float currentX  = transform.position.x;
        float newRotationZ;

        if (currentX < 0)
        {
           newRotationZ =  Mathf.Lerp(0,-maxRotation, currentX / minX);
        }
        else
        {
           newRotationZ = Mathf.Lerp(0, maxRotation, currentX / maxX);
        }

        Vector3 currentRotationVector3 = new Vector3(0,0,newRotationZ);
        Quaternion newRotation = Quaternion.Euler(currentRotationVector3);
        transform.localRotation = newRotation;
    }

    private void ColculateBoundries()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        transform.position = currentPosition;
    }

    private void SetUpBoundries()
    {
        float camDistace = Vector3.Distance(transform.position,Camera.main.transform.position);

        Vector2 bottomCorners = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistace));
        Vector2 topCorners = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistace));

        Bounds gameObjbounds = GetComponent<Collider>().bounds;

        float widthObj = gameObjbounds.size.x;
        float heightObj = gameObjbounds.size.y;

        minX = bottomCorners.x + widthObj;
        maxX = topCorners.x - widthObj;

        minY = bottomCorners.y + heightObj;
        maxY = topCorners.y - heightObj;

        AsteroidManager.Instnace.maxX = maxX;
        AsteroidManager.Instnace.maxY = maxY;
        AsteroidManager.Instnace.minX = minX;
        AsteroidManager.Instnace.minY = minY;


    }

    public void FireRockets()
    {
        
        if (canFire)
        {
            AudioManager.Instance.PlaySFX("Rocket");
            foreach (Transform t in rocketSpawnPoints)
            {
                Instantiate(rocketPrefub, t.position, Quaternion.identity);
            }
            canFire = false;
            StartCoroutine(ReloadDelay());
        }
    }

    private IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(fireInterval);
        canFire = true;
    }

    private void MovePlayer()
    {
        float horizontalMove = input.Horizontal;
        float verticalMove = input.Vertical;

        Vector3 movementVector = new Vector3(horizontalMove, verticalMove,0f);

        rb.velocity = movementVector * moveSpeed;
    }

    public void OnAsteroidImpact()
    {
        currentHealth--;

        inGameManager.ChangeHealthBar(currentHealth,maxHealth);

        if(currentHealth < 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        Debug.Log("Игрок Погиб");
    }
}

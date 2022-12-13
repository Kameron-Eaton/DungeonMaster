using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int playerHealth = 4;
    public GameObject h1;
    public GameObject h2;
    public GameObject h3;
    public GameObject h4;
    public GameObject sword;
    public Animator character;

    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayer;

    [HideInInspector]
    public bool swinging = false;

  
    
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = PlayerPrefs.GetInt("Health", 4);
        DamagePlayer(0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("hit");
            DamagePlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealPlayer(1);
        }
        if(sword.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    public void DamagePlayer(int dmgAmt)
    {
        Debug.Log("hit for: " + dmgAmt);
        playerHealth = playerHealth - dmgAmt;
        if(playerHealth <= 0)
        {
            playerHealth = 0;
        }
        switch (playerHealth)
        {
            case 0:
                GameOver();
                h1.SetActive(false);
                h2.SetActive(false);
                h3.SetActive(false);
                h4.SetActive(false);
                Debug.Log("lost");
                break;
            case 1:
                h1.SetActive(true);
                h2.SetActive(false);
                h3.SetActive(false);
                h4.SetActive(false);
                break;
            case 2:
                h1.SetActive(true);
                h2.SetActive(true);
                h3.SetActive(false);
                h4.SetActive(false);
                break;
            case 3:
                h1.SetActive(true);
                h2.SetActive(true);
                h3.SetActive(true);
                h4.SetActive(false);
                break;
            case 4:
                h1.SetActive(true);
                h2.SetActive(true);
                h3.SetActive(true);
                h4.SetActive(true);
                break;
        }
    }
    public void HealPlayer(int healAmt)
    {
        playerHealth = playerHealth + healAmt;
        if (playerHealth > 4)
            playerHealth = 4;
        switch (playerHealth)
        {
            case 2:
                h1.SetActive(true);
                h2.SetActive(true);
                h3.SetActive(false);
                h4.SetActive(false);
                break;
            case 3:
                h1.SetActive(true);
                h2.SetActive(true);
                h3.SetActive(true);
                h4.SetActive(false);
                break;
            case 4:
                h1.SetActive(true);
                h2.SetActive(true);
                h3.SetActive(true);
                h4.SetActive(true);
                break;
        }

    }
    void Attack()
    {
        character.SetTrigger("Swinging");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            Enemy enemyGO = enemy.GetComponent<Enemy>();
            enemyGO.TakeDamage();
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("Health", playerHealth);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }
}

using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.UIElements;

public class BossHealth : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 20;
    public int currentHealth;
    

    public Sprite[] healthSprites;
    //private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Image healthBarUI;


    public float deathDelay = 1f;

    private Boss bossScript;

    //public GameObject bossBar;

    void Start()
    {
        //bossBar.SetActive(false);
        currentHealth = maxHealth;
        //spriteRenderer = GetComponent<SpriteRenderer>();
        bossScript = GetComponent<Boss>();

        UpdateSprite();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateSprite();

        if (currentHealth <= 0)
        {
           StartCoroutine(HandleDeath());
        }
    }
    void UpdateSprite()
    {
        if (healthBarUI != null && healthSprites.Length > 0)
        {
            int spriteIndex = Mathf.Clamp(maxHealth - currentHealth, 0, healthSprites.Length - 1);
            healthBarUI.sprite = healthSprites[spriteIndex];
        }
    }

    private System.Collections.IEnumerator HandleDeath()
    {
        if (bossScript != null)
        {
            bossScript.enabled = false;
        }

        yield return new WaitForSeconds(deathDelay);

        EventManager.Instance?.PostNotification(NeonGrindEvents.BOSS_DEFEATED, this);

        Destroy(gameObject);  
    }
}

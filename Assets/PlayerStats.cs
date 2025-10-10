using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private float healthDecreaseRateForHungerOrThirst;
    [Header("Hunger")]
    [SerializeField] float currentHunger;
    [SerializeField] float maxHunger = 100f;
    [SerializeField] private Image hungerBarFill;
    [SerializeField] private float hungerDecreaseRate;
    [Header("Thirst")]
    [SerializeField] float currentThirst;
    [SerializeField] float maxThirst = 100f;
    [SerializeField] private Image thirstBarFill;
    [SerializeField] private float thirstDecreaseRate;

    void Awake()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        UpdateAllBars();
    }

    public void TakeDamage(float damage, bool overTime = false)
    {
        float amount = overTime ? damage * Time.deltaTime : damage;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBarFill();
        if (currentHealth <= 0f)
        {
            Debug.Log("Player is dead!");
            // add death handling here
        }
    }

    public void UpdateHealthBarFill()
    {
        healthBarFill.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }

    void Update()
    {
        UpdateHungerAndThirstBarFill();
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10f);
        }
    }

    public void UpdateHungerAndThirstBarFill()
    {
        // diminue la faim et la soif au fil du temps
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentThirst -= thirstDecreaseRate * Time.deltaTime;

        // on empeche que la faim et la soif descendent en dessous de 0 et au-dessus du max
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
        currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);

        // met à jour les visuels si assignés
        if (hungerBarFill != null)
            hungerBarFill.fillAmount = Mathf.Clamp01(currentHunger / maxHunger);
        if (thirstBarFill != null)
            thirstBarFill.fillAmount = Mathf.Clamp01(currentThirst / maxThirst);

        if (currentHunger <= 0f || currentThirst <= 0f)
        {
            TakeDamage((currentHunger <= 0f ? 1f : 0f) + (currentThirst <= 0f ? healthDecreaseRateForHungerOrThirst*2 : healthDecreaseRateForHungerOrThirst), true);
        }
    }

    void UpdateAllBars()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        if (hungerBarFill != null)
            hungerBarFill.fillAmount = Mathf.Clamp01(currentHunger / maxHunger);
        if (thirstBarFill != null)
            thirstBarFill.fillAmount = Mathf.Clamp01(currentThirst / maxThirst);
    }
}

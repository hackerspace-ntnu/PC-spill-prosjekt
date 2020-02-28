using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Vector2 spriteSize;

    [SerializeField]
    private Sprite fullHeart, damagedHeart;

    [SerializeField]
    private GameObject hearRendererPrefab;

    private List<Image> hearts;
    private int currentHealth = 0;
    // Start is called before the first frame update
    void Start()
    {
        hearts = new List<Image>();
        setMaxHealth(player.MaxHealth);
        setCurrentHealth(player.CurrentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.MaxHealth != hearts.Count)
        {
            setMaxHealth(player.MaxHealth);
        }
        if(player.CurrentHealth != currentHealth)
        {
            print("HeYo");
            setCurrentHealth(player.CurrentHealth);
        }
    }
    void setMaxHealth(int maxHealth)
    {
        while (hearts.Count < maxHealth)
        {
            GameObject a = Instantiate(hearRendererPrefab, transform);
            a.GetComponent<RectTransform>().localPosition = new Vector2(spriteSize.x * ( hearts.Count + 0.5f ), -spriteSize.y/2);
            hearts.Add(a.GetComponent<Image>());
        }
        while (hearts.Count > maxHealth)
        {
            GameObject a = hearts[hearts.Count].gameObject;
            hearts.RemoveAt(hearts.Count);
            Destroy(a);
        }
    }
    void setCurrentHealth(int health)
    {
        if (health > currentHealth)
        {
            for (int i = currentHealth; i < health; i++)
            {
                hearts[i].sprite = fullHeart;
            }
        }
        else if(health < currentHealth)
        {
            for(int i = hearts.Count-1; i >= health; i--)
            {
                hearts[i].sprite = damagedHeart;
            }
        }
        currentHealth = health;
    }
}

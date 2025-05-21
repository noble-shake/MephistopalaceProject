using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Image ItemSprite;
    [SerializeField] private TMP_Text ItemDescription;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    public Sprite itemSprite { get { return ItemSprite.sprite; } set { ItemSprite.sprite = value; } }
    public string itemDescription { get { return ItemDescription.text; } set { ItemDescription.text = value; } }

    public void OnPanel(bool value)
    {
        if (value)
        {
            canvas.alpha = 1f;
        }
        else
        {
            canvas.alpha = 0f;
        }

    }
}

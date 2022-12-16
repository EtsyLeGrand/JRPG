using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PlayerFightUI : MonoBehaviour
{
    private bool isListingAttacks = false;
    private bool isListingAbilities = false;
    private bool isInUse = true;
    private CharacterInstance playerSource;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject secondaryLayout;
    [SerializeField] private Text titleTurn;

    [Header("Button")]
    [SerializeField] private Sprite buttonSprite;
    [SerializeField] private Sprite buttonPressedSprite;
    [SerializeField] private Font buttonFont;

    public bool IsListingAttacks { get => isListingAttacks; set => isListingAttacks = value; }
    public bool IsListingAbilities { get => isListingAbilities; set => isListingAbilities = value; }
    public bool IsInUse { get => isInUse; set => isInUse = value; }

    private void Awake()
    {
        Hide();
    }

    public void Hide()
    {
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
    }

    public void Show()
    {
        ClearSecondaryLayout();
        canvasGroup.interactable = true;
        canvasGroup.alpha = 255;
    }

    public void SetPlayerSource(CharacterInstance instance)
    {
        playerSource = instance;
        titleTurn.text = playerSource.character.rpgClass.className;
    }

    public void OnFleeButtonClicked()
    {

    }

    public void OnAttacksButtonClicked()
    {
        if (!isListingAttacks)
        {
            isListingAttacks = true;
            isListingAbilities = false;
            ClearSecondaryLayout();

            List<Skill> skills = playerSource.GetSkills();

            foreach (Skill skill in skills)
            {
                if (skill.manaCost == 0)
                {
                    GameObject btnObj = CreateButtonFromSkill(skill);
                    btnObj.GetComponent<Button>().onClick.AddListener(delegate {
                        Hide();
                        StartCoroutine(FightManager.Instance.WaitPlayerChooseTarget(playerSource, skill));
                        ClearSecondaryLayout();
                    });
                }
            }
        }
    }

    public void OnAbilitiesButtonClicked()
    {
        if (!isListingAbilities)
        {
            isListingAttacks = false;
            isListingAbilities = true;
            ClearSecondaryLayout();

            List<Skill> skills = playerSource.GetSkills();

            foreach (Skill skill in skills)
            {
                if (skill.manaCost > 0 && playerSource.currentStats.mana >= skill.manaCost)
                {
                    GameObject btnObj = CreateButtonFromSkill(skill);
                    btnObj.GetComponent<Button>().onClick.AddListener(delegate {
                        Hide();
                        StartCoroutine(FightManager.Instance.WaitPlayerChooseTarget(playerSource, skill));
                        ClearSecondaryLayout();
                    });
                }
            }
        }
    }

    private void ClearSecondaryLayout()
    {
        foreach (Transform child in secondaryLayout.transform)
        {
            Destroy(child.gameObject);
        }
        
        isListingAbilities = false;
        isListingAttacks = false;
    }

    private GameObject CreateButtonFromSkill(Skill skill)
    {
        GameObject buttonObj = new GameObject(skill.skillName);
        buttonObj.transform.parent = secondaryLayout.transform;

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.localScale = new Vector3(0.8f, 0.8f);
        rect.sizeDelta = new Vector2(160f, 30f);
        rect.pivot = new Vector2(.5f, .5f);

        buttonObj.AddComponent<CanvasRenderer>();

        Image img = buttonObj.AddComponent<Image>();
        img.sprite = buttonSprite;

        Button btn = buttonObj.AddComponent<Button>();
        btn.transition = Selectable.Transition.SpriteSwap;
        btn.spriteState = new SpriteState
        {
            pressedSprite = buttonPressedSprite
        };

        BoxCollider2D boxCollider = buttonObj.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(160f, 30f);


        GameObject buttonText = new GameObject("Text");
        buttonText.transform.parent = buttonObj.transform;

        RectTransform textRect = buttonText.AddComponent<RectTransform>();
        textRect.localScale = new Vector3(0.1f, 0.1f);
        textRect.sizeDelta = new Vector2(0, 0);
        textRect.anchorMin = new Vector2(0, 0);
        textRect.anchorMax = new Vector2(1, 1);
        textRect.anchoredPosition = new Vector2(0, 0);

        buttonText.AddComponent<CanvasRenderer>();

        Text text = buttonText.AddComponent<Text>();
        text.text = skill.skillName;
        if (skill.manaCost > 0) text.text += (" (" + skill.manaCost + " mana)");
        text.font = buttonFont;
        text.fontSize = 150;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        return buttonObj;
    }
}

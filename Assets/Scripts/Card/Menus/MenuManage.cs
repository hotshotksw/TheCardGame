using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManage : MonoBehaviour
{
    public static MenuManage Instance;

    public GameObject homePanel;
    public GameObject collectionPanel;
    public GameObject profilePanel;
    public GameObject favoritesPanel;
    public GameObject cardPullPanel;

    public Image homeImage, collectionImage, cardPullImage;
    public Sprite homeAltSprite, collectionAltSprite, cardPullAltSprite;
    private Sprite homeOriginalSprite, collectionOriginalSprite, cardPullOriginalSprite;

    private Dictionary<string, GameObject> menus;
    private Dictionary<string, Dictionary<string, string>> transitionAnimations;

    private static string targetMenu = "Home";
    private string currentMenu = "Home";

    [SerializeField] public Animator parentAnimator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Store original sprites
        if (homeImage) homeOriginalSprite = homeImage.sprite;
        if (collectionImage) collectionOriginalSprite = collectionImage.sprite;
        if (cardPullImage) cardPullOriginalSprite = cardPullImage.sprite;

        menus = new Dictionary<string, GameObject>
        {
            { "Home", homePanel },
            { "Collection", collectionPanel },
            { "Profile", profilePanel },
            { "Favorites", favoritesPanel },
            { "CardPull", cardPullPanel }
        };

        transitionAnimations = new Dictionary<string, Dictionary<string, string>>
        {
            { "Home", new Dictionary<string, string>
                {
                    { "Collection", "HomeToCollection" },
                    { "Profile", "HomeToProfile" },
                    { "Favorites", "HomeToFavorites" },
                    { "CardPull", "HomeToCardPull" }
                }
            },
            { "Collection", new Dictionary<string, string>
                {
                    { "Home", "CollectionToHome" },
                    { "Favorites", "CollectionToFavorites" },
                    { "CardPull", "CollectionToCardPull" }
                }
            },
            { "Profile", new Dictionary<string, string>
                {
                    { "Home", "ProfileToHome" }
                }
            },
            { "Favorites", new Dictionary<string, string>
                {
                    { "Home", "FavoritesToHome" }
                }
            },
            { "CardPull", new Dictionary<string, string>
                {
                    { "Home", "CardPullToHome" },
                    { "Collection", "CardPullToCollection" }
                }
            }
        };

        // Assign the Animator from the parent object (you can attach the Animator to the parent of all panels)
        //parentAnimator = GetComponent<Animator>();

        if (menus.ContainsKey(targetMenu))
        {
            menus[targetMenu].SetActive(true);
            UpdateImages("", targetMenu);
            OpenMenu(targetMenu);
        }
        else
        {
            menus["Home"].SetActive(true);
            UpdateImages("", "Home");
            OpenMenu("Home");
        }
    }

    public void OpenMenu(string menuName)
    {
        if (currentMenu == menuName || !menus.ContainsKey(menuName))
            return;

        StartCoroutine(SwitchMenu(menuName));
    }

    private IEnumerator SwitchMenu(string menuName)
    {
        GameObject current = menus[currentMenu];
        GameObject next = menus[menuName];

        if (parentAnimator && transitionAnimations.ContainsKey(currentMenu) && transitionAnimations[currentMenu].ContainsKey(menuName))
        {
            string transitionOut = transitionAnimations[currentMenu][menuName];
            next.SetActive(true);

            if (HasAnimatorTrigger(parentAnimator, transitionOut))
            {
                parentAnimator.SetTrigger(transitionOut);
            }

            yield return new WaitForSeconds(parentAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        current.SetActive(false);

        UpdateImages(currentMenu, menuName);
        currentMenu = menuName;
    }


    // Helper method to check if a trigger exists in the Animator
    private bool HasAnimatorTrigger(Animator animator, string triggerName)
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateImages(string oldMenu, string newMenu)
    {
        // Revert previous menu's image
        switch (oldMenu)
        {
            case "Home":
                if (homeImage) homeImage.sprite = homeOriginalSprite;
                break;
            case "Collection":
                if (collectionImage) collectionImage.sprite = collectionOriginalSprite;
                break;
            case "CardPull":
                if (cardPullImage) cardPullImage.sprite = cardPullOriginalSprite;
                break;
        }

        switch (newMenu)
        {
            case "Home":
                if (homeImage) homeImage.sprite = homeAltSprite;
                break;
            case "Collection":
                if (collectionImage) collectionImage.sprite = collectionAltSprite;
                break;
            case "CardPull":
                if (cardPullImage) cardPullImage.sprite = cardPullAltSprite;
                break;
        }
    }

    // Make this static so it persists across scene transitions
    public static void SetTargetMenu(string menuName) => targetMenu = menuName;
}

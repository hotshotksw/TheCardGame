using UnityEngine;
using UnityEngine.UI;

public class PFPManager : MonoBehaviour
{
    [Header("Profile Picture Buttons")]
    public Button profilePicButton1;
    public Button profilePicButton2;

    [Header("Profile Selection Panel")]
    public GameObject profileSelectionPanel;
    public Button[] profileOptions; // Buttons that represent available profile pictures

    [Header("Default Profile Picture")]
    public Sprite defaultProfilePic;
    public Vector2 defaultScale = new Vector2(1f, 1f);
    public Vector2[] profileScales1; // Array for scales for ProfilePic1
    public Vector2[] profileScales2; // Array for scales for ProfilePic2
    public Vector3 defaultPosition = Vector3.zero;
    public Vector3[] profilePositions1; // Array for positions for ProfilePic1
    public Vector3[] profilePositions2; // Array for positions for ProfilePic2

    private const string PFP_PREF_KEY = "SelectedProfilePic";

    void Start()
    {
        // Load saved profile picture or set default
        int savedIndex = PlayerPrefs.GetInt(PFP_PREF_KEY, 0);
        ChangeProfilePicture(savedIndex);

        // Assign click event to profile buttons to open the selection panel
        profilePicButton1.onClick.AddListener(() => ToggleProfileSelection(true));
        profilePicButton2.onClick.AddListener(() => ToggleProfileSelection(true));

        // Assign click events to profile options
        for (int i = 0; i < profileOptions.Length; i++)
        {
            int index = i; // Capture index for delegate
            profileOptions[i].onClick.AddListener(() => SelectProfilePicture(index));
        }
    }

    private void Update()
    {
        int savedIndex = PlayerPrefs.GetInt(PFP_PREF_KEY, 0);
        ChangeProfilePicture(savedIndex);
    }

    private void ToggleProfileSelection(bool isOpen)
    {
        profileSelectionPanel.SetActive(isOpen);
    }

    private void SelectProfilePicture(int index)
    {
        ChangeProfilePicture(index);
        PlayerPrefs.SetInt(PFP_PREF_KEY, index);
        PlayerPrefs.Save();
        ToggleProfileSelection(false);
    }

    private void ChangeProfilePicture(int index)
    {
        Sprite selectedSprite = (index >= 0 && index < profileOptions.Length) ?
                                profileOptions[index].image.sprite : defaultProfilePic;

        Vector2 selectedScale1 = (index >= 0 && index < profileScales1.Length) ?
                                profileScales1[index] : defaultScale;
        Vector2 selectedScale2 = (index >= 0 && index < profileScales2.Length) ?
                                profileScales2[index] : defaultScale;

        Vector3 selectedPosition1 = (index >= 0 && index < profilePositions1.Length) ?
                                profilePositions1[index] : defaultPosition;
        Vector3 selectedPosition2 = (index >= 0 && index < profilePositions2.Length) ?
                                profilePositions2[index] : defaultPosition;

        profilePicButton1.image.sprite = selectedSprite;
        profilePicButton2.image.sprite = selectedSprite;

        profilePicButton1.image.rectTransform.localScale = selectedScale1;
        profilePicButton2.image.rectTransform.localScale = selectedScale2;

        profilePicButton1.image.rectTransform.localPosition = selectedPosition1;
        profilePicButton2.image.rectTransform.localPosition = selectedPosition2;
    }
}

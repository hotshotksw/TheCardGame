using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class UIManager : MonoBehaviour
{
    public GameObject ProfileTop;
    public GameObject FilterUI;
    public GameObject FilterDropdown;

    public TextMeshProUGUI FilterText;

    public GameObject Inventory;

    public bool isFilterOpen = false;
    public bool isFilterDropdownOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeFilter(string filterName)
    {
        FilterText.text = filterName;
        ToggleFilterDropdown();
    }

    public void ToggleFilterUI()
    {
        if (isFilterOpen)
        {
            FilterUI.SetActive(false);
            ProfileTop.SetActive(true);
            isFilterDropdownOpen = false;
        }else
        {
            FilterUI.SetActive(true);
            ProfileTop.SetActive(false);
        }

        isFilterOpen = !isFilterOpen;
        ToggleCollection();
    }

    public void ToggleFilterDropdown()
    {
        if (isFilterDropdownOpen)
        {
            FilterDropdown.SetActive(false);
        }else
        {
            FilterDropdown.SetActive(true);
        }

        isFilterDropdownOpen = !isFilterDropdownOpen;
    }

    public void CloseFilter()
    {
        FilterDropdown.SetActive(false);
        FilterUI.SetActive(false);
        isFilterDropdownOpen=false;
        isFilterOpen=false;

        ProfileTop.SetActive(true);

        ToggleCollection();
    }

    public void ToggleCollection()
    {
        if(isFilterOpen)
        {
            Inventory.SetActive(true);
        }else
        {
            Inventory.SetActive(false);
        }
    }
}

using System;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ProfileTop;
    public GameObject FilterUI;
    public GameObject FilterDropdown;

    public TextMeshProUGUI FilterText;

    public GameObject Inventory;
    public InventoryRenderer inven_renderer;

    private bool isFilterOpen = false;
    private bool isFilterDropdownOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inven_renderer.ToggleDisplay(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewSetFilter(int filter)
    {
        switch (filter)
        {
            case 0:
                inven_renderer.cardFilter = InventoryRenderer.Filter.NONE;
                FilterText.text = "Filter: None"; // Update text here
                break;
            case 1:
                inven_renderer.cardFilter = InventoryRenderer.Filter.RARITY;
                FilterText.text = "Filter: Rarity"; // Update text here
                break;
            case 2:
                inven_renderer.cardFilter = InventoryRenderer.Filter.ARTIST;
                FilterText.text = "Filter: Artist"; // Update text here
                break;
            case 3:
                inven_renderer.cardFilter = InventoryRenderer.Filter.ID;
                FilterText.text = "Filter: ID"; // Update text here
                break;
            case 4:
                inven_renderer.cardFilter = InventoryRenderer.Filter.HOLLOW;
                FilterText.text = "Filter: Holographic"; // Update text here
                break;
        }
        ToggleFilterDropdown(false);
        inven_renderer.Render(Inventory.GetComponent<Inventory>());
    }


    public void SetFilter(InventoryRenderer.Filter filter)
    {
        inven_renderer.cardFilter = filter;
        ToggleFilterDropdown(false);
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
            inven_renderer.Render(Inventory.GetComponent<Inventory>());
        }

        isFilterOpen = !isFilterOpen;
        ToggleCollection();
    }

    public void ToggleFilterDropdown(bool val)
    {
        if (val)
        {
            FilterDropdown.SetActive(true);
        }else
        {
            FilterDropdown.SetActive(false);
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
        var invRenderer = Inventory.GetComponent<InventoryRenderer>();
        if(isFilterOpen)
        {
            invRenderer.ToggleDisplay(true);
        }else
        {
            invRenderer.ToggleDisplay(false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class EnumSelector : MonoBehaviour
{
    public enum DropDownType {searchable, normal}
    public DropDownType ddtype;
    public GameObject normalTemplate;
    private Dropdown dropdown;
    public GameObject searchableTemplate;
    private AutoCompleteComboBox comboBox;
    private AutoCompAddons comboBoxAddon;

    private GameObject InstantiatedBox;

    public class EnumSelectedEvent : UnityEngine.Events.UnityEvent<int>
    {
    }
    public EnumSelectedEvent selectionMade;

    public bool maskable = false;
    public bool OpenOnSetup = false;

    Type EnumType;
    private object _enum;
    private List<string> enumStrings;


    private NodeRegistration.NodeTypes types;
    // Start is called before the first frame update
    void Awake()
    {
        selectionMade = new EnumSelectedEvent();
        normalTemplate.SetActive(false);
        searchableTemplate.SetActive(false);
        //SetUpEnum(typeof(NodeRegistration.NodeTypes), types);
    }

    //used for the default dropdown
    public void OnSelect(int selection)
    {
        _enum = selection;
        selectionMade.Invoke((int)_enum);
        Destroy(InstantiatedBox);
        InstantiatedBox = null;
    }

    string currentSelection = "";
    //used for search dropdown. This is called every time text is changed. We don't want to immediatly select
    //when you type in an exact name so save it and apply with sumbit from global controls. 
    public void OnSelect(string selectionName, bool validName)
    {        
        if (validName)
        {
            currentSelection = selectionName;
        }
    }

    private void Submit()
    {
        if(currentSelection != "")
        {
            _enum = Enum.Parse(EnumType, currentSelection, true);
            Debug.Log("Submitted: " + _enum);
            selectionMade.Invoke((int)_enum);
            currentSelection = "";
            Destroy(InstantiatedBox);
            InstantiatedBox = null;
        }
    }

    public void SetUpEnum(Type _enumType, object _enum)
    {        
        if (_enumType.IsEnum)
        {
            this._enum = _enum;
            EnumType = _enumType;
            enumStrings = Enum.GetNames(_enumType).ToList();
            foreach(string s in enumStrings){
                //Debug.Log(s);
            }
        }
        else
        {
            Debug.LogError("Invalid data type. Not an enum");
        }
        if (OpenOnSetup)
        {
            CreateDropDown();
        }
    }

    public void OpenMenu()
    {
        if (InstantiatedBox == null)
        {
            CreateDropDown();
        }
    }

    private void CreateDropDown()
    {
        switch (ddtype)
        {
            case DropDownType.searchable:
                InstantiatedBox = Instantiate(searchableTemplate, transform);
                comboBox = InstantiatedBox.GetComponent<AutoCompleteComboBox>();
                comboBox.AvailableOptions = enumStrings;
                comboBox.OnSelectionChanged.AddListener(OnSelect);
                break;
            case DropDownType.normal:
                InstantiatedBox = Instantiate(normalTemplate, transform);
                dropdown = InstantiatedBox.GetComponent<Dropdown>();
                dropdown.ClearOptions();
                dropdown.AddOptions(enumStrings);
                dropdown.onValueChanged.AddListener(OnSelect);
                dropdown.value = (int)_enum;
                break;
            default:
                break;
        }
        SetMask(InstantiatedBox.transform, maskable);
        InstantiatedBox.SetActive(true);
    }

    private void SetMask(Transform root, bool setting)
    {        
        Image[] images;
        images = root.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.maskable = setting;
        }
    }

    void Close()
    {
        Destroy(InstantiatedBox);
        InstantiatedBox = null;
    }

    private void OnEnable()
    {
        GlobalInputDelagates.escape += Close;
        GlobalInputDelagates.select += Submit;
    }

    private void OnDisable()
    {
        GlobalInputDelagates.escape -= Close;
        GlobalInputDelagates.select += Submit;
    }
}


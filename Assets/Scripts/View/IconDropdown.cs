using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class IconDropdown : MonoBehaviour
{
    public StatusIcons icons;    

    private void Awake()
    {
        var dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(
            icons.icons.Select(
                img=>new Dropdown.OptionData(img)
            ).ToList()
        );
    }
}

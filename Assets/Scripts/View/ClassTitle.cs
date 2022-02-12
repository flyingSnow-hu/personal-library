using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassTitle : MonoBehaviour
{
    [SerializeField] Text className;

    public void SetName(string name)
    {
        className.text = name;
    }
}

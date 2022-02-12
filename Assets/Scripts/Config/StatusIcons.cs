using UnityEngine;

[CreateAssetMenu(fileName = "StatusIcons", menuName = "ScriptableObjects/StatusIcons", order = 1)]
public class StatusIcons : ScriptableObject
{
    [SerializeField]public Sprite[] icons;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ScriptableObject", menuName = "ScriptableObject/NewChestScriptableObjectList")]
public class ChestScriptableObjectList : ScriptableObject
{
    public ChestScriptableObject[] chestScriptableObjects;
}

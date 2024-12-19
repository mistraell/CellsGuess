using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DifficultyLevel", menuName = "ScriptableObject/DifficultyLevel", order = 1)]
public class DifficultyLevel : ScriptableObject
{
    [field: SerializeField] public string difficultyLevelName { get; private set; }
    [field: SerializeField] public Vector2Int difficultyLevelSize {get; private set;}
}

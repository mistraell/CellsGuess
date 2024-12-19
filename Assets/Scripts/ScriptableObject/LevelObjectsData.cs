using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelObjects", menuName = "ScriptableObject/LevelObjects")]
//Класс коллекции, хранящий пары спрайт/значение для последуюещго использования в генерации уровней
public class LevelObjectsData : ScriptableObject
{
    [field: SerializeField] public List<ObjectPair> objects {get; private set;}
}

[Serializable]
public class ObjectPair
{
    public Sprite sprite;
    public string value;
    public int rotationAngle;
}

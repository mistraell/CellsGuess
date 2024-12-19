using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class HintText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hintText;

    public void Initialize(ObjectPair objectPair)
    {
        _hintText.text = $"Find {objectPair.value}";
    }
}

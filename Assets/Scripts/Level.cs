using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private CellsGrid _cellsGrid;
    [SerializeField] private HintText _textHint;
    [SerializeField] private GameObject _finishScreen;
    private LevelData _levelData;
    private int _currentIteraton;

    private void Start()
    {
        RegenerateLevel();
    }

    private void UpdateGrid(bool firstTime)
    {
        _textHint.Initialize(_levelData.LevelIterations[_currentIteraton].RightAnswer);
        _cellsGrid.GenerateGrid(_levelData.LevelIterations[_currentIteraton].LevelVariants, _levelData.LevelIterations[_currentIteraton].RightAnswer, firstTime);
    }

    [ContextMenu("NextIteration")]
    public void NextIteration()
    {
        _currentIteraton++;
        if (_currentIteraton < _levelData.LevelIterations.Count)
        {
            UpdateGrid(false);
        }
        else
        {
            _cellsGrid.OnRightAnswerClicked -= NextIteration;
            _finishScreen.SetActive(true);
        }
    }

    public void RegenerateLevel()
    {
        _cellsGrid.ClearCells();
        _levelData = _levelGenerator.GenerateLevel();
        _currentIteraton = 0;
        UpdateGrid(true);
        _cellsGrid.OnRightAnswerClicked += NextIteration;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class CellsGrid : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private Transform _centerPosition;
    private List<Cell> _cells = new List<Cell>();
    public Action OnRightAnswerClicked;
    public void GenerateGrid(ObjectPair[,] grid, ObjectPair rightAnswer, bool firstTime)
    {
        Vector3 offset = Vector3.zero;
        int index = 0;
        foreach (Cell cell in _cells)
        {
            cell.RightAnswerClicked -= BlockCells;
        }
        for (int x = 0; x < grid.GetLength(1); x++)
        {
            offset.x = 0;
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                Cell cell;
                if (_cells.Count - 1 < index)
                {
                    cell = Instantiate(_cellPrefab, _content).GetComponent<Cell>();
                    _cells.Add(cell);
                }
                else
                {
                    cell = _cells[index];
                }

                cell.transform.localPosition = offset;
                cell.Initialize(grid[y, x], grid[y, x] == rightAnswer);
                cell.RightAnswerClicked += BlockCells;
                offset.x += cell.transform.localScale.x;
                index++;
                if (firstTime)
                    cell.Bounce();
            }
            offset.y += _cellPrefab.transform.localScale.y;
           
                
        }
        UnblockCells();
        transform.position = _centerPosition.position - offset / 2 + _cellPrefab.transform.localScale / 2; // НЕ магические числа offset на 2 = центр сетки, + центр префаба ячейки чтобы выровнять перед камерой

    }

    private void BlockCells()
    {
        foreach (Cell cell in _cells)
        {
            cell.BlockCell(true);
        }
        OnRightAnswerClicked?.Invoke();
    }

    private void UnblockCells()
    {
        foreach (Cell cell in _cells)
        {
            cell.BlockCell(false);
        }
        
    }

    public void ClearCells()
    {
        foreach (Cell cell in _cells)
        {
            cell.RightAnswerClicked -= BlockCells;
        }
        _cells.Clear();
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }
    }
}
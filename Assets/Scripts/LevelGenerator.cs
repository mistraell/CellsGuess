using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using Random = System.Random;
[CreateAssetMenu(fileName = "LevelGenerator", menuName = "ScriptableObject/LevelGenerator")]
public class LevelGenerator : ScriptableObject
{
    private List<ObjectPair>
        previouslyGeneratedObjects; // Список объектов, которые уже были выбраны как правильный ответ

    [SerializeField] private List<LevelObjectsData> _levelObjectsData;
    [SerializeField] private List<DifficultyLevel> _difficultyLevels;
    private static readonly Random random = new Random();

    private ObjectPair _rightAnswer;
    private ObjectPair[,] _levelVariants;
    
    public LevelData GenerateLevel()
    {
        List<LevelIteration> iterations = new List<LevelIteration>();
        foreach (var difficultyLevel in _difficultyLevels)
        {
            LevelObjectsData randomLevelObjectsData = GetRandomObject(_levelObjectsData);
            iterations.Add(GenerateLevelIteration(randomLevelObjectsData, difficultyLevel));
        }
        LevelData levelData = new LevelData(iterations);
        return levelData;
    }

    private LevelIteration GenerateLevelIteration(LevelObjectsData levelObjectsData, DifficultyLevel difficultyLevel)
    {
        if (levelObjectsData == null || !levelObjectsData.objects.Any())
        {
            throw new Exception("LevelObjectsData is not set or contains no objects.");
        }

        _rightAnswer = GenerateAnswer(levelObjectsData);

        _levelVariants =
            new ObjectPair[difficultyLevel.difficultyLevelSize.x, difficultyLevel.difficultyLevelSize.y];

        bool rightAnswerExsits = false;

        for (int i = 0; i < difficultyLevel.difficultyLevelSize.x; i++)
        {
            for (int j = 0; j < difficultyLevel.difficultyLevelSize.y; j++)
            {
                do
                {
                    _levelVariants[i, j] = GetRandomObject(levelObjectsData.objects);
                } while (rightAnswerExsits && _levelVariants[i, j] == _rightAnswer);

                if (_levelVariants[i, j] == _rightAnswer)
                    rightAnswerExsits = true;
            }
        }

        if (!rightAnswerExsits)
        {
            PlaceElementRandomly(_levelVariants, _rightAnswer);
        }

        return new LevelIteration(_levelVariants, _rightAnswer);
    }


    private void PlaceElementRandomly(ObjectPair[,] array, ObjectPair element)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        int randomRow = random.Next(rows);
        int randomCol = random.Next(cols);
        array[randomRow, randomCol] = element;
    }

    private ObjectPair GenerateAnswer(LevelObjectsData levelObjectsData)
    {
        ObjectPair rightAnswer;
        do
        {
            rightAnswer = GetRandomObject(levelObjectsData.objects);
        } while
            (previouslyGeneratedObjects.Contains(rightAnswer));

        previouslyGeneratedObjects.Add(rightAnswer);
        return rightAnswer;
    }

    public static T GetRandomObject<T>(IEnumerable<T> objects)
    {
        if (objects == null || !objects.Any())
        {
            throw new ArgumentException("The collection is empty or null.", nameof(objects));
        }
        int index = random.Next(objects.Count());
        return objects.ElementAt(index);
    }
}

[Serializable]
public class LevelIteration
{
    private ObjectPair _rightAnswer;
    private ObjectPair[,] _levelVariants;
    public ObjectPair[,] LevelVariants => _levelVariants;
    public ObjectPair RightAnswer => _rightAnswer;

    public LevelIteration(ObjectPair[,] levelVariants, ObjectPair rightAnswer)
    {
        _levelVariants = levelVariants;
        _rightAnswer = rightAnswer;
    }
}

[Serializable]
public class LevelData
{
    private List<LevelIteration> _levelIterations;
    public List<LevelIteration> LevelIterations => _levelIterations;

    public LevelData(List<LevelIteration> levelIterations)
    {
        _levelIterations = levelIterations;
    }
}
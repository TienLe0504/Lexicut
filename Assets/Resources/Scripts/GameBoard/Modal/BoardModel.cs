using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardModel 
{
    public CellModel[,] cells;
    public List<string> wordList = new List<string>();
    public List<Vector2> usedPoint;
    public Dictionary<Vector2, HashSet<string>> usedPositions = new Dictionary<Vector2, HashSet<string>>();
    public bool[,] testPosition;
    public HashSet<Vector2> usedPositionVector = new HashSet<Vector2>();
    public Dictionary<string, List<Vector2>> keyValuePairs = new Dictionary<string, List<Vector2>>();
    public List<string> answer = new List<string>();
    public List<CellController> currentChain = new List<CellController>();

    public void ResetData()
    {
        cells = null;
        wordList = new List<string>();
        usedPoint = new List<Vector2>();
        usedPositions = new Dictionary<Vector2, HashSet<string>>();
        testPosition = null;
        usedPositionVector = new HashSet<Vector2>();
        keyValuePairs = new Dictionary<string, List<Vector2>>();
        answer = new List<string>();
        currentChain = new List<CellController>();
    }
    public void CreateAnswer()
    {
        answer.Clear();
        foreach(string n in wordList)
        {
            answer.Add(n);
        }
    }
    public void RemoveWordAnswer( string word)
    {
        answer.Remove(word);
    }
    public void InitBoard(int rows, int columns)
    {
        cells = new CellModel[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                cells[i, j] = new CellModel { x = i, y = j, letter = ' ', isUsed = false };
            }
        }
    }
    public void InitUsePoint(int rows, int columns)
    {
        usedPoint = new List<Vector2>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                usedPoint.Add(new Vector2(i,j));
            }
        }
    }
    public void InitTestPosition(int rows, int columns)
    {
        testPosition = new bool[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                testPosition[i, j] = false;
            }
        }
    }
    public void GetListException(ref string director, ref int x, ref int y, ref HashSet<string> exceptDictionary)
    {
        if (director != "")
        {


            if (usedPositions.ContainsKey(new Vector2(x, y)))
            {
                HashSet<string> str = usedPositions[new Vector2(x, y)];
                if (str.Contains(""))
                {
                    usedPositions[new Vector2(x, y)].Remove("");
                }
                if (str.Contains(director))
                {
                    foreach (string s in str)
                    {
                        exceptDictionary.Add(s);
                    }
                }
                else
                {
                    usedPositions[new Vector2(x, y)].Add(director);
                    if (str.Count > 0)
                    {
                        foreach (string s in str)
                        {
                            exceptDictionary.Add(s);
                        }
                    }
                }
            }
            else
            {
                usedPositions.Add(new Vector2(x, y), new HashSet<string> { director });
                exceptDictionary.Add(director);
            }
        }
    }
    public void MergeVector()
    {
        if (usedPoint == null || usedPoint.Count == 0)
        {
            return;
        }

        HashSet<Vector2> uniqueVectors = new HashSet<Vector2>();
        List<Vector2> mergedList = new List<Vector2>();

        foreach (Vector2 point in usedPoint)
        {
            if (uniqueVectors.Add(point))
            {
                mergedList.Add(point);
            }
        }
        usedPoint = mergedList;

    }
    public int SwapCell(ref List<Vector2> checkPos, ref int lastIndex, ref Vector2 pos2, ref Vector2 value2)
    {
        checkPos[lastIndex] = value2;
        cells[(int)pos2.x, (int)pos2.y].isUsed = false;
        usedPositions[new Vector2(pos2.x, pos2.y)] = new HashSet<string>();
        usedPositions[new Vector2(value2.x, value2.y)] = new HashSet<string>();
        testPosition[(int)pos2.x, (int)pos2.y] = false;
        CellModel cell = new CellModel();
        cell.x = (int)value2.x;
        cell.y = (int)value2.y;
        cell.letter = cells[(int)pos2.x, (int)pos2.y].letter;
        cell.isUsed = true;
        cells[(int)value2.x, (int)value2.y] = cell;
        usedPoint.Add(new Vector2(pos2.x, pos2.y));
        usedPoint.Remove(new Vector2(value2.x, value2.y));
        lastIndex++;
        return lastIndex;
    }
    public void SetWord(string word, List<Vector2> positions, BoardView boardView)
    {
        for (int i = 0; i < word.Length; i++)
        {
            Vector2 pos = positions[i];
            int x = (int)pos.x;
            int y = (int)pos.y;

            if (x >= 0 && x < boardView.rows && y >= 0 && y < boardView.columns)
            {
                char upperChar = char.ToUpper(word[i]);

                CellModel cell = cells[x, y];
                cell.letter = upperChar;
                cell.isUsed = true;
                cells[x, y] = cell;
                Debug.Log($"Setting character '{word[i]}' at position ({x},{y})");
            }
            else
            {
                Debug.LogError($"Position ({x},{y}) for character {word[i]} is out of bounds");
            }
        }
    }

    public void FillEmptyCellsWithRandomLetters(BoardView boardView)
    {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        for (int x = 0; x < boardView.rows; x++)
        {
            for (int y = 0; y < boardView.columns; y++)
            {
                if (!cells[x, y].isUsed || cells[x, y].letter == ' ' || cells[x, y].letter == '\0')
                {
                    int randomIndex = UnityEngine.Random.Range(0, alphabet.Length);
                    char randomLetter = alphabet[randomIndex];

                    CellModel cell = cells[x, y];
                    cell.letter = randomLetter;
                    cell.isUsed = true;
                    cells[x, y] = cell;
                }
            }
        }
        for (int x = 0; x < boardView.rows; x++)
        {
            for (int y = 0; y < boardView.columns; y++)
            {
                cells[x, y].isUsed = false;
            }

        }
    }
    public void AddInKeyValue(string word, List<Vector2> checkPos)
    {
        foreach (Vector2 vector2 in checkPos)
        {
            if (!keyValuePairs.ContainsKey(word))
            {
                keyValuePairs[word] = new List<Vector2>();
            }
            keyValuePairs[word].Add(vector2);
        }
    }
    public void ResetListDot(ref List<Vector2> checkPos)
    {
        if (checkPos.Count > 0)
        {
            foreach (Vector2 pos in checkPos)
            {
                CellModel cell = cells[(int)pos.x, (int)pos.y];
                cell.isUsed = false;
                usedPositions[new Vector2(pos.x, pos.y)] = new HashSet<string>();
                usedPoint.Add(pos);
            }
        }
        MergeVector();
    }

}

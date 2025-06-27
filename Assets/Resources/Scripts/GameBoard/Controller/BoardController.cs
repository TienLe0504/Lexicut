using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public enum Direction
{
    Right,
    Left,
    Up,
    Down
}
public class BoardController : MonoBehaviour
{
    public BoardModel boardModel = new BoardModel();
    public BoardView boardView;
    public bool needReset = false;
    public int maxTried = 5;
    public int attempts = 0;
    public int resetCounter = 0;
    public const int max_reset = 3;
    public bool needIncreaseBoardSize = false;
    public string category;
    private void Awake()
    {
        boardView = GetComponent<BoardView>();
    }

    private void OnDisable()
    {
        this.Unregister(EventID.HanldeCell, RecieveCell);
        this.Unregister(EventID.playGameAgain, PlayGameAgagin);
        this.Unregister(EventID.showPopupEndGame, ShowPopupEndGame);
    }
    private void OnEnable()
    {
        this.Register(EventID.HanldeCell, RecieveCell);
        this.Register(EventID.playGameAgain, PlayGameAgagin);
        this.Register(EventID.showPopupEndGame, ShowPopupEndGame);
    }
    public void StartGame(string category)
    {
        boardModel.ResetData();
        ResetData();
        RepairData(category);
        CreateBoard();
        CreateBoardAgain();
    }
    public void PlayGameAgagin(object data)
    {
        boardModel.CreateAnswer();
        CreateBoard();
    }
    public void ResetData()
    {
        needReset = false;
        maxTried = 5;
        attempts = 0;
        resetCounter = 0;
        needIncreaseBoardSize = false;
        category = "";
    }
    private void CreateBoardAgain()
    {
        if (needIncreaseBoardSize)
        {

            needIncreaseBoardSize = false;
            needReset = false;
            IncreaseBoardSize();
            CreateBoard();
        }
        if (needReset)
        {
            needReset = false;
            needIncreaseBoardSize = false;
            CreateBoard();
        }
    }


    public void RecieveCell(object data)
    {
        if(data is CellController cell)
        {
            HandleCellSelected(cell);
        }
    }

    private void HandleCellSelected(CellController cell)
    {

        boardModel.currentChain.Add(cell);
    }
    
    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (boardModel.currentChain.Count > 0)
                AnswerDot();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (boardModel.currentChain.Count > 0)
                    AnswerDot();
            }
        }
    }
    private void CreateBoard()
    {

        boardModel.usedPositions = new Dictionary<Vector2, HashSet<string>>();
        boardModel.InitBoard(boardView.rows, boardView.columns);
        boardModel.InitUsePoint(boardView.rows, boardView.columns);
        boardModel.InitTestPosition(boardView.rows, boardView.columns);
        CreateCharFromWord();
        if (!needReset && !needIncreaseBoardSize)
        {
            boardView.CreateBoard(boardModel, this);

        }
        SendImage();
    }

    public void RepairData(string category)
    {
        this.category = category;
        List<string> selectedWords = SelectedWord(category);
        boardModel.wordList = selectedWords;
        boardModel.wordList.Sort((a, b) => b.Length.CompareTo(a.Length));
        boardModel.CreateAnswer();
        CalculateBoardSize();
    }
    private void CalculateBoardSize()
    {
        if (boardModel.wordList == null || boardModel.wordList.Count == 0)
            return;

        int totalChars = 0;
        foreach (string word in boardModel.wordList)
        {
            totalChars += word.Length;
        }
        if (totalChars <= 30)
        {
            boardView.rows = 7;
            boardView.columns = 7;
            return;
        }
        else if (totalChars <= 40)
        {
            boardView.rows = 8;
            boardView.columns = 7;
            return;
        }
        else if (totalChars <= 50)
        {
            boardView.rows = 8;
            boardView.columns = 8;
            return;
        }
        else if (totalChars <= 70)
        {
            boardView.rows = 9;
            boardView.columns = 9;
            return;
        }
        else
        {
            while (boardView.rows * boardView.columns < totalChars)
            {
                boardView.rows++;
                boardView.columns++;

            }
        }
    }
    public  List<string> SelectedWord(string category)
    {
        string path = CONST.PATH_DATA + category;
        List<string> listword = ResourceManager.Instance.LoadJson<List<string>>(path);

        List<string> shuffledWords = new List<string>(listword);
        int randomIndex = 0;
        for (int i = 0; i < shuffledWords.Count; i++)
        {
            randomIndex = UnityEngine.Random.Range(i, shuffledWords.Count);
            (shuffledWords[i], shuffledWords[randomIndex]) = (shuffledWords[randomIndex], shuffledWords[i]);
        }
        List<string> selectedWords = shuffledWords.GetRange(0, 5);
        
        return selectedWords;
    }


    public void ShowPopupEndGame(object data=null)
    {

        StartCoroutine(WaitToShowPoupGame());
    }
    IEnumerator WaitToShowPoupGame()
    {
        yield return new WaitForSeconds(0.5f);
        DataPopup dataPopup = new DataPopup();
        dataPopup.Setup(category, boardModel.wordList, boardModel.answer, SetUpGold());
        UIManager.Instance.ShowPopup<WinOrLosePopup>(dataPopup);
    }
    public int SetUpGold()
    {
        if (boardModel.answer.Count == 0)
        {
            return UnityEngine.Random.Range(100,120);

        }
        if (boardModel.answer.Count == 4)
        {
            return 20;
        }
        if (boardModel.answer.Count == 3)
        {
            return 40;
        }
        if (boardModel.answer.Count == 2)
        {
            return  UnityEngine.Random.Range(60, 70);
        }
        if (boardModel.answer.Count == 1)
        {
            return UnityEngine.Random.Range(70, 90);
        }
        return 0;
    }
    public void FalseSelect()
    {
        this.Broadcast(EventID.moves);
    }
    private void AnswerDot()
    {
        string selectedWord = "";
        foreach (CellController cell in boardModel.currentChain)
        {
            selectedWord += cell.cellmodel.letter;
        }

        string selectedWordLower = selectedWord.ToLower();
        string selectedWordReverse = ReverseString(selectedWordLower);

        bool found = false;
        string wordToRemove = null;
        foreach (string word in boardModel.answer)
        {
            string lowerWord = word.ToLower();
            if (lowerWord == selectedWordLower || lowerWord == selectedWordReverse)
            {
                found = true;
                wordToRemove = word;
                break;
            }
        }

        if (!found)
        {
            foreach (CellController cell in boardModel.currentChain)
            {
                cell.FalseDot();
            }
            FalseSelect();
        }
        if (found)
        {
            boardModel.RemoveWordAnswer(wordToRemove);
            effectColor dotColor = effectColor.Blue;
            bool random = false;
            foreach (CellController cell in boardModel.currentChain)
            {
                if (!random) {
                    dotColor = HelperColor.Instance.RandomColor(true);
                    random = true;
                }

                cell.cellmodel.SaveColor(HelperColor.Instance.GetColor(dotColor));
                cell.TrueDot(HelperColor.Instance.GetColor(dotColor));
            }
        }

        boardModel.currentChain.Clear();
        if (boardModel.answer.Count == 0)
        {
            ShowPopupEndGame();
        }
    }

    private string ReverseString(string input)
    {
        char[] array = input.ToCharArray();
        System.Array.Reverse(array);
        return new string(array);
    }

  
    public void SendImage()
    {
        if (boardModel?.wordList != null)
        {
            Dictionary<string, List<string>> value = new Dictionary<string, List<string>>();
            value[category] = boardModel.wordList;
            this.Broadcast(EventID.SendImage,value );
        }
    }


    public void CreateCharFromWord()
    {
        boardModel.keyValuePairs = new Dictionary<string, List<Vector2>>();
        bool clearChar = false;
        foreach (string word in boardModel.wordList)
        {
            boardModel.InitTestPosition(boardView.rows,boardView.columns);
            bool firstChar = true;
            clearChar = false;
            string director = "";
            List<Vector2> checkPos = new List<Vector2>();
            clearChar = CreateCell(word, ref firstChar,ref clearChar,ref checkPos, ref director);
            if (clearChar)
            {
                break;
            }
            else
            {
                boardModel.AddInKeyValue(word, checkPos);
            }
        }
        if (!clearChar)
        {
            CreateCharToCreateBoard();
        }
        else
        {
            CreateBoardAgain();
        }


    }

    public void CreateCharToCreateBoard()
    {
        boardModel.InitBoard(boardView.rows, boardView.columns);

        foreach (var wordEntry in boardModel.keyValuePairs)
        {
            string word = wordEntry.Key;
            List<Vector2> positions = wordEntry.Value;

            if (positions.Count != word.Length)
            {
                continue;
            }
            boardModel.SetWord(word, positions,boardView);
        }

        boardModel.FillEmptyCellsWithRandomLetters(boardView);

    }

  
    private bool CreateCell(string word, ref bool firstChar, ref bool clearChar,ref List<Vector2> checkPos,ref string director)
    {
        int i = 0;
        bool shouldRandom = false;
        foreach (char letter in word)
        {
            if (i >= 3)
            {
                shouldRandom = true;
            }
            Vector2 position = CreatePosition(ref firstChar,ref checkPos,ref director,ref shouldRandom);
            if (needIncreaseBoardSize || needReset)
            {
                clearChar = true;
                return clearChar;
            }
            i++;
            checkPos.Add(position);
            boardModel.usedPoint.Remove(position);
            CellModel cell = new CellModel
            {
                x = (int)position.x,
                y = (int)position.y,
                letter = letter,
                isUsed = true
            };
            boardModel.cells[cell.x, cell.y] = cell;
        }
        foreach(Vector2 vector2 in checkPos)
        {
            CellModel cell = boardModel.cells[(int)vector2.x, (int)vector2.y];
        }
        return clearChar;
    }

    public Vector2 CreatePosition(ref bool firstChar,ref List<Vector2> checkPos,ref string director,ref bool shouldRandom)
    {
        Vector2 position = default;
        if (firstChar==true)
        {
            firstChar = false;
            return CreateFirstChar(ref director, ref checkPos, false, out position);
        }
        else
        {
            (bool flowControl, Vector2 value) = CreateContinueDot(ref checkPos, ref director,ref shouldRandom, ref position);
            if (!flowControl)
            {
                return value;
            }

        }
        return position;
    }

    private (bool flowControl, Vector2 value) CreateContinueDot(ref List<Vector2> checkPos, ref string director,ref bool shouldRandom, ref Vector2 position)
    {
        int x = (int)checkPos[checkPos.Count - 1].x;
        int y = (int)checkPos[checkPos.Count - 1].y;

        if (shouldRandom)
        {
            director = CreateRandomDir(director);
        }
        (bool flowControl, Vector2 value) = GetVectorDirector(x, y, ref director);
        if (!flowControl)
        {
            return (flowControl: false, value: value);
        }
        
        HashSet<string> exceptDictionary = new HashSet<string>();
        boardModel.GetListException(ref director,ref x,ref y,ref exceptDictionary);
        while (flowControl)
        {
            (flowControl, value) = GetVectorDirector(x, y, ref director, true, exceptDictionary);
            if (!flowControl)
            {
                if (value == default)
                {
                    CreateAgainDot(ref checkPos, ref director, ref position);
                    if (needReset || needIncreaseBoardSize)
                    {
                        return (flowControl: false, value: default);
                    }
                    (flowControl, value) = CreateContinueDot(ref checkPos, ref director, ref shouldRandom, ref position);
                    if (needIncreaseBoardSize || needReset)
                    {
                        return (flowControl: false, value: default);
                    }
                    if (!flowControl)
                    {
                        return (flowControl: false, value: value);
                    }
                }
                else
                {
                    return (flowControl: false, value: value);
                }
            }
            else
            {
                if (director != "")
                {
                    boardModel.GetListException(ref director,ref x,ref y,ref exceptDictionary);
                }
            }
        }

        return (flowControl: true, value: default);
    }
    private void CreateAgainDot(ref List<Vector2> checkPos, ref string director, ref Vector2 position)
    {
        int lastIndex = checkPos.Count - 1;
        if (lastIndex + 1 > 0)
        {
            while (lastIndex < checkPos.Count)
            {
                if (lastIndex > 0)
                {
                    int frontindex = lastIndex - 1;
                    Vector2 pos1 = checkPos[frontindex];
                    Vector2 pos2 = checkPos[lastIndex];
                    Vector2 subtract = pos2 - pos1;
                    bool isUsed = true;
                    if (subtract.x == 1 && subtract.y == 0)
                    {
                        director = Direction.Right.ToString();
                    }
                    else if (subtract.x == -1 && subtract.y == 0)
                    {
                        director = Direction.Left.ToString();
                    }
                    else if (subtract.y == 1 && subtract.x == 0)
                    {
                        director = Direction.Up.ToString();
                    }
                    else if (subtract.y == -1 && subtract.x == 0)
                    {
                        director = Direction.Down.ToString();
                    }
                    else
                    {
                        isUsed = false;
                        director = CreateRandomDir(director);
                    }

                    HashSet<string> exceptDictionaryflow = new HashSet<string>();
                    int x = (int)pos1.x;
                    int y = (int)pos1.y;
                    if (isUsed)
                    {
                        boardModel.GetListException(ref director, ref x, ref y, ref exceptDictionaryflow);
                    }
                    (bool flowControl2, Vector2 value2) = GetVectorDirector((int)pos1.x, (int)pos1.y, ref director, isUsed, exceptDictionaryflow);
                    if (!flowControl2)
                    {
                        lastIndex = boardModel.SwapCell(ref checkPos, ref lastIndex, ref pos2, ref value2);
                        boardModel.MergeVector();
                    }
                    while (flowControl2)
                    {
                        (flowControl2, value2) = GetVectorDirector((int)pos1.x, (int)pos1.y, ref director, true, exceptDictionaryflow);
                        if (!flowControl2)
                        {
                            if (value2 == default)
                            {
                                lastIndex--;
                                continue;
                            }
                            lastIndex = boardModel.SwapCell(ref checkPos, ref lastIndex, ref pos2, ref value2);
                        }
                        else
                        {
                            if (director != "")
                            {
                                exceptDictionaryflow.Add(director);

                                boardModel.GetListException(ref director, ref x, ref y, ref exceptDictionaryflow);
                            }
                        }
                    }
                }
                else if (lastIndex == 0)
                {
                    Vector2 vector2 = checkPos[0];
                    List<Vector2> backupCheckPos = new List<Vector2>();
                    foreach (Vector2 n in checkPos)
                    {
                        backupCheckPos.Add(n);
                    }
                    CreateFirstChar(ref director, ref checkPos, true, out position);
                    if (needIncreaseBoardSize || needReset)
                    {
                        return;
                    }
                    checkPos = backupCheckPos;
                    lastIndex = boardModel.SwapCell(ref checkPos, ref lastIndex, ref vector2, ref position);
                    boardModel.MergeVector();
                }
            }

        }
    }
   
  
    private Vector2 CreateFirstChar(ref string director, ref List<Vector2> checkPos, bool firtDot, out Vector2 position)
    {
        int x=0, y=0;
        bool isUsed = false;
        bool check = false;
        Vector2 value = Vector2.zero;
        if (firtDot)
        {
            boardModel.ResetListDot(ref checkPos);
        }
        attempts = 0;
        
        maxTried = boardView.rows * boardView.columns - 2;
        do
        {
            attempts++;
            if(attempts > maxTried)
            {
                resetCounter++;
                if(resetCounter >= 3)
                {
                    
                    resetCounter = 0;
                    position = Vector2.zero;
                    needIncreaseBoardSize = true;
                    return position;
                }
                position = Vector2.zero;
                needReset = true;
                return position;
            }
            if (boardModel.usedPoint.Count <= 0)
            {
                needIncreaseBoardSize = true;
                position = Vector2.zero;
                return position;
            }
            int randomNumber = UnityEngine.Random.Range(0, boardModel.usedPoint.Count); 
            Vector2 randomPosition = boardModel.usedPoint[randomNumber];
            x = (int)randomPosition.x;
            y = (int)randomPosition.y;
            if (boardModel.testPosition[x, y])
            {
                continue;
            }
            if(checkPos.Contains(new Vector2(x, y)))
            {
                continue;
            }
 
            boardModel.testPosition[x, y] = true;
            isUsed = boardModel.cells[x, y].isUsed;
            isUsed = CheckCondition(x, y, isUsed);
            if (!isUsed)
            {
                position = new Vector2(x, y);
                return position;
            }
            (check ,value)  = GetAvailableDirections(x, y);

        } while (isUsed || !check);

        position = value;


        director = GetStrongestDirection(x, y); 
        return position;
    }
    private void IncreaseBoardSize()
    {
        boardView.rows = Mathf.Min(boardView.rows + 1, 12);
        boardView.columns = Mathf.Min(boardView.columns + 1, 12);
        resetCounter = 0;
        attempts = 0;
    }
    private (bool, Vector2) GetAvailableDirections(int x, int y)
    {
        if (x < boardView.rows - 1 && !boardModel.cells[x + 1, y].isUsed)
        {
            if(!CheckCondition(x+1,y, false)){
                return  (true,new Vector2(x + 1, y));
            }

        }
        if (x > 0 && !boardModel.cells[x - 1, y].isUsed)
        {
            if (!CheckCondition(x - 1, y, false))
            {
                return (true, new Vector2(x - 1, y));
            }
            
        }
        if (y < boardView.columns - 1 && !boardModel.cells[x, y + 1].isUsed)
        {
            if (!CheckCondition(x, y+1, false))
            {
                return (true, new Vector2(x, y + 1));
            }
            
        }
        if (y > 0 && !boardModel.cells[x, y - 1].isUsed)
        {
            if (!CheckCondition(x, y - 1, false))
            {
                return (true, new Vector2(x, y - 1));
            }
        }

        return (false,Vector2.zero);
    }

    private bool CheckCondition(int x, int y, bool isUsed)
    {
        if(x<boardView.rows-1 && y< boardView.columns-1 && x>0 && y>0)
        {
            if (boardModel.cells[x + 1, y].isUsed && boardModel.cells[x - 1, y].isUsed && boardModel.cells[x, y - 1].isUsed && boardModel.cells[x, y + 1].isUsed)
            {
                isUsed = true;
            }

        }

        return isUsed;
    }

    private string CreateRandomDir(string currentDirection)
    {
        int number = UnityEngine.Random.Range(0, 100);
        if (number >= 40)
        {
            List<string> allDirections = new List<string>
        {
            Direction.Right.ToString(),
            Direction.Left.ToString(),
            Direction.Up.ToString(),
            Direction.Down.ToString()
        };

            allDirections.Remove(currentDirection);

            int randomIndex = UnityEngine.Random.Range(0, allDirections.Count);
            return allDirections[randomIndex];
        }

        return currentDirection;
    }


    private (bool flowControl, Vector2 value) GetVectorDirector( int x, int y,ref string direction, bool checkDirector = false, HashSet<string>listDirection = null)
    {
        if (checkDirector) 
        {
            List<string> allDirections = new List<string> { Direction.Left.ToString(), Direction.Right.ToString(), Direction.Up.ToString(), Direction.Down.ToString() };
            if (listDirection.Count == 4)
            {
                return (flowControl: false, value: default);
            }
            foreach (string dir in listDirection)
            {
                allDirections.Remove(dir);
            }
            if (allDirections.Count > 0)
            {
                direction = allDirections[UnityEngine.Random.Range(0, allDirections.Count)];
            }
            else
            {
                direction = "";
            }
        }

        if (direction == Direction.Right.ToString())
        {
            if(x<boardView.rows - 1)
            {
                if (!boardModel.cells[x+1, y].isUsed)
                {
                    return (flowControl: false, value: new Vector2(x + 1, y));
                }
            }
        }
        else if (direction == Direction.Left.ToString())
        {
            if (x > 0)
            {
                if (!boardModel.cells[x - 1, y].isUsed)
                {
                    return (flowControl: false, value: new Vector2(x - 1, y));
                }
            }
        }
        else if (direction == Direction.Up.ToString())
        {
            if (y < boardView.columns - 1)
            {
                if (!boardModel.cells[x, y + 1].isUsed)
                {
                    return (flowControl: false, value: new Vector2(x, y + 1));
                }

            }
        }
        else if (direction == Direction.Down.ToString())
        {
            if (y > 0)
            {
                if (!boardModel.cells[x, y - 1].isUsed)
                {
                    return (flowControl: false, value: new Vector2(x, y - 1));
                }

            }
        }

        return (flowControl: true, value: default);
    }

    public string GetStrongestDirection(int x, int y)
    {
        int rowRight = CheckRows(x, y, Direction.Right.ToString());
        int rowLeft = CheckRows(x, y, Direction.Left.ToString());
        int columnUp = CheckColumns(x, y, Direction.Up.ToString());
        int columnDown = CheckColumns(x, y, Direction.Down.ToString());

        Dictionary<string, int> directions = new Dictionary<string, int>()
        {
            { Direction.Right.ToString(), rowRight },
            { Direction.Left.ToString(), rowLeft },
            { Direction.Up.ToString(), columnUp },
            { Direction.Down.ToString(), columnDown }
        };

        string maxDirection = null;
        int maxValue = int.MinValue;

        foreach (var kvp in directions)
        {
            if (kvp.Value > maxValue)
            {
                maxValue = kvp.Value;
                maxDirection = kvp.Key;
            }
        }
        if (maxValue == -1)
        {
            return "";
        }
        return maxDirection;
    }

    public int CheckRows(int x,int y, string direction = null)
    {
        int start = 0;
        int end = 0;
        if(direction == Direction.Left.ToString())
        {
            start = 0;
            end = x;
        }
        if (direction == Direction.Right.ToString()) {
            start = x + 1;
            end = boardView.rows;
        }
        if(x+1 < boardView.rows)
        {
            for(int i = start; i < end; i++)
            {
                if(boardModel.cells[i, y] != null && boardModel.cells[i, y].isUsed)
                {
                    return (int) math.abs( i-x);
                }
            }

        }

        return UnityEngine.Random.Range(0, boardView.rows - 1);

    }
    public int CheckColumns(int x, int y, string direction)
    {
        int start = 0;
        int end = 0;
        if (direction == Direction.Up.ToString())
        {
            start = y+1;
            end = boardView.columns;
        }
        if (direction == Direction.Down.ToString())
        {
            start = 0;
            end = y-1;
        }
        if (y + 1 < boardView.columns)
        {
            for (int i = start; i < end ; i++)
            {
                if (boardModel.cells[x, i] != null && boardModel.cells[x, i].isUsed)
                {
                    return (int) math.abs(i-y);
                }
            }
        }
        return UnityEngine.Random.Range(0, boardView.columns - 1);

    }
}

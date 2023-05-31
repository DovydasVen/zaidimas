using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public class Cell{
        public bool visited = false;
        public bool[] entrances = new bool[4];
    }

    public int roomCount = 0;
    public int roomsCompleted = 0;
    public GameObject gameController;
    public Vector2 size;
    public int startingPos = 0;
    List<Cell> board;
    public GameObject area;
    public Vector2 offset;

    private bool completed = false;

    void GenerateDungeon(){
        for(int i=0; i < size.x; i++){
            for(int j=0; j < size.x; j++){
                Cell currentCell = board[Mathf.FloorToInt(i+j*size.x)];
                if(currentCell.visited){
                    var newArea = Instantiate(area,new Vector3(i*offset.x,0,-j*offset.y), Quaternion.identity, transform).GetComponent<AreaController>();
                    newArea.InitRoom(board[Mathf.FloorToInt(i+j*size.x)].entrances);
                    newArea.name += " "+i+"-"+j;
                    if(Mathf.FloorToInt(i+j*size.x) == startingPos){
                        newArea.DisableTriggers();
                    }
                    roomCount++;
                }
            }
        }
    }

    List<int> CheckNeightbors(int cell){
        List<int> neighbors = new List<int>();

        //Check up neighbor
        //Ensure that the cell is not on the top row and the cell wasn't visited already
        if(cell - size.x >= 0 && board[Mathf.FloorToInt(cell-size.x)].visited == false){
            neighbors.Add(Mathf.FloorToInt(cell-size.x));
        }

        //Check down neighbor
        //Ensure that the cell is not on the bottom row and the cell wasn't visited already
        if(cell + size.x < board.Count && board[Mathf.FloorToInt(cell+size.x)].visited == false){
            neighbors.Add(Mathf.FloorToInt(cell+size.x));
        }

        //Check right neighbor
        //Ensure that the cell is not on the last collumn and the cell wasn't visited already
        if((cell+1) % size.x != 0 && board[Mathf.FloorToInt(cell+1)].visited == false){
            neighbors.Add(Mathf.FloorToInt(cell+1));
        }

        //Check left neighbor
        //Ensure that the cell is not on the first collumn and the cell wasn't visited already
        if(cell % size.x != 0 && board[Mathf.FloorToInt(cell-1)].visited == false){
            neighbors.Add(Mathf.FloorToInt(cell-1));
        }

        return neighbors;
    }

    void MazeGenerator(){
        board = new List<Cell>();
        for(int i = 0; i < size.x; i++){
            for(int j = 0; j < size.y; j++){
                board.Add(new Cell());
            }
        }

        int currentCell = startingPos;
        Stack<int> path = new Stack<int>();
        int k = 0;
        while(k < 35){
            k++;
            board[currentCell].visited = true;

            if(currentCell == board.Count-1) break;

            List<int> neighbors = CheckNeightbors(currentCell);
            if(neighbors.Count == 0){
                if(path.Count == 0){
                    break;
                } else {
                    currentCell = path.Pop();
                }
            } else {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];
                
                if(newCell > currentCell){
                    if(newCell - 1 == currentCell){
                        //Cell is going right
                        board[currentCell].entrances[2] = true;
                        currentCell = newCell;
                        board[currentCell].entrances[3] = true;
                    } else {
                        //Cell is going down
                        board[currentCell].entrances[1] = true;
                        currentCell = newCell;
                        board[currentCell].entrances[0] = true;
                    }
                } else {
                    if(newCell + 1 == currentCell){
                        //Cell is going left
                        board[currentCell].entrances[3] = true;
                        currentCell = newCell;
                        board[currentCell].entrances[2] = true;
                    } else {
                        //Cell is going up
                        board[currentCell].entrances[0] = true;
                        currentCell = newCell;
                        board[currentCell].entrances[1] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }

    void Update(){
        if(roomCount-1 == roomsCompleted && completed == false){
            gameController.GetComponent<GameController>().Victory();
            completed = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }
}

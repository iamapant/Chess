using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour {
    public ChessBoard() { }

    public Dictionary<Vector2Int, Square> BoardSquares { get; } = new();

    internal Blackboard SquareTemplates { get; } = new();


    public class Builder : IInitializationBoard, IPopulationBoard, IFinalBoard {
        private ChessBoard board;

        #region Initialization

        public IInitializationBoard AddPrefab(GameObject prefab) {
            if (prefab.TryGetComponent<ChessBoard>(out ChessBoard board)) {
                var objs = GameObject.FindObjectsOfType<ChessBoard>();
                if (objs.Length == 0) objs.ForEach(Destroy);
                Instantiate(prefab);
                this.board = board;
            }

            return this;
        }

        public IPopulationBoard InitializeBoard() {
            if (this.board == null) {
                BoardInitializeDefault();
            }

            SetTag();

            return this;
        }

        private void BoardInitializeDefault() { 
            var go = new GameObject();
            go.AddComponent<ChessBoard>();
            
            //Add scripts to create a default board here
            
            board = go.GetComponent<ChessBoard>();
        }

        private void SetTag() => board.tag = "ChessBoard";

        #endregion

        #region Population

        public IFinalBoard PopulateBoard() { throw new System.NotImplementedException(); }

        #endregion

        #region Finalization

        public ChessBoard Build() => board;

        #endregion
    }
}

public interface IInitializationBoard {
    IPopulationBoard InitializeBoard();
}

public interface IPopulationBoard {
    IFinalBoard PopulateBoard();
}

public interface IFinalBoard {
    ChessBoard Build();
}
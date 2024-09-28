using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChessBoard : MonoBehaviour {
    private ChessBoard() { }

    private void Start() {
        grid = GetComponent<Grid>();
    }

    private void OnMouseDown() { }

    private Grid grid;

    private Dictionary<Vector2Int, Square> boardSquares { get; } = new();
    public ReadOnlyDictionary<Vector2Int, Square> BoardSquares => new(boardSquares);

    public Vector2Int Size { get; set; } = new Vector2Int(8, 8);

    public void AddSquare(Square square, Vector2Int position) {
        if (boardSquares.ContainsKey(position)) RemoveSquare(position);

        var instantiate = Instantiate(square, transform, true);

        instantiate.name = position.ToString();
        instantiate.transform.localPosition = grid.CellToLocal(position.ToVector3Int());
        boardSquares.Add(position, instantiate);
    }

    public void RemoveSquare(Vector2Int position) {
        if (!boardSquares.TryGetValue(position, out var square)) return;

        boardSquares.Remove(position);
        Destroy(square.gameObject);
    }

    public bool TryGetSquareLocation(Square square, out Vector2Int location) {
        foreach (var keyPair in boardSquares) {
            if (keyPair.Value == square) {
                location = keyPair.Key;
                return true;
            }
        }
        
        
        location = default;
        return false;
    }

    /// <summary>
    /// Builder for the chessboard
    /// </summary>
    public class Builder : InitializationBoardBuilderAddTemplate, IInitializationBoardBuilder, IPopulationBoardBuilder, 
        IFinalBoardBuiler
         {
        private ChessBoard board;
        Vector2Int boardSize = new(8, 8);
        string tag = "ChessBoard";
        private Grid grid;

        #region Utilities

        private void RemoveDuplicate() {
            var boards = GameObject.FindGameObjectsWithTag(tag);
            foreach (var chessBoard in boards) {
                if (chessBoard.gameObject != boardObject) Destroy(chessBoard.gameObject);
            }
        }

        #endregion

        #region Initialization

        public Blackboard squareTemplates { get; } = new();
        private Dictionary<Vector2Int, BlackboardKey> templatePosition = new();
        private BlackboardKey currentTemplateKey;

        private GameObject boardObject;

        void RetrieveDataFromPrefab(GameObject prefab) {
            if (!prefab.TryGetComponent<ChessBoard>(out ChessBoard prefabBoard)) {
                prefabBoard = prefab.GetComponentInChildren<ChessBoard>();
                if (prefabBoard == null)
                    throw new ArgumentException("Prefab is missing a ChessBoard component");
            }

            this.board = prefabBoard;
            boardSize = prefabBoard.Size;
            grid = prefab.GetComponent<Grid>();
        }

        public IInitializationBoardBuilder WithPrefab(GameObject prefab) {
            if (prefab.GetComponent<ChessBoard>() == null && prefab.GetComponentInChildren<ChessBoard>() == null) {
                throw new ArgumentException("Prefab is missing a ChessBoard component");
            }

            boardObject = Instantiate(prefab) as GameObject;
            board = boardObject.GetComponent<ChessBoard>();

            RetrieveDataFromPrefab(prefab);
            RemoveDuplicate();
            return this;
        }

        public IInitializationBoardBuilder WithSize(int width, int height) {
            boardSize = new Vector2Int(width, height);

            return this;
        }

        public IInitializationBoardBuilder AddRule(SquareRule rule) {
            if (rule == null) throw new ArgumentNullException("rule");

            rule.BoardSize = boardSize;
            AddTemplate(rule.RuleName, rule.Square).WithPositions(rule);
            return this;
        }

        public InitializationBoardBuilderAddTemplate AddTemplate(string templateName, Square square) {
            var key = squareTemplates.GetOrRegister(templateName);
            if (squareTemplates.TryGetValue(key, out Square value))
                Debug.LogWarning($"Overriding template {templateName} with square {value.name}");

            squareTemplates.SetValue(key, square);
            currentTemplateKey = key;

            return this;
        }

        public override IInitializationBoardBuilder WithPositions([System.Diagnostics.CodeAnalysis.NotNull] params Vector2Int[] positions) {
            if (positions == null || positions.Length == 0) throw new ArgumentNullException(nameof(positions));
            foreach (var position in positions) {
                if (templatePosition.TryGetValue(position, out BlackboardKey key) && key != currentTemplateKey) {
                    Debug.LogWarning(
                        $"Overriding template {key.ToString()} with {currentTemplateKey.ToString()} at {position}");
                }

                if (!squareTemplates.TryGetValue(key, out Square square))
                    throw new NullReferenceException($"Template not found {key.ToString()}.");
                templatePosition[position] = currentTemplateKey;
            }

            return this;
        }

        private void InitializeDefaults() {
            boardObject = new GameObject("ChessBoard");
            board = boardObject.AddComponent<ChessBoard>();
            boardObject.AddComponent<Grid>();
        }

        public IPopulationBoardBuilder InitializeBoard() {
            if (this.board == null) InitializeDefaults();

            //Code to set up the new board here
            CleanupBoard();
            SetBoardSize();
            GenerateNewBoard();

            SetTag();
            RemoveDuplicate();
            return this;
        }

        private void GenerateNewBoard() {
            foreach (var position in templatePosition.Keys) {
                if (squareTemplates.TryGetValue(templatePosition[position], out Square square)) {
                    var instance = Instantiate(square, grid?.CellToWorld(position.ToVector3Int()) ?? Vector3.zero, Quaternion.identity);
                    board.AddSquare(instance, position);
                    instance.Initialize(board);
                }
            }
        }

        private void CleanupBoard() =>
            board.gameObject.GetComponentsInChildren<Square>(true).ForEach(e => Destroy(e.gameObject));

        private void SetBoardSize() => board.Size = boardSize;

        private void SetTag() => board.tag = tag;

        #endregion

        #region Population

        public IPopulationBoardBuilder AddEntity(Entity entity, params Vector2Int[] positions) {
            foreach (var position in positions) {
                if (!board.BoardSquares.TryGetValue(position, out Square square)) {
                    Debug.LogError($"Cannot add entity {entity.name} to {position.ToString()}");
                    continue;
                }

                Instantiate(entity);
                entity.MoveSquare(square);
            }

            return this;
        }

        public IFinalBoardBuiler PopulateBoard() {
            return this;
        }

        #endregion

        #region Finalization

        public ChessBoard Build() {
            if (this.board == null) throw new NullReferenceException("Board is not initialized!");
            RemoveDuplicate();

            return board;
        }

        #endregion
    }
}

public interface IInitializationBoardBuilder {
    IPopulationBoardBuilder InitializeBoard();
}

public interface IPopulationBoardBuilder {
    IFinalBoardBuiler PopulateBoard();
}

public interface IFinalBoardBuiler {
    ChessBoard Build();
}

public abstract class InitializationBoardBuilderAddTemplate {
    Blackboard squareTemplates { get; }
    public abstract IInitializationBoardBuilder WithPositions(params Vector2Int[] positions);
}
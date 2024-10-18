using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class GameController : PersistentSingleton<GameController> {
    public delegate void onUpdateCallback();
    public event onUpdateCallback OnUpdateCallback;
    
    #region Chessboard Configuration
    [Header("Chessboard Configuration")]
    [SerializeField] private Vector2Int boardSize;
    [SerializeField] GameObject chessboardPrefab;
    [Tooltip("Default prefab")]
    [SerializeField] Square defaultSquarePrefab;
    [Tooltip("(Optional) Prefab used to instantiate the chessboard")]
    public ChessBoard ChessBoard { get; private set; }
    [Tooltip("Rule for where each square should be placed")]
    [SerializeReference, SubclassSelector] private List<SquareRule> rules;
    [Tooltip("Rule for where each square should be placed")]
    [SerializeField] private SerializedDictionary<Square, List<Vector2Int>> templates;
    
    [Space(10)]
    [SerializeField] SerializedDictionary<Entity, List<Vector2Int>> entities;
    #endregion

    #region Square Template
    [Space(20)]
    [Header("Square Template")]
    [SerializeField]SerializedDictionary<string, SquareTemplate> SquareTemplates;
    public SquareTemplateController SquareTemplateController;
    #endregion
    
    #region Turn Configuration
    [Space(20)]
    [Header("Turn configuration")]
    [SerializeReference, SubclassSelector] private List<Turn> turns;
    public TurnManager TurnManager { get; private set; }
    #endregion

    #region Log
    public GameActionLogger GameActionLogger { get; private set; }    
    #endregion

    #region Entity Movement
    [Space(20)]
    [Header("Square Template")]
    public EntityMovementController EntityMovementController;
    #endregion
    
    [Space(20)] [Header("Test")] 
    [SerializeField] private Entity entity;
    [SerializeField] private Square square;

    protected override void Awake() {
        base.Awake();
        TurnManager = new TurnManager(turns.ToArray());
        ChessBoard = BuildChessBoard();
        SquareTemplateController = new SquareTemplateController();
        GameActionLogger = new GameActionLogger();
        
    }

    private ChessBoard BuildChessBoard() {
        var builder =  new ChessBoard.Builder();
        
        if (defaultSquarePrefab != null) builder.WithDefaultSquare(defaultSquarePrefab);
        rules.ForEach((e) => {
            e.BoardSize = boardSize;
            
            builder.AddRule(e);
        });
        templates.ForEach(e => builder.AddTemplate(e.Key.name,e.Key).WithPositions(e.Value.ToArray()));

        builder.InitializeBoard();
        entities.ForEach(e => builder.AddEntity(e.Key, e.Value.ToArray()));
        
        return builder.Build();
    }

    private void Update() {
        OnUpdateCallback?.Invoke();
    }

    private void Start() {
        
        // if (!FindObjectOfType<Square>())square = Instantiate(square);
        // var pre1 = Instantiate(entity);
        // var pre2 = Instantiate(entity);
        //
        // pre1.name = "pre1";
        // pre2.name = "pre2";
        //
        // pre1.AddModifier<HelloModifier>();
        // square.AddModifier<HelloModifier>();
        //
        // pre1.MoveSquare(square);
        // pre2.MoveSquare(square);
    }
}

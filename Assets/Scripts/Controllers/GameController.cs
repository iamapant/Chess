using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : PersistentSingleton<GameController> {
    [Header("Chessboard configuration")]
    [SerializeField] GameObject chessboardPrefab;
    public ChessBoard ChessBoard { get; private set; }
    [SerializeReference, SubclassSelector] private List<SquareRule> rules;
    
    [Space(20)]
    [Header("Turn configuration")]
    [SerializeReference, SubclassSelector] private List<Turn> turns;
    public TurnManager TurnManager { get; private set; }

    protected override void Awake() {
        TurnManager = new TurnManager(turns.ToArray());
        ChessBoard = BuildChessBoard();
    }

    private ChessBoard BuildChessBoard() {
        throw new NotImplementedException();
    }
}

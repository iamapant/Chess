using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : PersistentSingleton<GameController> {
    [SerializeField] private List<Turn> turns;
    public TurnManager TurnManager { get; private set; }
    public ChessBoard board { get; private set; }
    [SerializeField] GameObject chessboardPrefab;
    
    private void Awake() {
        TurnManager = new TurnManager(turns.ToArray());
        board = BuildChessBoard();
    }

    private ChessBoard BuildChessBoard() {
        throw new NotImplementedException();
    }
}

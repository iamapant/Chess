using System;
using UnityEngine;

[Serializable]
public class SquareRuleList {
    [SerializeReference, SubclassSelector] public SquareRule AddRule;
}
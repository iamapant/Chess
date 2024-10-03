using System;
using UnityEngine;

[Serializable]
public abstract class Feature {
    [SerializeField] public RenderSupplier renderSupplier;
    [SerializeField] public Sprite Image;
    [SerializeField] public string Name;
    [SerializeField] public string Desciption;
    
    
}

[Serializable]
public class EmptyFeature : Feature {}
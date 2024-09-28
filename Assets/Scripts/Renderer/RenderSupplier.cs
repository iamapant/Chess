using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public abstract class RenderSupplier {
    [HideInInspector] public GameObject RenderObject;

    public virtual void Render() {
        if (!RenderObject) throw new NullReferenceException();
    }
}

[Serializable]
public class NoRender : RenderSupplier {
    public override void Render() { }
}

[Serializable]
public class RenderSupplier2D : RenderSupplier {
    public Sprite Sprite;

    public override void Render() {
        base.Render();

        var sr = RenderObject.GetOrAddComponent<SpriteRenderer>();
        
        sr.sprite = Sprite;
    }
}

[Serializable]
public class RenderSupplier3D : RenderSupplier {
    public override void Render() {
        throw new NotImplementedException();
    }
}
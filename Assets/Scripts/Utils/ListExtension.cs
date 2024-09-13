using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public static class ListExtension {
    public static void AddAfter<T>(this List<T> list, T item, T after) {
        if (!list.Contains(item)) throw new Exception($"Item {after.GetType()} is not in the list");
        List<T> temp = list;
        list.Clear();
        foreach (var i in temp) {
            list.Add(i);
            if (i.Equals(after)) list.Add(item);
        }
    }

    public static void AddAfter<T, TAfter>(this List<T> list, T item, bool forAllCopy = false) where TAfter : T {
        if (!list.Contains(item)) throw new Exception($"Item {typeof(TAfter)} is not in the list");

        bool shouldAdd = true;

        List<T> temp = list;
        list.Clear();
        foreach (var i in temp) {
            list.Add(i);
            if (i is TAfter && shouldAdd) {
                list.Add(item);
                if (!forAllCopy) shouldAdd = false;
            }
        }
    }

    public static void AddBefore<T>(this List<T> list, T item, T before) {
        if (!list.Contains(item)) throw new Exception($"Item {before.GetType()} is not in the list");
        List<T> temp = list;
        list.Clear();
        foreach (var i in temp) {
            if (i.Equals(before)) list.Add(item);
            list.Add(i);
        }
    }

    public static void AddBefore<T, TBefore>(this List<T> list, T item, bool forAllCopy = false) where TBefore : T {
        if (!list.Contains(item)) throw new Exception($"Item {typeof(TBefore)} is not in the list");

        bool shouldAdd = true;

        List<T> temp = list;
        list.Clear();
        foreach (var i in temp) {
            if (i is TBefore && shouldAdd) {
                list.Add(item);
                if (!forAllCopy) shouldAdd = false;
            }

            list.Add(i);
        }
    }
}
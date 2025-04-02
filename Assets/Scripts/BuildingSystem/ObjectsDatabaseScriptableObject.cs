using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ObjectsDatabaseScriptableObject : ScriptableObject
{
    public List<DatabaseObject> objectsData;
}

[Serializable]
public class DatabaseObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public int ID { get; private set; }

    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField] public GameObject Prefab { get; private set; }
    
    [field: SerializeField] public int Cost { get; private set; }
    
    [field: SerializeField] public int RequiredLevel { get; private set; }

    [field: SerializeField] public bool Placable;
    [field: SerializeField] public Sprite icon;


}

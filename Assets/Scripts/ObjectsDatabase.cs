using UnityEngine;

public class ObjectsDatabase : MonoBehaviour
{
    public static ObjectsDatabase Instance;
    [SerializeField] private ObjectsDatabaseScriptableObject database;

    private void Awake()
    {
        Instance = this;
    }

    public DatabaseObject FindObjectByID (int ID)
    {
        int selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        return database.objectsData[selectedObjectIndex];
    }

    public DatabaseObject FindObjectByIndex(int Index)
    {
        return database.objectsData[Index];
    }

    public ObjectsDatabaseScriptableObject GetObjectsDatabase()
    {
        return database;
    }

}

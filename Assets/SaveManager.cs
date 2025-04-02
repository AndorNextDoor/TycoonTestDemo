using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private Transform loadMenuContainer;
    [SerializeField] private GameObject loadFilePrefab;

    [SerializeField] private List<GuardianSavable> playersGuardians;

    [SerializeField] private Transform player;

    [SerializeField] private GameObject scene;
    [SerializeField] private GameObject mainMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void AddSavableGuardian(GameObject newGuardian, Vector3 _startPos)
    {
        GuardianSavable _guardian = new GuardianSavable
        {
            guardian = newGuardian,
            startPos = _startPos,
        };
        playersGuardians.Add(_guardian);
    }

    public void RemoveSavableGuardian(object sender,GameObject newGuardian)
    {
        int i = 0;
        foreach (GuardianSavable _object in playersGuardians)
        {
            if (_object.guardian.GetInstanceID() == newGuardian.GetInstanceID())
            {
                playersGuardians.RemoveAt(i);
                break;
            }
            i++;
        }
    }

    public void Save()
    {
        Vector3 playerPosition = player.position;
        int moneyAmount = ResourcesManager.Instance.GetCurrentGold();

        SaveObject saveObject = new SaveObject
        {
            goldAmount = moneyAmount,
            playerPosition = playerPosition,
            playerGuardians = playersGuardians,
        };
        
        string json = JsonUtility.ToJson(saveObject);

        SaveSystem.Save(json);

    }


    public void Load()
    {

        string saveString = SaveSystem.Load(); 
        if(saveString != null)
        {

            SaveObject saveObject =  JsonUtility.FromJson<SaveObject>(saveString);

            player.transform.position = saveObject.playerPosition;
            ResourcesManager.Instance.SetCurrentGold(saveObject.goldAmount);

            foreach (GuardianSavable _guardians in saveObject.playerGuardians)
            {
                Instantiate(_guardians.guardian, _guardians.startPos, Quaternion.identity);
            }
            
        }
    }

    public void Load(string saveString)
    {
        scene.SetActive(true);
        mainMenu.SetActive(false);

        if (saveString != null)
        {

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            player.transform.position = saveObject.playerPosition;
            ResourcesManager.Instance.SetCurrentGold(saveObject.goldAmount);

            foreach (GuardianSavable _guardians in saveObject.playerGuardians)
            {
                Instantiate(_guardians.guardian, _guardians.startPos, Quaternion.identity);
            }

        }
    }

    public void GenerateAllLoadButtons()
    {
        FileInfo[] saveFiles = SaveSystem.GetAllSaveFiles();

        foreach(Transform child in loadMenuContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (FileInfo fileInfo in saveFiles)
        {
            GameObject newLoad = Instantiate(loadFilePrefab, loadMenuContainer);
            newLoad.GetComponentInChildren<TextMeshProUGUI>().text = "SAVE FILE FROM: " + fileInfo.LastAccessTime;

            string saveString = File.ReadAllText(fileInfo.FullName);
            newLoad.GetComponentInChildren<Button>().onClick.AddListener(() => Load(saveString));
        }
    }
}

[Serializable]
public class SaveObject
{
    public int goldAmount;
    public Vector3 playerPosition;
    public List<GuardianSavable> playerGuardians;
}

[Serializable]
public class GuardianSavable
{
    public GameObject guardian;
    public Vector3 startPos;
}

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static readonly string K_FILENAME = "/SaveData.dat";

    [SerializeField]
    private SaveChannel m_SaveChannel;

    private void Awake()
    {
        m_SaveChannel.OnRequestSave += SaveGame;
    }

    private void OnDestroy()
    {
        m_SaveChannel.OnRequestSave -= SaveGame;
    }

    public static bool HasASave()
    {
        return File.Exists(Application.persistentDataPath + K_FILENAME);
    }

    public void SaveGame()
    {
        using (FileStream file = File.Create(Application.persistentDataPath + K_FILENAME))
        {
            string data = "Just dump your save data in some serializable classes.";
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
        }
    }

    public void LoadGame()
    {
        if (HasASave())
        {
            using (FileStream file = File.OpenRead(Application.persistentDataPath + K_FILENAME))
            {
                BinaryFormatter bf = new BinaryFormatter();
                string data = (string)bf.Deserialize(file);
            }
        }
    }
}
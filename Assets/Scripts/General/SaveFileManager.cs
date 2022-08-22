using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Defines saving and loading progress and save file management.
/// </summary>
public class SaveFileManager : Singleton<SaveFileManager>
{
    private GameData gameData;
    private PlayerData playerData;

    protected override void Awake()
    {
        base.Awake();

        // Load general game data
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
        gameData = (GameData)bf.Deserialize(file);
        file.Close();
    }

    private void Update()
    {
        if (playerData != null)
        {
            playerData.playTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Save all application data that is outside the game.
    /// </summary>
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
        bf.Serialize(file, gameData);
        file.Close();
    }

    #region Character Save File

    /// <summary>
    /// Creates a new character.
    /// </summary>
    public void NewCharacter()
    {
        playerData = new PlayerData();
    }

    /// <summary>
    /// Saves the current character into their respective save file.
    /// </summary>
    public void SaveCharacter()
    {
        if (playerData != null)
        {
            string filePath = Application.persistentDataPath + String.Format("/playerInfo_{0}.dat", playerData.id);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePath);
            bf.Serialize(file, playerData);
            file.Close();

            // If first save for a character, add to save files
            if (gameData.saveFiles.ToList().TrueForAll(saveFile => saveFile != filePath))
            {
                gameData.saveFiles.Add(filePath);
            }
        }
        
        SaveGame();
    }

    /// <summary>
    /// Unloads the character (when exiting to title screen).
    /// </summary>
    public void UnloadCharacter()
    {
        SaveCharacter();
        playerData = null;
    }

    /// <summary>
    /// Loads the character saved in the specified slot.
    /// </summary>
    public void LoadCharacter(int slot)
    {
        string filePath = GetSaveFilePaths()[slot];

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);
        playerData = (PlayerData)bf.Deserialize(file);
        file.Close();
    }

    /// <summary>
    /// Deletes the character saved in the specified slot.
    /// </summary>
    public void DeleteCharacter(int slot)
    {
        string filePath = GetSaveFilePaths()[slot];
        gameData.saveFiles.Remove(filePath);
        SaveGame();

        File.Delete(filePath);
    }

    #endregion

    #region Save Files

    /// <summary>
    /// Returns a list of the save file paths in order of creation.
    /// </summary>
    private List<string> GetSaveFilePaths()
    {
        return gameData.saveFiles.ToList()
                                 .OrderBy(path => int.Parse(path.Split('_')[1]))
                                 .ToList();
    }

    /// <summary>
    /// Returns a list of the name, level, and playtime for each character's save file.
    /// </summary>
    private List<List<object>> GetSaveFileProfiles()
    {
        List<List<object>> profiles = new List<List<object>>();

        foreach (string path in GetSaveFilePaths())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            PlayerData data = ((PlayerData)bf.Deserialize(file));

            List<object> profile = new List<object>();
            profile.Add(data.playerName);
            profile.Add(data.playerLvl);
            profile.Add(data.playTime);
            profiles.Add(profile);
            file.Close();
        }

        return profiles;
    }

    #endregion
}


/// <summary>
/// Save data for the entire game.
/// </summary>
[Serializable]
class GameData
{
    public HashSet<string> saveFiles = new HashSet<string>();
}


/// <summary>
/// Save data for a character.
/// </summary>
[Serializable]
class PlayerData
{
    #region Character ID

    private static int idCounter = 0;
    public int id = 0;

    public PlayerData()
    {
        id = idCounter;
        idCounter++;
    }

    #endregion

    public string playerName;
    public int playerLvl;
    public float playTime;
}
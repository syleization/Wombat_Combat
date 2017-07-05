using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainMenuStartUp : MonoBehaviour
{
    public string startMusic;
    private string FolderPath;

    private bool RunTutorial = false;

    public GameObject PopUpUI;

    private void Start()
    {
        SoundManager.Instance.PlaySound(startMusic);

        FolderPath = SaveLoad.DocumentsPath(SaveLoad.FolderName);
        //Debug.Log(FolderPath);

        if (Directory.Exists(FolderPath))
        {
            if (File.Exists(FolderPath + "/" + SaveLoad.Filename))
            {
                RunTutorial = false;

                string reader = SaveLoad.ReadStringFromFile(SaveLoad.FolderName + "/" + SaveLoad.Filename);
                char[] spliter = { ':' };
                string[] data = reader.Split(spliter, System.StringSplitOptions.RemoveEmptyEntries);

                SoundManager.Instance.MasterVolume = System.Single.Parse(data[0]);
                SoundManager.Instance.MusicVolume = System.Single.Parse(data[1]);
                SoundManager.Instance.EffectVolume = System.Single.Parse(data[2]);
                SoundManager.Instance.ResetVolumes();
            }
            else
            {
                RunTutorial = true;

                SaveLoad.WriteStringToFile("1:1:1", SaveLoad.FolderName + "/" + SaveLoad.Filename);
            }
        }
        else
        {
            RunTutorial = true;

            Directory.CreateDirectory(FolderPath);
            SaveLoad.WriteStringToFile("1:1:1", SaveLoad.FolderName + "/" + SaveLoad.Filename);
        }

        if (RunTutorial)
        {
            PopUpUI.SetActive(true);
        }
    }
}

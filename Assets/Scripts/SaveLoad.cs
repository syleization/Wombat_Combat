using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public static class SaveLoad
{
    public static string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "//WombatCombat";

    public static bool DoesDirExist()
    {
        if (Directory.Exists(path) && File.Exists(path + "//Save.xml"))
            return true;
        else
            return false;
    }

    public static bool CreateDir()
    {
        Directory.CreateDirectory(path);
        SaveClient();
        return true;
    }

    public static bool SaveClient()
    {
        using (XmlWriter writer = XmlWriter.Create(path + "//Save.xml"))
        {
            writer.WriteStartDocument();                //Start
            writer.WriteStartElement("WombatCombat");   //Main Start

            //Add things to save here
            writer.WriteElementString("Coins", CurrencyManager.CurrentCoins.ToString());    //[0]
            writer.WriteElementString("Gems", CurrencyManager.CurrentGems.ToString());      //[1]

            writer.WriteEndElement();                   //Main End
            writer.WriteEndDocument();                  //End
        }

        return true;
    }

    public static bool LoadClient()
    {
        XmlDocument loader = new XmlDocument();
        loader.Load(path + "//Save.xml");

        CurrencyManager.CurrentCoins = System.Convert.ToInt32(loader.ChildNodes[1].ChildNodes[0].InnerText);
        CurrencyManager.CurrentGems = System.Convert.ToInt32(loader.ChildNodes[1].ChildNodes[1].InnerText);

        return true;
    }
}

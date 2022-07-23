﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace JavaScriptInterpreter
{
  static public class Tools
  {
    static public void AddExcludedImagesInFolderToMetaFile()
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      string[] tempImagesInFolder = Directory.GetFiles(MetaFileManager.Instance.RootFolder, "*.jpg");

      Regex regexIsValidFileName = new Regex(@"(.*\/)\d\..*"); // check if it matches the 1.jpg format
      Regex regexIsJpg = new Regex(@".*.jpg"); // check if is jpg

      foreach (string imName in tempImagesInFolder)
      {
        // Handle image that has invalid name and does not exist in metadata
        if (regexIsJpg.IsMatch(imName) && !regexIsValidFileName.IsMatch(imName))
        {
          LiamDebugger.Message($"imName is jpg and IS NOT valid filename {imName}, adding to database and renaming file", 2);
          int newFileNum = ItterateToNonExistingItem();

          string fromFile = $"{imName}";
          string toFile = $"{MetaFileManager.Instance.RootFolder}{newFileNum}.jpg";

          LiamDebugger.Message($"attempting to change file name from {fromFile} to {toFile}", 2);

          File.Move(fromFile, toFile);

          LiamDebugger.Message($"creating new data item with name: {newFileNum}", 2);
          DataModel newDataItem = new DataModel(newFileNum.ToString(), "unknown", "unkown", "unknown", "unkown", "unknown");
          MetaFileManager.Instance.DataList.Add(newDataItem);
          continue;
        }

        // Handle image that has valid name and does not exist in metadata
        string metadataName = Path.GetFileName(imName);
        metadataName = metadataName.Remove(metadataName.Length - 4);

        bool existsInMetaData = MetaFileManager.Instance.DataList.Any(d => d.FileName == metadataName);
        LiamDebugger.Message($"{imName} exists in metadata = {existsInMetaData}", 2);
        if (regexIsJpg.IsMatch(imName) && regexIsValidFileName.IsMatch(imName) && !existsInMetaData)
        {
          LiamDebugger.Message($"imName is jpg, does not exist in metadata and IS valid filename {imName}, adding to database and renaming file", 2);

          string filename = Path.GetFileName(imName);

          LiamDebugger.Message($"creating new data item with name: {metadataName}", 2);
          DataModel newDataItem = new DataModel(metadataName, "unknown", "unkown", "unknown", "unkown", "unknown");
          MetaFileManager.Instance.DataList.Add(newDataItem);
          continue;
        }

        LiamDebugger.Message($"imName already exists. Iterating to next loop", 2);

      }
      MetaFileManager.Instance.LoadJsMetaFile();
      MetaFileManager.Instance.SaveJsMetaFile();
    }

    static public void RemoveUnusedMetaFileData()
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      List<DataModel> dataToRemove = new List<DataModel>();
      foreach (DataModel data in MetaFileManager.Instance.DataList)
      {
        string name = data.FileName;
        string checkExists = $"{MetaFileManager.Instance.RootFolder}{name}.jpg";
        LiamDebugger.Message($"checking if {name} is in folder", 2);

        if(!File.Exists(checkExists))
        {
          LiamDebugger.Message($"{name} exists in meta but not in folder", 2);
          dataToRemove.Add(data);
        }

        MetaFileManager.Instance.DataList = MetaFileManager.Instance.DataList.Except(dataToRemove).ToList();
        MetaFileManager.Instance.SaveJsMetaFile();
      }
    }

    static public void ConvertImagesToJpg()
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
    }

    static private int ItterateToNonExistingFile(int fileNum)
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);


      while (true)
      {
        string path = $"{MetaFileManager.Instance.RootFolder}{fileNum}.jpg";
        if (File.Exists(path))
        {
          fileNum++;
          continue;
        }
        break; // does not exist, use this number
      }
      return fileNum;
    }
    static private string ItterateToNonExistingData(int dataNum)
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      while (true)
      {
        string path = $"{MetaFileManager.Instance.RootFolder}{dataNum}.jpg";
        if (MetaFileManager.Instance.DataList.Any(d => d.FileName == dataNum.ToString()))
        {
          dataNum++;
          continue;
        }
        break; // does not exist, use this number
      }
      return dataNum.ToString();
    }
    static private int ItterateToNonExistingItem()
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      int num = 1;


      while (true)
      {
        string path = $"{MetaFileManager.Instance.RootFolder}{num}.jpg";
        bool fileExists = File.Exists(path);
        bool dataExists = MetaFileManager.Instance.DataList.Any(d => d.FileName == num.ToString());

        if (!fileExists && !dataExists)
        {
          break;
        }
        num++;
      }
      return num;
    }

    static public bool IsValidFileName(string fileName)
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      Regex regexIsValidFileName = new Regex(@"(.*\/)\d\..*"); // check if it matches the 1.jpg format
      return regexIsValidFileName.IsMatch(fileName);      
    }

    static public bool IsJpgFileName(string fileName)
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      Regex regexIsJpg = new Regex(@".*.jpg"); // check if is jpg
      return regexIsJpg.IsMatch(fileName);
    }

  }
}
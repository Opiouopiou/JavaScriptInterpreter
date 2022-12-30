using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace JavaScriptInterpreter
{
  static public class Tools
  {
    static public void AddExcludedImagesInFolderToMetaFile()
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      string[] tempFilesInFolder = Directory.GetFiles(MetaFileManager.Instance.FolderPath, "*.jpg");

      Regex regexIsValidFileName = new Regex(@"(.*\/)\d\..*"); // check if it matches the 1.jpg format
      Regex regexIsJpg = new Regex(@".*.jpg"); // check if is jpg
      MetaFileManager.Instance.LoadJsMetaFile();
      ImageGridManager.Instance.gridFromNumCells = null;
      foreach (string imName in tempFilesInFolder)
      {
        //Check if imName exists in metaData
        string fileName = Path.GetFileName(imName);
        fileName = Path.GetFileNameWithoutExtension(fileName);

        bool existsInMetaData;

        if (MetaFileManager.Instance.DataList == null)
        {
          existsInMetaData = false;
        }
        else
        {
          existsInMetaData = MetaFileManager.Instance.DataList.Any(d => d.FileName == fileName);
        }

        // Handle image that has invalid name and does not exist in metadata
        if (regexIsJpg.IsMatch(imName) && !existsInMetaData)   // !regexIsValidFileName.IsMatch(imName)) 
        {
          LiamDebugger.Message($"imName is jpg and IS NOT valid filename {imName}, adding to database and renaming file", 2);
          int newFileNum = ItterateToNonExistingItem();

          string fromFile = $"{imName}";
          string toFile = $"{MetaFileManager.Instance.FolderPath}{newFileNum}.jpg";

          LiamDebugger.Message($"attempting to change file name from {fromFile} \n to {toFile}", 2);

          File.Move(fromFile, toFile);

          LiamDebugger.Message($"creating new data item with name: {newFileNum}", 2);
          DataModel newDataItem = new DataModel(newFileNum.ToString(), "unknown", "unkown", "unknown", "unknown", "unknown");
          MetaFileManager.Instance.DataList.Add(newDataItem);
          continue;
        }

        // Handle image that has valid name and does not exist in metadata
        LiamDebugger.Message($"{imName} exists in metadata = {existsInMetaData}", 3);
        if (regexIsJpg.IsMatch(imName) && regexIsValidFileName.IsMatch(imName) && !existsInMetaData)
        {
          LiamDebugger.Message($"imName is jpg, does not exist in metadata and IS valid filename {imName}, adding to database and renaming file", 2);

          string filename = Path.GetFileName(imName);

          LiamDebugger.Message($"creating new data item with name: {fileName}", 2);
          DataModel newDataItem = new DataModel(fileName, "unknown", "unkown", "unknown", "unkown", "unknown");
          MetaFileManager.Instance.DataList.Add(newDataItem);
          continue;
        }

        LiamDebugger.Message($"imName already exists. Iterating to next loop", 3);
      }
    }

    static public void RemoveUnusedMetaFileData()
    {
      LiamDebugger.Message(System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      List<DataModel> dataToRemove = new List<DataModel>();

      if (MetaFileManager.Instance.DataList == null)
      {
        return;
      }

      foreach (DataModel data in MetaFileManager.Instance.DataList)
      {
        string name = data.FileName;
        string checkExists = $"{MetaFileManager.Instance.FolderPath}{name}.jpg";
        LiamDebugger.Message($"checking if {name} is in folder", 3);

        if (!File.Exists(checkExists))
        {
          LiamDebugger.Message($"{name} exists in meta but not in folder", 2);
          dataToRemove.Add(data);
        }

        MetaFileManager.Instance.DataList = MetaFileManager.Instance.DataList.Except(dataToRemove).ToList();
        if (MetaFileManager.Instance.DataList == null)
        {
          MetaFileManager.Instance.DataList = new List<DataModel>();
        }
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
        string path = $"{MetaFileManager.Instance.FolderPath}{fileNum}.jpg";
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
        string path = $"{MetaFileManager.Instance.FolderPath}{dataNum}.jpg";
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

      if (MetaFileManager.Instance.DataList == null)
      {
        return 1;
      }

      while (true)
      {
        string path = $"{MetaFileManager.Instance.FolderPath}{num}.jpg";
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

    static public void ConvertAllImageFilesInFolderToJpg(string folderPath)
    {
      string[] files = Directory.GetFiles(folderPath);



      foreach (string file in files)
      {
        LiamDebugger.Message(file, 2);

        string ext = Path.GetExtension(file).ToLower();

        LiamDebugger.Message(ext, 2);


        if (ext == ".png" || ext == ".webp" || ext == ".JPG" || ext == ".jpeg")
        {
          LiamDebugger.Message("passed webp or png", 2);
          using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(file))
          {
            LiamDebugger.Message($"saving in to {Path.GetDirectoryName(file)}\\{Path.GetFileNameWithoutExtension(file)}.jpg", 2);

            image.SaveAsJpeg($"{Path.GetDirectoryName(file)}\\{Path.GetFileNameWithoutExtension(file)}.jpg");
            File.Delete(file);
            image.Dispose();
          }
        }
      }

    }
  }
}

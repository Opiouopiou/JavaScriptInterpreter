using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptInterpreter
{
  public class MetaFileManager
  {
    string _rootFolder = "K:/Torrent downloads/_temp/gams/2021/FortOfChains/dist/imagepacks/default/gender_female/subrace_demon";
    List<DataModel> _dataList;
    string metaFileName = "/imagemeta.js";

    string beginMeta = "";
    string stringOfConcern = "";
    string endMeta = "";

    string RootFolder { get => _rootFolder; set { _rootFolder = value; } }
    List<DataModel> DataList { get => _dataList; set { _dataList = value; } }



    public List<DataModel> LoadJsMetaFile(string rootFolder)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      ClearData();
      List<DataModel> DataList = new List<DataModel>();

      RootFolder = rootFolder;
      string[] ImageMetaLines = File.ReadAllLines(_rootFolder + metaFileName);

      int startOfArray = 0;
      while (!ImageMetaLines[startOfArray].Contains("UNITIMAGE_CREDITS"))
      {
        startOfArray++;

        if (startOfArray > ImageMetaLines.Length)
        {
          return null;
        }
        continue;
      }
      startOfArray++;

      for (int i = startOfArray; i < ImageMetaLines.Length; i++)
      {

        if(i + 5 >= ImageMetaLines.Length)
        {
          LiamDebugger.Message($"Exiting Data load loop", 2);

          break;
        }

        LiamDebugger.Message($" ---------- Adding new Datamodel item ---------", 2);


        string dataName     = "";
        string dataTitle    = "";
        string dataArtist   = "";
        string dataURL      = "";
        string dataLicense  = "";
        string dataExtra    = "";

        

        if(ImageMetaLines[i].Contains("{"))
        {
          dataName = ImageMetaLines[i].Trim();

          dataName = dataName.Remove(dataName.Length - 3 ); // trim ": {" from string

          LiamDebugger.Message($"name: {dataName}", 2);

        }
        if (ImageMetaLines[i+1].Contains("title"))
        {
          int from = ImageMetaLines[i + 1].IndexOf(" \"") + " \"".Length;
          int to = ImageMetaLines[i + 1].LastIndexOf("\",");
         
          dataTitle = ImageMetaLines[i + 1].Substring(from, to - from);

          LiamDebugger.Message($"title: {dataTitle}", 2);
        }
        if (ImageMetaLines[i + 2].Contains("artist"))
        {
          int from = ImageMetaLines[i + 2].IndexOf(" \"") + " \"".Length;
          int to = ImageMetaLines[i + 2].LastIndexOf("\",");

          dataArtist = ImageMetaLines[i + 2].Substring(from, to - from);

          LiamDebugger.Message($"artist: {dataArtist}", 2);
        }
        if (ImageMetaLines[i + 3].Contains("url"))
        {
          int from = ImageMetaLines[i + 3].IndexOf(" \"") + " \"".Length;
          int to = ImageMetaLines[i + 3].LastIndexOf("\",");

          dataURL = ImageMetaLines[i + 3].Substring(from, to - from);

          LiamDebugger.Message($"url: {dataURL}", 2);
        }
        if (ImageMetaLines[i + 4].Contains("license"))
        {
          int from = ImageMetaLines[i + 4].IndexOf(" \"") + " \"".Length;
          int to = ImageMetaLines[i + 4].LastIndexOf("\",");

          dataLicense = ImageMetaLines[i + 4].Substring(from, to - from);

          LiamDebugger.Message($"license: {dataLicense}", 2);
        }
        if (!ImageMetaLines[i + 5].Contains("extra"))
        {
          DataModel dataModel = new DataModel(dataName, dataTitle, dataArtist, dataURL, dataLicense);
          DataList.Add(dataModel);
          i = i + 5;
          continue;
        }
        if (ImageMetaLines[i + 5].Contains("extra"))
        {
          int from = ImageMetaLines[i + 5].IndexOf(" \"") + " \"".Length;
          int to = ImageMetaLines[i + 5].LastIndexOf("\",");

          dataArtist = ImageMetaLines[i + 5].Substring(from, to - from);

          LiamDebugger.Message($"extra: {dataExtra}", 2);

          DataModel dataModelExtra = new DataModel(dataName, dataTitle, dataArtist, dataURL, dataLicense, dataExtra);
          DataList.Add(dataModelExtra);
          i = i + 6;
        }

      }



      return DataList;
    }

    void SaveJsMetaFile(List<DataModel> dataToSave)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

    }

    void ClearData()
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      RootFolder = "";
      beginMeta = "";
      stringOfConcern = "";
      endMeta = "";
    }

  }
}

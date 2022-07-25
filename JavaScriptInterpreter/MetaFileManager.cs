using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace JavaScriptInterpreter
{
  public class MetaFileManager
  {
    string _rootFolder = "K:/Torrent downloads/_temp/gams/Own Stuff/JavaScriptInterpreter/testImageFolder/";
    int _lastClickedButton = 1;
    List<DataModel> _dataList = new List<DataModel>();
    string metaFileName = "\\imagemeta.js";
    MainWindow Form = Application.Current.Windows[0] as MainWindow;

    public Dictionary<int, DataModel> ImageButtonMapping = new Dictionary<int, DataModel>();

    string beginMeta = "";
    StringBuilder stringOfConcern = new StringBuilder();
    string endMeta = "  }\n \n}());\n";
    string _displayImageSource = "";
    public string FolderPath { get => _rootFolder; set { _rootFolder = value; } }
    public List<DataModel> DataList { get { if (_dataList == null) _dataList = new List<DataModel>(); return _dataList; } set { _dataList = value; } }
    public string DisplayImageSource { get => _displayImageSource; set { _displayImageSource = value; } }
    private static Lazy<MetaFileManager> lazy = new Lazy<MetaFileManager>(() => new MetaFileManager());
    public static MetaFileManager Instance { get { return lazy.Value; } }
    private MetaFileManager() { }

    public int LastClickedButton { get => _lastClickedButton; set { _lastClickedButton = value; } }

    public DataModel GetDataModelFromNum(int dataNum)
    {
      return DataList.ElementAt(dataNum);
    }

    public List<DataModel> LoadJsMetaFile()
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      ClearData();
      List<DataModel> DataList = new List<DataModel>();

      if (_rootFolder + metaFileName == null)
      {
        LiamDebugger.Message("! ------ ERROR: no meta file exists ------ !", 1);
        return null;
      }

      string[] ImageMetaLines = File.ReadAllLines(_rootFolder + metaFileName);

      int startOfArray = 0;
      while (!ImageMetaLines[startOfArray].Contains("UNITIMAGE_CREDITS"))
      {
        startOfArray++;
        if (startOfArray > ImageMetaLines.Length)
        {
          LiamDebugger.Message("! ------ ERROR: meta file does not contain \"UNITIMAGE_CREDITS\" ------ !", 2);
          return null;
        }
        continue;
      }
      startOfArray++;

      for (int i = 0; i < startOfArray; i++)
      {
        beginMeta = $"{beginMeta}{ImageMetaLines[i]} \n";
      }


      // ------------- setup begin meta complete ---------------- //

      while (!ImageMetaLines[startOfArray].Contains("{"))
      {
        startOfArray++;
        if (startOfArray > ImageMetaLines.Length - 1)
        {
          LiamDebugger.Message("! ------ ERROR: Could not find start of data 1 ------ !", 2);
          return null;
        }
      }


      for (int i = startOfArray; i < ImageMetaLines.Length; i++)
      {
        if (i + 5 >= ImageMetaLines.Length)
        {
          LiamDebugger.Message($"Exiting Data load loop", 2);
          break;
        }

        // find next start of data
        while (!ImageMetaLines[i].Contains("{"))
        {
          if (i > ImageMetaLines.Length)
          {
            LiamDebugger.Message("! ------ ERROR: Could not find start of data 2 ------ !", 2);
            break;
          }
          i++;
        }


        LiamDebugger.Message($" --------- Adding new Datamodel item to list ---------", 4);

        string dataName = "";
        string dataTitle = "";
        string dataArtist = "";
        string dataURL = "";
        string dataLicense = "";
        string dataExtra = "";

        if (ImageMetaLines[i].Contains("{"))
        {
          dataName = ImageMetaLines[i].Trim();
          dataName = dataName.Remove(dataName.Length - 3); // trim ": {" from end of string

          LiamDebugger.Message($"name: {dataName}", 2);
        }
        if (ImageMetaLines[i + 1].Contains("title"))
        {
          int from = ImageMetaLines[i + 1].IndexOf(" \"") + " \"".Length;
          int to = ImageMetaLines[i + 1].LastIndexOf("\",");

          dataTitle = ImageMetaLines[i + 1].Substring(from, to - from);

          LiamDebugger.Message($"title: {dataTitle}", 5);
        }
        if (ImageMetaLines[i + 2].Contains("artist"))
        {
          int from = ImageMetaLines[i + 2].IndexOf(" \"") + " \"".Length;
          int to = ImageMetaLines[i + 2].LastIndexOf("\",");

          dataArtist = ImageMetaLines[i + 2].Substring(from, to - from);

          LiamDebugger.Message($"artist: {dataArtist}", 5);
        }
        if (ImageMetaLines[i + 3].Contains("url"))
        {
          int from = ImageMetaLines[i + 3].IndexOf(" \"") + " \"".Length;
          int to = ImageMetaLines[i + 3].LastIndexOf("\",");

          dataURL = ImageMetaLines[i + 3].Substring(from, to - from);

          LiamDebugger.Message($"url: {dataURL}", 4);
        }
        if (ImageMetaLines[i + 4].Contains("license"))
        {
          int from = ImageMetaLines[i + 4].IndexOf(" \"") + " \"".Length;
          int to = ImageMetaLines[i + 4].LastIndexOf("\",");

          dataLicense = ImageMetaLines[i + 4].Substring(from, to - from);

          LiamDebugger.Message($"license: {dataLicense}", 5);
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

          LiamDebugger.Message($"extra: {dataExtra}", 5);

          DataModel dataModelExtra = new DataModel(dataName, dataTitle, dataArtist, dataURL, dataLicense, dataExtra);
          DataList.Add(dataModelExtra);
          i = i + 6;
        }
      }
      if (DataList.Count > 0) // This will execute if there were no items
      {
        LiamDebugger.Message($"end of metafile: {endMeta}", 2);

      }
      //DataList.Sort();

      return DataList;
    }

    public void SaveJsMetaFile()
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      // -- update meta file -- //
      stringOfConcern.Clear();
      DataModel d;
      ImageButtonMapping.TryGetValue(LastClickedButton, out d);
      if (d != null)
      {
        LiamDebugger.Message($" --------- Updating Datamodel item ---------", 4);

        d.Title = Form.Ttitle.Text;
        d.Artist = Form.Tartist.Text;
        d.Url = Form.Turl.Text;
        d.License = Form.Tlicense.Text;
      }

      LiamDebugger.Message($" --------- Writing datamodels in string ---------", 2);
      string midMeta = "";
      if (DataList == null)
      {
        // do nothing
      }
      else
      {
        for (int i = 0; i < DataList.Count; i++)
        {
          stringOfConcern.Append($"\n    {DataList[i].FileName}: {{\n");
          stringOfConcern.Append($"      title: \"{DataList[i].Title}\",\n");
          stringOfConcern.Append($"      artist: \"{DataList[i].Artist}\",\n");
          stringOfConcern.Append($"      url: \"{DataList[i].Url}\",\n");
          stringOfConcern.Append($"      license: \"{DataList[i].License}\",\n");
          stringOfConcern.Append($"    }},");
        }
        if (stringOfConcern.Length > 0)
        {
          midMeta = stringOfConcern.ToString().Remove(stringOfConcern.Length - 1);
        }
      }
        string fileText = "";
        if (DataList != null && DataList.Count > 0)
        {
          fileText = beginMeta + midMeta + "\n" + endMeta;
        }
        else
        {
          fileText = beginMeta + "\n" + endMeta;
        }

      LiamDebugger.Message($" --------- about to delete: {FolderPath}{metaFileName} ---------", 2);
      File.Delete($"{FolderPath}{metaFileName}");
      LiamDebugger.Message($"deleted: {FolderPath}{metaFileName}", 2);
      File.WriteAllText($"{FolderPath}{metaFileName}", fileText);
      LiamDebugger.Message($"saved:   {FolderPath}{metaFileName}", 2);

    }
    void ClearData()
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      if (_dataList != null)
      {
        _dataList.Clear();
      }
      ImageButtonMapping.Clear();
      beginMeta = "";
      stringOfConcern.Clear();
    }
    public void LoadDataFromButtonNum(int buttonNum, Button butt)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      LiamDebugger.Message($"datalist: {DataList}", 2);

      LastClickedButton = buttonNum;

      DataModel d;
      ImageButtonMapping.TryGetValue(buttonNum, out d);
      Image im = new Image();

      string[] uri = Directory.GetFiles(_rootFolder, $"{d.FileName}.jpg");
      LiamDebugger.Message($"dataNum: {d.FileName}, searching for {_rootFolder}{d.FileName}.jpg", 2);
      //LiamDebugger.Message($"num files gotten: {uri.Length}", 2);

      if (uri.Length == 1)
      {
        BitmapImage bi = new BitmapImage();
        bi.BeginInit();
        bi.CacheOption = BitmapCacheOption.OnLoad;
        bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
        bi.UriSource = new Uri(uri[0], UriKind.Relative);
        bi.EndInit();
        im.Source = bi;

        Form.Idisplay.Source = bi;
        Form.Tartist.Text = d.Artist;
        Form.Textra.Text = d.Extra;
        Form.Tlicense.Text = d.License;
        Form.Ttitle.Text = d.Title;
        Form.Turl.Text = d.Url;
      }
      LiamDebugger.Message($"image source = {Form.Idisplay.Source}", 2);
      ImageGridManager.Instance.LoadFolderIntoGrid();
    }
  }
}

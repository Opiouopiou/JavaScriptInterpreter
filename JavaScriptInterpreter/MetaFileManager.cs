using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace JavaScriptInterpreter
{
  public class MetaFileManager
  {
    string _rootFolder = "K:/Torrent downloads/_temp/gams/2021/FortOfChains/dist/imagepacks/default/gender_female/subrace_demon/";
    List<DataModel> _dataList;
    string metaFileName = "imagemeta.js";
    MainWindow Form = Application.Current.Windows[0] as MainWindow;

    public Dictionary<int, DataModel> ImageButtonMapping = new Dictionary<int, DataModel>();

    string beginMeta = "";
    StringBuilder stringOfConcern = new StringBuilder();
    string endMeta = "  }\n \n}())\n";
    string _displayImageSource = "";
    public string RootFolder { get => _rootFolder; set { _rootFolder = value; } }
    public List<DataModel> DataList { get => _dataList; set { _dataList = value; } }
    public string DisplayImageSource { get => _displayImageSource; set { _displayImageSource = value; } }
    private static Lazy<MetaFileManager> lazy = new Lazy<MetaFileManager>(() => new MetaFileManager());
    public static MetaFileManager Instance { get { return lazy.Value; } }
    private MetaFileManager() { }

    public DataModel GetDataModelFromNum(int dataNum)
    {
      return DataList.ElementAt(dataNum);
    }

    public List<DataModel> LoadJsMetaFile()
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      ClearData();
      List<DataModel> DataList = new List<DataModel>();

      if(_rootFolder + metaFileName == null)
      {

      }

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

      foreach (string line in ImageMetaLines)
      {
        beginMeta = $"{beginMeta}\n{line}";
      }
      LiamDebugger.Message($"start of metafile: {beginMeta}", 2);


      for (int i = startOfArray; i < ImageMetaLines.Length; i++)
      {
        if (i + 5 >= ImageMetaLines.Length)
        {
          LiamDebugger.Message($"Exiting Data load loop", 2);
          break;
        }

        LiamDebugger.Message($" --------- Adding new Datamodel item ---------", 4);

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

          LiamDebugger.Message($"name: {dataName}", 5);
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
      if (DataList.Count > 0)
      {
        LiamDebugger.Message($"end of metafile: {endMeta}", 2);

      }
      //DataList.Sort();
      return DataList;
    }

    public void SaveJsMetaFile()
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      for (int i = 0; i < DataList.Count; i++)
      {
        stringOfConcern.Append($"    {i}: {{\n");
        stringOfConcern.Append($"    title: \"{DataList[i].Title}\"\n");
        stringOfConcern.Append($"    artist: \"{DataList[i].Artist}\"\n");
        stringOfConcern.Append($"    url: \"{DataList[i].Url}\"\n");
        stringOfConcern.Append($"    license: \"{DataList[i].License}\"\n");
        stringOfConcern.Append($"  }},");
      }

      string fileText = "";
      if (DataList.Count > 0)
      {
        fileText = beginMeta + stringOfConcern.ToString() + endMeta;
      }
      else
      {
        fileText = beginMeta;
      }
      LiamDebugger.Message($"about to delete: {RootFolder}imagemeta.js",2); 
      File.Delete($"{RootFolder}imagemeta.js");
      LiamDebugger.Message($"deleted: {RootFolder}imagemeta.js",2); 
      File.WriteAllText($"{RootFolder}imagemeta.js", fileText);
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
      //endMeta = "";
    }
    public void LoadDataFromButtonNum(int buttonNum, Button butt)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      LiamDebugger.Message($"datalist: {DataList}", 2);
      //DataModel d = DataList[dataNum];

      DataModel d;
      ImageButtonMapping.TryGetValue(buttonNum, out d);
      Image im = new Image();

      string[] uri = Directory.GetFiles(_rootFolder, $"{d.FileName}.jpg");
      LiamDebugger.Message($"dataNum: {d.FileName}, searching for {_rootFolder}{d.FileName}.jpg", 2);
      LiamDebugger.Message($"num files gotten: {uri.Length}", 2);

      if (uri.Length == 1)
      {
        im.Source = new BitmapImage(new Uri(uri[0]));
        Form.Idisplay.Source = new BitmapImage(new Uri(uri[0]));
        Form.Tartist.Text = d.Artist;
        Form.Textra.Text = d.Extra;
        Form.Tlicense.Text = d.License;
        Form.Ttitle.Text = d.Title;
        Form.Turl.Text = d.Url;
      }
      LiamDebugger.Message($"image source = {Form.Idisplay.Source}", 2);
    }
  }
}

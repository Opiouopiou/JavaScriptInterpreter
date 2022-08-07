using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptInterpreter
{
  public class DataModel :IComparable
  {
    //int _num;
    string _fileName;
    string _title;
    string _artist;
    string _url;
    string _license;
    string _extra;

    //public int Num { get => _num; set { _num = value; } }
    public string FileName { get => _fileName; set { _fileName = value; } }
    public string Title { get=>_title; set { _title = value; } }
    public string Artist { get=>_artist; set { _artist = value; } }
    public string Url { get=>_url; set { _url = value; } }
    public string License { get=>_license; set { _license = value; } }
    public string Extra { get=>_extra; set { _extra = value; } }

    public DataModel(string fileName, string title, string artist, string url, string license)
    {
      //LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      _fileName = fileName;
      _title = title;
      _artist = artist;
      _url = url;
      _license = license;
    }

    public DataModel(string fileName, string title, string artist, string url, string license, string extra)
    {
      //LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      _fileName = fileName;
      _title = title;
      _artist = artist;
      _url = url;
      _license = license;
      _extra = extra;
    }

    //make sortable
    public int CompareTo(object obj)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      if (obj == null) return 1;
      DataModel otherDateModel = obj as DataModel;
      if (otherDateModel != null)
      {
        LiamDebugger.Message($"fileName: {this.FileName}",2);

        //int num = Int32.Parse(Path.GetFileName(this.FileName).Remove(this.FileName.Length -3,3));
        int num = Int32.Parse(this.FileName);
        return num;
      }
      else
      {
        throw new ArgumentException("object is not a DataModel");
      }
    }

  }
}

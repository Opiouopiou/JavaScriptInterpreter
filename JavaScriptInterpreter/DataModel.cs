using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptInterpreter
{
  public class DataModel
  {
    string _fileName;
    string _title;
    string _artist;
    string _url;
    string _license;
    string _extra;

    public string Num { get => _fileName; set { _fileName = value; } }
    public string Title { get=>_title; set { _title = value; } }
    public string Artist { get=>_artist; set { _artist = value; } }
    public string Url { get=>_url; set { _url = value; } }
    public string License { get=>_license; set { _license = value; } }
    public string Extra { get=>_extra; set { Extra = value; } }

    public DataModel(string num, string title, string artist, string url, string license)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      _fileName = num;
      _title = title;
      _artist = artist;
      _url = url;
      _license = license;
    }

    public DataModel(string num, string title, string artist, string url, string license, string extra)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      _fileName = num;
      _title = title;
      _artist = artist;
      _url = url;
      _license = license;
      _extra = extra;
    }

  }
}

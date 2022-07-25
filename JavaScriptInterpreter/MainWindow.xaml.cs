using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;

namespace JavaScriptInterpreter
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    ImageGridManager ImGridManager;
    MetaFileManager metaFileManager;

    string chosenFolder;

    public MainWindow()
    {
      InitializeComponent();
      Initialize();
    }

    private void Initialize()
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      LiamDebugger.Message(" -------- init ---------- ", 2);


      metaFileManager = MetaFileManager.Instance;
      ImGridManager = ImageGridManager.Instance;

      chosenFolder = Directory.GetCurrentDirectory();
      chosenFolder = System.IO.Path.GetFullPath(System.IO.Path.Combine(chosenFolder, @"..\..\..\"));

      metaFileManager.FolderPath = $"{chosenFolder}\\testImageFolder/";

      metaFileManager.DataList = metaFileManager.LoadJsMetaFile();


      ImGridManager.LoadFolderIntoGrid();
    }

    private void SaveMetaJS(object sender, RoutedEventArgs e)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      metaFileManager.SaveJsMetaFile();
      //metaFileManager.LoadJsMetaFile();
    }

    private void ChangeFolder(object sender, RoutedEventArgs e)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      LiamDebugger.Message(" --------- change folder ---------- ", 2);
      Microsoft.Win32.OpenFileDialog fileBrowserDialog = new Microsoft.Win32.OpenFileDialog();

      if (fileBrowserDialog.ShowDialog() == true)
      {
        string path = $"{fileBrowserDialog.FileName}";
        string[] pathSplit = path.Split('\\');
        int len = pathSplit.Length - 1;
        string pathNew = path.Remove(path.Length - pathSplit[len].Length);
        metaFileManager.FolderPath = pathNew;
        chosenFolder = pathNew;
        metaFileManager.DataList = metaFileManager.LoadJsMetaFile();
        ImGridManager.LoadFolderIntoGrid();
      }
    }


    void LoadFirstImageInFolder()
    {
      if (metaFileManager.DataList.Count > 0)
      {
        Ttitle.Text = metaFileManager.DataList[0].Title;
        Tartist.Text = metaFileManager.DataList[0].Artist;
        Turl.Text = metaFileManager.DataList[0].Url;
        Tlicense.Text = metaFileManager.DataList[0].License;
        Textra.Text = metaFileManager.DataList[0].Extra;

      }
    }

    private void UpdateFolder(object sender, RoutedEventArgs e)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      Tools.RemoveUnusedMetaFileData();
      Tools.AddExcludedImagesInFolderToMetaFile();
      ImageGridManager.Instance.LoadFolderIntoGrid();

      LiamDebugger.Message("completed updating folder", 2);
    }

    private void UpdateFolderAndChildFolders(object sender, RoutedEventArgs e)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      UpdateAllChildFolders(metaFileManager.FolderPath);

      metaFileManager.FolderPath = chosenFolder;
      metaFileManager.LoadJsMetaFile();
      Tools.RemoveUnusedMetaFileData();
      Tools.AddExcludedImagesInFolderToMetaFile();
      ImageGridManager.Instance.LoadFolderIntoGrid();

      LiamDebugger.Message("completed updating folder and subfolders", 2);
    }

    private void UpdateAllChildFolders(string sDir)
    {

      try
      {
        LiamDebugger.Message(sDir, 2);
        metaFileManager.FolderPath = sDir;
        metaFileManager.LoadJsMetaFile();
        Tools.RemoveUnusedMetaFileData();
        Tools.AddExcludedImagesInFolderToMetaFile();

        foreach (string d in Directory.GetDirectories(sDir))
        {
          UpdateAllChildFolders(d);
        }
      }
      catch (Exception excpt)
      {
        LiamDebugger.Message("ERROR - " + excpt.Message, 2);
      }

    }
  }
}

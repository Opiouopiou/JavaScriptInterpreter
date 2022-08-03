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

      metaFileManager.LoadJsMetaFile();


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
        metaFileManager.LoadJsMetaFile();
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
      MainWindow Form = System.Windows.Application.Current.Windows[0] as MainWindow;
      Form.Tinfo.Text = "Running code";
      Form.Idisplay.Source = null;

      Tools.ConvertAllImageFilesInFolderToJpg(metaFileManager.FolderPath);

      Tools.RemoveUnusedMetaFileData();
      metaFileManager.SaveJsMetaFile();

      Tools.AddExcludedImagesInFolderToMetaFile();
      metaFileManager.SaveJsMetaFile();
      ImageGridManager.Instance.LoadFolderIntoGrid();
      Form.Tinfo.Text = metaFileManager.FolderPath;
      LiamDebugger.Message("completed updating folder", 2);
    }

    private async void UpdateFolderAndChildFolders(object sender, RoutedEventArgs e)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      MainWindow Form = System.Windows.Application.Current.Windows[0] as MainWindow;

      Form.Tinfo.Dispatcher.Invoke(() => {Form.Tinfo.Text = "Running code"; });



      await UpdateAllChildFolders(metaFileManager.FolderPath + "\\");
      Form.Tinfo.Text = metaFileManager.FolderPath;

      LiamDebugger.Message("completed updating folder and subfolders", 2);

      //private async void executeParallelAsync_Click(object sender, RoutedEventArgs e)
      //{
      //  var watch = System.Diagnostics.Stopwatch.StartNew();

      //  await RunDownloadParallelASync();

      //  watch.Stop();
      //  var elapsedMs = watch.ElapsedMilliseconds;

      //  resultsWindow.Text += $"Total execution time: {elapsedMs}";
      ////}

    }

    private async Task UpdateAllChildFolders(string sDir)
    {

      try
      {

        metaFileManager.FolderPath = chosenFolder;
        metaFileManager.LoadJsMetaFile();

        Tools.RemoveUnusedMetaFileData();
        metaFileManager.SaveJsMetaFile();

        Tools.AddExcludedImagesInFolderToMetaFile();
        metaFileManager.SaveJsMetaFile();

        ImageGridManager.Instance.LoadFolderIntoGrid();

        MainWindow Form = System.Windows.Application.Current.Windows[0] as MainWindow;
        Form.Idisplay.Source = null;
        LiamDebugger.Message(sDir, 2);
        metaFileManager.FolderPath = sDir;

        Tools.ConvertAllImageFilesInFolderToJpg(metaFileManager.FolderPath);
        metaFileManager.LoadJsMetaFile();

        Tools.RemoveUnusedMetaFileData();
        metaFileManager.SaveJsMetaFile();

        Tools.AddExcludedImagesInFolderToMetaFile();
        metaFileManager.SaveJsMetaFile();


        foreach (string d in Directory.GetDirectories(sDir))
        {
          await UpdateAllChildFolders(d + "\\");
        }
      }
      catch (Exception excpt)
      {
        LiamDebugger.Message("ERROR - " + excpt.Message, 2);
      }

    }
  }
}

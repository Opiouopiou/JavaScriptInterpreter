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

      string workingPath = Directory.GetCurrentDirectory();
      workingPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(workingPath, @"..\..\..\"));

      metaFileManager.FolderPath = $"{workingPath}/testImageFolder/";

      metaFileManager.DataList = metaFileManager.LoadJsMetaFile();


      ImGridManager.LoadFolderIntoGrid();

      //LoadFirstImageInFolder();
    }

    //private void button1_Click(object sender, EventArgs e)
    //{
    //  int size = -1;
    //  OpenFileDialog openFileDialog1 = new OpenFileDialog();
    //  DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
    //  if (result == DialogResult.OK) // Test result.
    //  {
    //    string file = openFileDialog1.FileName;
    //    try
    //    {
    //      string text = File.ReadAllText(file);
    //      size = text.Length;
    //    }
    //    catch (IOException)
    //    {
    //    }
    //  }

    //}

    //todo
    // - browse for image and add it to data list
    // - remove image from meta file
    // - add all images in folder to meta file




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
        metaFileManager.DataList = metaFileManager.LoadJsMetaFile();
        ImGridManager.LoadFolderIntoGrid();
        //LoadFirstImageInFolder();

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
      GetAllChildFolders();
      Tools.RemoveUnusedMetaFileData();
      Tools.AddExcludedImagesInFolderToMetaFile();

    }

    private string[] GetAllChildFolders()
    {
      string[] paths = Directory.GetDirectories(metaFileManager.FolderPath);

      foreach(string path in paths)
      {
        LiamDebugger.Message($"{path}",2);
      }

      return null;
    }

  }
}

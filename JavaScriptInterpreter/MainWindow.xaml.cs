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
      metaFileManager.RootFolder = "K:/Torrent downloads/_temp/gams/2021/FortOfChains/dist/imagepacks/default/gender_female/subrace_demon/";
      metaFileManager.DataList = metaFileManager.LoadJsMetaFile();

      ImGridManager = ImageGridManager.Instance;




      ImGridManager.LoadFolderIntoGrid(scrollViewer);

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
    // - load folder with browse.
    // - load all images only with valid data model
    // - browse for image and add it to data list
    // - save to new meta file
    // - remove image from meta file
    // - class data in xaml
    // - hover and select in xaml





    private void SaveMetaJS(object sender, RoutedEventArgs e)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      LiamDebugger.Message(" -------- save to imagemets.js ---------- ", 2);

      //int sut = imagesInFolder.Length;


      metaFileManager.SaveJsMetaFile();

    }

    private void ChangeFolder(object sender, RoutedEventArgs e)
    {
      Microsoft.Win32.OpenFileDialog fileBrowserDialog = new Microsoft.Win32.OpenFileDialog();
      LiamDebugger.Message(" -------- change folder ---------- ", 2);

      if (fileBrowserDialog.ShowDialog() == true)
      {
        string path = $"{fileBrowserDialog.FileName}";
        string[] pathSplit = path.Split('\\');
        int len = pathSplit.Length - 1;
        string pathNew = path.Remove(path.Length - pathSplit[len].Length);
        metaFileManager.RootFolder = pathNew;
        metaFileManager.DataList = metaFileManager.LoadJsMetaFile();
        ImGridManager.LoadFolderIntoGrid(scrollViewer);
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
  }
}
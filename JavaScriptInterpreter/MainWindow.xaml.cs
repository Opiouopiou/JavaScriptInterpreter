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
      metaFileManager.DataList = metaFileManager.LoadJsMetaFile(metaFileManager.RootFolder);
      ImGridManager = ImageGridManager.Instance;
      
      string[] imagesInFolder = Directory.GetFiles(metaFileManager.RootFolder, "*.jpg");
      int numImagesInFolder = imagesInFolder.Length;


      //int sut = imagesInFolder.Length;

      ImGridManager.LoadImageIntoAllGridCells(metaFileManager.RootFolder, scrollViewer);

      if (metaFileManager.DataList.Count > 0)
      {
        Ttitle.Text = metaFileManager.DataList[1].Title;
        Tartist.Text = metaFileManager.DataList[1].Artist;
        Turl.Text = metaFileManager.DataList[1].Url;
        Tlicense.Text = metaFileManager.DataList[1].License;
        Textra.Text = metaFileManager.DataList[1].Extra;
      }
      //Idisplay.Source
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





    private void ClickTestAction(object sender, RoutedEventArgs e)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      LiamDebugger.Message(" -------- click ---------- ", 2);

      string[] imagesInFolder = Directory.GetFiles(metaFileManager.RootFolder, "*.jpg");
      int numImagesInFolder = imagesInFolder.Length;

      //int sut = imagesInFolder.Length;

      ImGridManager.LoadImageIntoAllGridCells(metaFileManager.RootFolder, scrollViewer);
      List<DataModel> dataList = metaFileManager.LoadJsMetaFile(metaFileManager.RootFolder);

    }

    //public void LoadNewDisplayImage(Image displayImage)
    //{
    //  LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

    //  Idisplay = displayImage;
    //}


  }
}
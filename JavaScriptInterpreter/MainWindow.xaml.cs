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
    string _rootFolder = "K:/Torrent downloads/_temp/gams/2021/FortOfChains/dist/imagepacks/default/gender_female/subrace_demon";

    //string _rootFolder = "K:/Torrent downloads/_temp/gams/2021/FortOfChains/dist/imagepacks/default/gender_female/subrace_humansea/bg_whore";

    string metaFile = "imagemeta.js";
    ImageGridManager ImGridManager;

    MetaFileManager metaFileManager;

    public MainWindow()
    {
      InitializeComponent();
      Initialize();
      Console.WriteLine("test writeline");
    }

    private void Initialize()
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      ImGridManager = new ImageGridManager();

      metaFileManager = new MetaFileManager();
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
    //  Console.WriteLine(size); // <-- Shows file size in debugging mode.
    //  Console.WriteLine(result); // <-- For debugging use.
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

      LiamDebugger.Message(" -------- click ---------- ",2);


      string[] imagesInFolder = Directory.GetFiles(_rootFolder, "*.jpg");
      int numImagesInFolder = imagesInFolder.Length;

      //int sut = imagesInFolder.Length;

      ImGridManager.LoadImageIntoAllGridCells(_rootFolder, scrollViewer);
      List<DataModel> dataList = metaFileManager.LoadJsMetaFile(_rootFolder);

      ContentTesterText.Text = dataList[1].Title;
    }
  }
}
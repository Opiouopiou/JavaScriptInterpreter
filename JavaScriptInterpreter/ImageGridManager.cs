using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Text.RegularExpressions;

namespace JavaScriptInterpreter
{
  public class ImageGridManager
  {
    int _gridCols = 4;
    MetaFileManager metaFileManager;
    List<string> imagesInFolder = new List<string>();
    public Grid gridFromNumCells;


    private static Lazy<ImageGridManager> lazy = new Lazy<ImageGridManager>(() => new ImageGridManager());

    public static ImageGridManager Instance { get { return lazy.Value; } }

    private ImageGridManager() { metaFileManager = MetaFileManager.Instance; }


    int GetGridNumber(int col, int row, Grid grid)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      int cols = grid.ColumnDefinitions.Count;
      int rows = grid.RowDefinitions.Count;

      int gridNumber = row * cols + col;

      return gridNumber;
    }

    private Grid CreateGridFromNumOfCells(int num)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      Grid gridFromNumCells = new Grid();
      for (int i = 0; i < _gridCols; i++)
      {
        gridFromNumCells.ColumnDefinitions.Add(new ColumnDefinition());
      }
      int rows = ((num - 1) / _gridCols) + 1;
      for (int i = 0; i < rows; i++)
      {
        gridFromNumCells.RowDefinitions.Add(new RowDefinition());
      }
      return gridFromNumCells;
    }
    int GetColNumFromGrid(int number, Grid grid)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 4);

      int cols = grid.ColumnDefinitions.Count;
      int rowNum = GetRowNumFromGrid(number, grid);
      int col = (number - rowNum * cols);
      return col - 1;
    }
    int GetRowNumFromGrid(int number, Grid grid)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 4);

      int cols = grid.ColumnDefinitions.Count;
      int row = ((number - 1) / cols);
      return row;
    }


    public void LoadFolderIntoGrid()
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      metaFileManager.ImageButtonMapping.Clear();

      if (metaFileManager.DataList == null)
      {
        return;
      }

      MainWindow Form = Application.Current.Windows[0] as MainWindow;
      ScrollViewer scrollViewer = Form.scrollViewer;

      // setup sorted list of images in root folder
      Regex regexWithFolder = new Regex(@"(.*\/)\d*.jpg");
      Regex regex = new Regex(@"\d*.jpg");

      string[] tempImagesInFolder = Directory.GetFiles(metaFileManager.FolderPath, "*.jpg");
      LiamDebugger.Message($"len tempimagesinfolder: {tempImagesInFolder.Length}", 2);
      imagesInFolder.Clear();

      foreach (string imgPathName in tempImagesInFolder)
      {
        string imgFileName = Path.GetFileName(imgPathName);
        LiamDebugger.Message($"img name: {imgFileName}", 3);
        if (regex.IsMatch(imgPathName))
        {
          LiamDebugger.Message($"regex: {regex} matches string {imgFileName}",3);
          imagesInFolder.Add(imgPathName);
          continue;
        }
        LiamDebugger.Message($"regex: {regex} did not match string {imgFileName}", 3);

      }
      imagesInFolder.Sort();
      LiamDebugger.Message($"count imagesInFolder: {imagesInFolder.Count}", 2);


      // create grid from number of images in folder given a column size
      int rows = (imagesInFolder.Count - 1) / _gridCols + 1;
      int cols = _gridCols;
      int numCells = rows * cols;
      gridFromNumCells = CreateGridFromNumOfCells(numCells);


      // fill each grid cell with buttons and images
      for (int i = 1; i < metaFileManager.DataList.Count + 1; i++)
      {
        // get row and column number from i
        int coli = GetColNumFromGrid(i, gridFromNumCells);
        int rowi = GetRowNumFromGrid(i, gridFromNumCells);

        // create button
        Button butt = new Button();
        butt.Name = $"B{i}";
        butt.Click += ImageButtonClick;


        // create image
        LiamDebugger.Message($"metafile: {metaFileManager.DataList[i - 1].FileName}, button: {i - 1}", 3);
        if (File.Exists($"{metaFileManager.FolderPath}{metaFileManager.DataList[i - 1].FileName}.jpg"))
        {
          LiamDebugger.Message($"Existing file to load into button: {metaFileManager.FolderPath}{metaFileManager.DataList[i - 1].FileName}.jpg", 3);

          Image im = new Image();

          string source = $@"{metaFileManager.FolderPath}{metaFileManager.DataList[i - 1].FileName}.jpg";
          im.Source = new BitmapImage(new Uri(source));

          butt.Content = im;
        }
        else
        {
          LiamDebugger.Message($"Missing file: {metaFileManager.DataList[i - 1].FileName}.jpg", 2);
          butt.Content = $"missing image file: \n {metaFileManager.DataList[i - 1].FileName}.jpg";
        }


        // mapping and image in button
        metaFileManager.ImageButtonMapping.Add(i, metaFileManager.DataList[i - 1]);


        // add to grid cell
        gridFromNumCells.Children.Add(butt);
        Grid.SetRow(butt, rowi);
        Grid.SetColumn(butt, coli);
      }



      scrollViewer.Content = gridFromNumCells;
    }



    public void ImageButtonClick(object sender, EventArgs e)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);
      LiamDebugger.Message(" -------- click image ---------- ", 2);


      Button B = sender as Button;
      string buttonNum = B.Name.Remove(0, 1); // remove letter B at front of name
      LiamDebugger.Message($"load number: {buttonNum}", 2);
      MetaFileManager.Instance.LoadDataFromButtonNum(Int32.Parse(buttonNum), B);
    }

  }
}

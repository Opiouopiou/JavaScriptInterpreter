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

namespace JavaScriptInterpreter
{
  public class ImageGridManager
  {
    int _gridCols = 4;


    int GetGridNumber(int col, int row, Grid grid)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      int cols = grid.ColumnDefinitions.Count;
      int rows = grid.RowDefinitions.Count;

      int gridNumber = row * cols + col;

      return gridNumber;
    }

    public Grid CreateGridFromNumOfCells(int num)
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


    public void LoadImageIntoAllGridCells(string rootDir, ScrollViewer scrollViewer)
    {
      LiamDebugger.Name(GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, 2);

      string[] imagesInFolder = Directory.GetFiles(rootDir, "*.jpg");
      int rows = ((imagesInFolder.Length - 1) / _gridCols + 1);
      int cols = _gridCols;
      int numCells = rows * cols;

      Grid generatedImageGrid = CreateGridFromNumOfCells(numCells);

      for (int i = 1; i < imagesInFolder.Length + 1; i++)
      {
        int coli = GetColNumFromGrid(i, generatedImageGrid);
        int rowi = GetRowNumFromGrid(i, generatedImageGrid);

        Button butt = new Button();

        //var bitmapFrame = BitmapFrame.Create(new Uri(imagesInFolder[i - 1]), BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
        //var width = bitmapFrame.PixelWidth;
        //var height = bitmapFrame.PixelHeight;


        Image im = new Image();
        im.Source = new BitmapImage(new Uri(imagesInFolder[i - 1]));

        ImageBrush imb = new ImageBrush();
        //imb.Stretch = Stretch.Uniform;
        //imb.ImageSource = im.Source;
        //butt.color = imb;
        butt.Content = im;

        generatedImageGrid.Children.Add(butt);
        Grid.SetRow(butt, rowi);
        Grid.SetColumn(butt, coli);
        //LiamDebugger.Message($"height: {height}",1);
        //LiamDebugger.Message($"width: {width}", 1);

        //butt.Height = 100;
        //butt.Width = width;

        //generatedImageGrid.VerticalAlignment = VerticalAlignment.Top;
      }

      //int g = ((4 - 1) / 3) + 1;
      //ContentTesterText.Text = ($"g = {g.ToString()}, Rows = {rows}, Cols = {cols}, number = {numCells}");

      scrollViewer.Content = generatedImageGrid;
    }

    public void CheckMouseOver(object sender, EventArgs e)
    {
      
    }

  }
}

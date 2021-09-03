using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    
    public partial class Form4 : Form
    {
        Size sz;

        public Form4(string filePath)
        {
           
            InitializeComponent();
           


            String start ="", end="";

            double duration=0, tX = 0, tY = 0, tZ = 0, tVS=0, maxX = 0, maxY = 0, maxZ = 0, ZC_X = 0, ZC_Y = 0, ZC_Z = 0, maxAx = 0, maxAy = 0, maxAz = 0, maxVS = 0, maxSound = 0;
            double x = 0, y = 0, z = 0;
            
            using (var fileRdr = new StreamReader(filePath))
            {
                var columns = fileRdr.ReadLine().Split(", ".ToArray(), StringSplitOptions.RemoveEmptyEntries);


                foreach (var col in columns)
                {
                    var dataColumn = new DataGridViewTextBoxColumn();

                }
                var lineData = fileRdr.ReadLine().Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                DateTime dt1 = Convert.ToDateTime(lineData[0]);
                chart1.Series[0].Points.AddXY(Convert.ToDouble(0), lineData[1]);
                chart2.Series[0].Points.AddXY(Convert.ToDouble(0), lineData[2]);
                chart3.Series[0].Points.AddXY(Convert.ToDouble(0), lineData[3]);
                chart4.Series[0].Points.AddXY(Convert.ToDouble(0), lineData[5]);

              
                start = dt1.ToString("yyyy/MM/dd hh:mm:ss");
                maxX = Double.Parse(lineData[1]);
                maxY = Double.Parse(lineData[2]);
                maxZ = Double.Parse(lineData[3]);

                maxVS = Double.Parse(lineData[4]);

                maxAx = Double.Parse(lineData[6]);
                maxAy = Double.Parse(lineData[7]);
                maxAz = Double.Parse(lineData[8]);
                
                maxSound = Double.Parse(lineData[5]);
                x = Double.Parse(lineData[1]);
                y = Double.Parse(lineData[2]);
                z = Double.Parse(lineData[3]);
                while (!fileRdr.EndOfStream)
                {
                    lineData = fileRdr.ReadLine().Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);

                    DateTime dt2 = Convert.ToDateTime(lineData[0]);
                    end = dt2.ToString("yyyy/MM/dd hh:mm:ss");
                    double t = (dt2 - dt1).TotalSeconds;

                    chart1.Series[0].Points.AddXY(t, lineData[1]);
                    chart2.Series[0].Points.AddXY(t, lineData[2]);
                    chart3.Series[0].Points.AddXY(t, lineData[3]);
                    chart4.Series[0].Points.AddXY(t, lineData[5]);


                    if (x * Double.Parse(lineData[1]) < 0)
                    {
                        ZC_X++;
                        x = Double.Parse(lineData[1]);
                    }
                    if (y * Double.Parse(lineData[2]) < 0)
                    {
                        ZC_Y++;
                        x = Double.Parse(lineData[2]);
                    }
                    if (z * Double.Parse(lineData[3]) < 0)
                    {
                        ZC_Z++;
                        x = Double.Parse(lineData[3]);
                    }
                    if (maxX < Double.Parse(lineData[1]))
                    {
                        maxX = Double.Parse(lineData[1]);
                        tX = t;
                    }
                    if (maxY < Double.Parse(lineData[2]))
                    {
                        maxY = Double.Parse(lineData[2]);
                        tY = t;
                    }
                    if (maxZ < Double.Parse(lineData[3]))
                    {
                        maxZ = Double.Parse(lineData[3]);
                        tZ = t;
                    }
                    if (maxVS < Double.Parse(lineData[4]))
                    {
                        maxVS = Double.Parse(lineData[4]);
                        tVS = t;
                    }

                    if (maxSound < Double.Parse(lineData[5])) maxSound = Double.Parse(lineData[5]);

                    if (maxAx < Double.Parse(lineData[6]))  maxAx = Double.Parse(lineData[6]);
                      
                    if (maxAy < Double.Parse(lineData[7]))  maxAy = Double.Parse(lineData[7]);
                   
                    if (maxAz < Double.Parse(lineData[8]))  maxAz = Double.Parse(lineData[8]);



                    duration = t;
                }

                fileRdr.Close();
                fileRdr.Dispose();

                
            }

            string[] str = filePath.Split('\\');
            string filename = str[str.Length - 1];
            string fileheader = filename.Substring(0, 3);

            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;



            if (fileheader.Equals("BLS"))
            {
                chart1.ChartAreas[0].AxisY.Title = "X(mm/s)";
                chart2.ChartAreas[0].AxisY.Title = "Y(mm/s)";
                chart3.ChartAreas[0].AxisY.Title = "Z(mm/s)";
            }
            else if (fileheader.Equals("EVS"))
            {
                chart1.ChartAreas[0].AxisY.Title = "X(dB)";
                chart2.ChartAreas[0].AxisY.Title = "Y(dB)";
                chart3.ChartAreas[0].AxisY.Title = "Z(dB)";
            }

            chart1.ChartAreas[0].AxisX.Title = "Time(sec)";

            chart2.ChartAreas[0].AxisX.Title = "Time(sec)";

            chart3.ChartAreas[0].AxisX.Title = "Time(sec)";

            chart4.ChartAreas[0].AxisX.Title = "Time(sec)";
            chart4.ChartAreas[0].AxisY.Title = "Sound(db(A))";

            chart1.Legends[0].Enabled = false;
            chart2.Legends[0].Enabled = false;
            chart3.Legends[0].Enabled = false;
            chart4.Legends[0].Enabled = false;

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            //chart1.ChartAreas[0].AxisX.Interval = 0.25;

            chart2.ChartAreas[0].AxisX.Minimum = 0;
            //chart2.ChartAreas[0].AxisX.Interval = 0.25;

            chart3.ChartAreas[0].AxisX.Minimum = 0;
            //chart3.ChartAreas[0].AxisX.Interval = 0.25;

            chart4.ChartAreas[0].AxisX.Minimum = 0;
            //chart4.ChartAreas[0].AxisX.Interval = 0.25;

            label_tx.Text = Math.Round(tX, 3).ToString();
            label_ty.Text = Math.Round(tY, 3).ToString();
            label_tz.Text = Math.Round(tZ, 3).ToString();
            label_x.Text = Math.Round(maxX, 3).ToString();
            label_y.Text = Math.Round(maxY, 3).ToString();
            label_z.Text = Math.Round(maxZ, 3).ToString();
            label_zcx.Text = ZC_X.ToString();
            label_zcy.Text = ZC_Y.ToString();
            label_zcz.Text = ZC_Z.ToString();
            label_Ax.Text = Math.Round(maxAx / 9.8, 4).ToString();
            label_Ay.Text = Math.Round(maxAy / 9.8, 4).ToString();
            label_Az.Text = Math.Round(maxAz / 9.8, 4).ToString();

   
            if (fileheader.Equals("BLS"))
            {
                label_peakvs.Text += Math.Round(maxVS,3) + "mm/s at " + Math.Round(tVS,3)+ " sec";
               
            }
            else if (fileheader.Equals("EVS"))
            {
                label2.Text = "Peak Particle Velocity (dB)";
                label_peakvs.Text += Math.Round(maxVS,3) + "dB at " + Math.Round(tVS,3) + "sec";
                
            }
            label_starttime.Text += start;
            label_endtime.Text += end;
            label_duration.Text += (Math.Round(duration,2).ToString())+ " sec";


            label_maxsound.Text += maxSound + "dB(A)";
           
           
        }

     


        Bitmap img;
        public void btn_print_Click(object sender, EventArgs e)
        {
            

            Point oldPosition = new Point(this.HorizontalScroll.Value, this.VerticalScroll.Value);

            panel1.PerformLayout();

            ComposedImage ci = new ComposedImage(new Size(panel1.DisplayRectangle.Width, panel1.DisplayRectangle.Height));

            int visibleWidth = panel1.Width - (panel1.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0);
            int visibleHeightBuffer = panel1.Height - (panel1.HorizontalScroll.Visible ? SystemInformation.HorizontalScrollBarHeight : 0);
           

            //int Iteration = 0;

            for (int x = panel1.DisplayRectangle.Width - visibleWidth; x >= 0; x -= visibleWidth)
            {

                int visibleHeight = visibleHeightBuffer;

                for (int y = panel1.DisplayRectangle.Height - visibleHeight; y >= 0; y -= visibleHeight)
                {
                    panel1.HorizontalScroll.Value = x;
                    panel1.VerticalScroll.Value = y;

                    panel1.PerformLayout();

                    Bitmap bmp = new Bitmap(visibleWidth, visibleHeight);

                    panel1.DrawToBitmap(bmp, new Rectangle(0, 0, visibleWidth, visibleHeight));
                    ci.images.Add(new ImagePart(new Point(x, y), bmp));

                   

                    if (y - visibleHeight < (panel1.DisplayRectangle.Height % visibleHeight))
                        visibleHeight = panel1.DisplayRectangle.Height % visibleHeight;

                    if (visibleHeight == 0)
                        break;
                }

                if (x - visibleWidth < (panel1.DisplayRectangle.Width % visibleWidth))
                    visibleWidth = panel1.DisplayRectangle.Width % visibleWidth;
                if (visibleWidth == 0)
                    break;
            }

            
            img = ci.composeImage();
            //printDocument1.DefaultPageSettings.Landscape = false;
            //printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custom", panel1.Width+20, panel1.DisplayRectangle.Height+40);
            panel1.HorizontalScroll.Value = oldPosition.X;
            panel1.VerticalScroll.Value = oldPosition.Y;
            printPreviewDialog1.Document = printDocument1;
         
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {

                printDocument1.Print();

            }
           
        }



        public class ComposedImage
        {
            public Size dimensions;
            public List<ImagePart> images;

            public ComposedImage(Size dimensions)
            {
                this.dimensions = dimensions;
                this.images = new List<ImagePart>();
            }

            public ComposedImage(Size dimensions, List<ImagePart> images)
            {
                this.dimensions = dimensions;
                this.images = images;
            }

            public Bitmap composeImage()
            {
                if (dimensions == null || images == null)
                    return null;

                Bitmap fullbmp = new Bitmap(dimensions.Width, dimensions.Height);
                using (Graphics grD = Graphics.FromImage(fullbmp))
                {
                    foreach (ImagePart bmp in images)
                    {
                        grD.DrawImage(bmp.image, bmp.location.X, bmp.location.Y);
                    }
                }
                return fullbmp;
            }
        }

        public class ImagePart
        {
            public Point location;
            public Bitmap image;

            public ImagePart(Point location, Bitmap image)
            {
                this.location = location;
                this.image = image;
            }
        }


        private void printDocument1_PrintPage(System.Object sender,
              System.Drawing.Printing.PrintPageEventArgs e)
        {
            Size resize = new Size(800, 1100);
            Bitmap resizeImage = new Bitmap(img, resize);
            //e.Graphics.DrawImage(img, 0, 0, img.Size.Width, img.Size.Height);
            e.Graphics.DrawImage(resizeImage, 0, 0);
        }

    }
    }



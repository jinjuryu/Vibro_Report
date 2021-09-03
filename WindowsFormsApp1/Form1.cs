using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string m_curPath = "";
        Thread m_thread;

        public Form1()
        {
            InitializeComponent();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_curPath = "Root";
            //label1.Text = m_curPath;

            TreeNode root = treeView1.Nodes.Add(m_curPath);

            string[] drives = Directory.GetLogicalDrives();

            foreach (string drive in drives)
            {
                DriveInfo di = new DriveInfo(drive);

                if (di.IsReady)
                {
                    TreeNode node = root.Nodes.Add(drive);
                    node.Nodes.Add("\\");
                }
            }

            ColumnHeader header1;
            header1 = new ColumnHeader();
            header1.Text = "File name";
            header1.TextAlign = HorizontalAlignment.Left;
            header1.Width = listView1.Width - 5;
            listView1.Columns.Add(header1);
            listView1.View = View.Details;





        }


        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text.Equals("\\"))
                {
                    e.Node.Nodes.Clear();

                    string path = e.Node.FullPath.Substring(e.Node.FullPath.IndexOf("\\") + 1);

                    string[] directories = Directory.GetDirectories(path);
                    foreach (string directory in directories)
                    {
                        TreeNode newNode = e.Node.Nodes.Add(directory.Substring(directory.LastIndexOf("\\") + 1));
                        newNode.Nodes.Add("\\");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("treeView1_BeforeExpand : " + ex.Message);
            }
        }

        private void ViewDirectoryList(string path)
        {
            if (m_thread != null && m_thread.IsAlive)
                m_thread.Abort();

            string curPath = path;

            Console.WriteLine(path.IndexOf("Root\\"));
            if (path.IndexOf("Root\\") == 0)
            {
                curPath = path.Substring(path.IndexOf("\\") + 1);
                m_curPath = (curPath.Length > 4) ? curPath.Remove(curPath.IndexOf("\\") + 1, 1) : curPath;
            }
            else
            {
                m_curPath = path;
            }

            try
            {
                listView1.Items.Clear();

                string[] directories = Directory.GetDirectories(curPath);

                foreach (string directory in directories)
                {
                    DirectoryInfo info = new DirectoryInfo(directory);
                    ListViewItem item = new ListViewItem(new string[]
                    {
                        info.Name
                    });
                    listView1.Items.Add(item);
                }

                string[] files = Directory.GetFiles(curPath);

                foreach (string file in files)
                {
                    FileInfo info = new FileInfo(file);
                    ListViewItem item = new ListViewItem(new string[]
                    {
                        info.Name
                    });
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ViewDirectoryList : " + ex.Message);
            }
        }

        private void SelectTreeView(TreeNode node)
        {
            if (node.FullPath == null)
            {
                Console.WriteLine("empth node.FullPath");
                return;
            }

            string path = node.FullPath;

            ViewDirectoryList(path);
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)    // 트리뷰에서 +버튼으로 항목을 펼친 후에 수행할 작업
        {
            SelectTreeView(e.Node);
        }

        private void btn_table_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedIndices.Count ==0)
            {
                MessageBox.Show("파일을 선택하세요.");
            }
            else
            {
                string filename = listView1.SelectedItems[0].Text;
                string filePath = m_curPath + "\\" + filename;

            
                if (!filename.Substring(0, 3).Equals("BLS") && !filename.Substring(0, 3).Equals("EVS")){
                        MessageBox.Show("올바른 형식의 파일을 선택하세요.");
                }
                else{
                    Form2 table = new Form2(filePath);
                    table.Show();
                }
            }
           

        }



       /* private void btn_chart_Click(object sender, EventArgs e)
        {
            string filePath = m_curPath + "\\" + listView1.SelectedItems[0].Text;
            Form3 chart = new Form3(filePath);
            chart.Show();
        }*/

        private void btn_report_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("파일을 선택하세요.");
            }
            else
            {
                string filename = listView1.SelectedItems[0].Text;
                string filePath = m_curPath + "\\" + filename;


                if (!filename.Substring(0, 3).Equals("BLS") && !filename.Substring(0, 3).Equals("EVS"))
                {
                    MessageBox.Show("올바른 형식의 파일을 선택하세요.");
                }
                else
                {
                    Form4 report = new Form4(filePath);
                    report.Show();
                }
            }

      
        }
    }
}


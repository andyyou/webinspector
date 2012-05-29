using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml.Linq;

namespace PxP
{
    public partial class GradeSetup : Form
    {
        #region Local variable
        private int ASCNumOfMarkGrade = 65;
        #endregion

        
        #region Constructor
        public GradeSetup()
        {
            InitializeComponent();
            InitialzeAllCustomObject();
        }

        ~GradeSetup()
        { 
            
        }
        #endregion

        #region Reflactoring
        
        //初始化所有物件
        void InitialzeAllCustomObject()
        {
            //Reload grade config
            SystemVariable.LoadGradeConfig();

            //Radio button get default value
            switch (GradeVariable.RoiMode)
            { 
                case 0:
                    rbNoRoi.Checked = true;
                    break;
                case 1:
                    rbSymmetrical.Checked = true;
                    break;
                default:
                    rbNoRoi.Checked = true;
                    break;
            }

            //Textbox get default value
            if (GradeVariable.RoiGradeColumns > 0)
                tboxColumns.Text = GradeVariable.RoiGradeColumns.ToString();
            if (GradeVariable.RoiGradeRows > 0)
                tboxRows.Text = GradeVariable.RoiGradeRows.ToString();

            //set cobobox get xml list in config folder
            bsGradConfigList.DataSource = GetGradeConfList();
            cboxGradeConfigFile.DataSource = bsGradConfigList.DataSource;
            cboxGradeConfigFile.SelectedItem = SystemVariable.ConfigFileName.ToString().Substring(0, SystemVariable.GradeConfigFileName.ToString().LastIndexOf("."));

            //set columns gridview get xml default value
            bsColumns.DataSource = GradeVariable.RoiColumnsGrid;
            gvColumns.DataSource = bsColumns.DataSource;

            //set rows gridview get xml default vale
            bsRows.DataSource = GradeVariable.RoiRowsGrid;
            gvRows.DataSource = bsRows.DataSource;

            //subpiece comobox get list
            bsRoiList.DataSource = GetSubPieceList();
            cboxSubPieceOfGrade.DataSource = bsRoiList.DataSource;
            cboxSubPieceOfPoint.DataSource = bsRoiList.DataSource;

            //Grade setting > Point get grid value for ROI-11 ROI-12 etc...
            foreach (var p in GradeVariable.PointSubPieces)
            { 
                if(p.Name == cboxSubPieceOfPoint.Text)
                    bsPointSubPiece.DataSource = p.Grades;
            }
            
            gvPoint.DataSource = bsPointSubPiece.DataSource;
            gvPoint.Columns["ClassId"].Visible = false;
            gvPoint.Columns["ClassName"].ReadOnly = true;
            //Grade setting > Point get value of cobobox for enable 
            cboxEnablePTS.Checked = GradeVariable.IsPointEnable;

            //Grade setting > MarkGrade get value of cobobox for enable
            cboxEnableGrade.Checked = GradeVariable.IsMarkGradeEnable;

            //Grade setting > MarkGrade get grid value for ROI-11, ROI-12 etc...
            foreach (var m in GradeVariable.MarkGradeSubPieces)
            {
                if (m.Name == cboxSubPieceOfPoint.Text)
                    bsMarkSubPiece.DataSource = m.Grades;
            }
            gvGrade.DataSource = bsMarkSubPiece.DataSource;
            gvGrade.Columns["GradeName"].ReadOnly = true;
            //Grade setting > pass or fail of cobobox for enable
            cboxEnablePFS.Checked = GradeVariable.IsPassOrFailScoreEnable;


            //Grade setting > pass or fail of textbox for get filter score
            tboxFilterScore.Text = GradeVariable.PassOrFileScore.ToString();

            

        }
        //取得Folder底下所有XML清單
        List<string> GetGradeConfList()
        {

            List<string> ConfList = new List<string>();
            string ConfPath = Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "/../Parameter Files/CPxP/grade/";
            DirectoryInfo di = new DirectoryInfo(ConfPath);
            FileInfo[] rgFiles = di.GetFiles("*.xml");

            foreach (FileInfo fi in rgFiles)
            {
                ConfList.Add(fi.Name.ToString().Substring(0, fi.Name.ToString().LastIndexOf(".")));
            }
            return ConfList;
        }
        //get subpiece list
        List<string> GetSubPieceList()
        {
            List<string> result = new List<string>();
            result.Add("ALL");
            foreach (var r in GradeVariable.RoiRowsGrid)
            {
                foreach (var c in GradeVariable.RoiColumnsGrid)
                {

                    string tmp =  "ROI-" + r.Name + c.Name;
                    result.Add(tmp);
                }
            }
            return result;

        }
        //轉換ASCII Number
        public static char Chr(int Num)
        {
            char C = Convert.ToChar(Num);
            return C;
        }
        //轉換Char to ASCnumber
        public static int ASC(string S)
        {
            int N = Convert.ToInt32(S[0]);
            return N;
        }
        #endregion

       

        #region Action Events
        //cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string x = "";
            foreach (var i in GradeVariable.PointSubPieces)
            {
                x += i.Name + ": ";
                foreach (var j in i.Grades)
                {
                    x += j.Score + ", ";
                }
                x += "\n";
            }
            MessageBox.Show(x);
            //this.Close();
        }
        //save
        private void btnSaveGradeConfigFile_Click(object sender, EventArgs e)
        {
            string FolderPath = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\grade\\";
            string FullSystemPath = FolderPath + cboxGradeConfigFile.Text + ".xml";
            
            XDocument XDoc = XDocument.Load(FullSystemPath);
            XElement XGradeConfFile = XDoc.Element("SystemConfig").Element("GradeConfigFile");
        }
        //select subpiece like ROI-11, ROI-12
        private void cboxSubPieceOfPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var p in GradeVariable.PointSubPieces)
            {
                if (p.Name == cboxSubPieceOfPoint.Text)
                {
                    bsPointSubPiece.DataSource = p.Grades;
                    bsPointSubPiece.ResetBindings(false);
                    bsPointSubPiece.ResumeBinding();
                }
            }
            gvPoint.DataSource = bsPointSubPiece.DataSource;
            
        }
        private void cboxSubPieceOfGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var m in GradeVariable.MarkGradeSubPieces)
            {
                if (m.Name == cboxSubPieceOfGrade.Text)
                {
                    bsMarkSubPiece.DataSource = m.Grades;
                    bsMarkSubPiece.ResetBindings(false);
                    bsMarkSubPiece.ResumeBinding();
                }
            }
            gvGrade.DataSource = bsMarkSubPiece.DataSource;
        }
        #endregion

        private void gvGrade_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Get all rows entered on each press of Enter.
            if (e.RowIndex == gvGrade.Rows.Count - 1 && !String.IsNullOrEmpty(gvGrade.Rows[e.RowIndex].Cells[1].Value.ToString()))
            {
                foreach (var m in GradeVariable.MarkGradeSubPieces)
                {
                    if (m.Name == cboxSubPieceOfGrade.Text)
                    {
                        MarkGrade tmp = new MarkGrade();
                        if (e.RowIndex < 26)
                        {
                            tmp.GradeName = Chr(e.RowIndex + 66).ToString();
                            m.Grades.Add(tmp);
                        }
                        bsMarkSubPiece.DataSource = m.Grades;
                    }
                }
                gvGrade.DataSource = typeof(MarkSubPiece);
                gvGrade.DataSource = bsMarkSubPiece.DataSource;
            }


        }

      

        

       

       

       

       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace PxP
{
    public partial class MapSetup : Form
    {
        #region Local Variable
        string tmpMDSize = "";
        string tmpCDSize = "";
        #endregion
        #region Constructor
        public MapSetup()
        {
            InitializeComponent();
            InitAllObject();
        }

        ~MapSetup()
        {
            
        }
        #endregion
        #region Reflactoring
        //取得Folder底下所有XML清單
        List<string> GetConfList()
        {
            List<string> ConfList = new List<string>();
            string ConfPath = Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "/../Parameter Files/CPxP/conf/";
            DirectoryInfo di = new DirectoryInfo(ConfPath);
            FileInfo[] rgFiles = di.GetFiles("*.xml");

            foreach (FileInfo fi in rgFiles)
            {
                ConfList.Add(fi.Name.ToString().Substring(0,fi.Name.ToString().LastIndexOf(".")));
            }
            return ConfList;
        }
        //載入所有物件預設值
        void InitAllObject()
        {
            ///////////////////////////////////////////////////////////////////////////////////////
            //組態檔繫結
            bsConfList.DataSource = GetConfList();
            cboxConfList.DataSource = bsConfList.DataSource;
            cboxConfList.SelectedItem = SystemVariable.ConfigFileName.ToString().Substring(0, SystemVariable.ConfigFileName.ToString().LastIndexOf("."));
            ///////////////////////////////////////////////////////////////////////////////////////
            ndImgCols.Value = PxPVariable.ImgColsSet;
            ndImgRows.Value = PxPVariable.ImgRowsSet;
            ///////////////////////////////////////////////////////////////////////////////////////
            cboxMapSize.SelectedIndex = MapWindowVariable.MapProportion;  //紀錄Map比例 0->1:1, 2->1:1, 2->2:1, 3->4:3, 4->3:4,  5->16:9
            ///////////////////////////////////////////////////////////////////////////////////////
            rbMapGridOff.Checked = !MapWindowVariable.ShowGridSet;
            rbMapGridOn.Checked = MapWindowVariable.ShowGridSet;
            switch (MapWindowVariable.SeriesSet)
            {
                case 0:  //形伏
                    rbSharp.Checked = true;
                    break;
                case 1:  //字形
                    rbLetter.Checked = true;
                    break;
            }
            ///////////////////////////////////////////////////////////////////////////////////////
            switch (MapWindowVariable.MapGridSet)
            {   //0: EachCellSize , 1: EachCellCount
                case 0:
                    rbFixCellSize.Checked = true;
                    tboxFixSizeCD.Text = MapWindowVariable.MapCDSet.ToString();
                    tboxFixSizeMD.Text = MapWindowVariable.MapMDSet.ToString();
                    tboxCountSizeCD.Text = "";
                    tboxCountSizeMD.Text = "";

                    break;
                case 1:
                    rbCountSize.Checked = true;
                    tboxFixSizeCD.Text = "";
                    tboxFixSizeMD.Text = "";
                    tboxCountSizeCD.Text = MapWindowVariable.MapCDSet.ToString();
                    tboxCountSizeMD.Text = MapWindowVariable.MapMDSet.ToString();
                    break;
            }
            ///////////////////////////////////////////////////////////////////////////////////////
            tboxFixSizeCD.Enabled = rbFixCellSize.Checked;
            tboxFixSizeMD.Enabled = rbFixCellSize.Checked;
            tboxCountSizeCD.Enabled = rbCountSize.Checked;
            tboxCountSizeMD.Enabled = rbCountSize.Checked;
            ///////////////////////////////////////////////////////////////////////////////////////
            cboxButtomAxe.SelectedIndex = MapWindowVariable.BottomAxe;

            ///////////////////////////////////////////////////////////////////////////////////////
            cbCDInverse.Checked = (MapWindowVariable.CDInver == 1) ? true : false;
            cbMDInverse.Checked = (MapWindowVariable.MDInver == 1) ? true : false;
        }
        #endregion

        
        #region Method
        
        #endregion

        #region Action Events
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            
            try
            {
                //Save config of setup filename 
                string FolderPath = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\sys_conf\\";
                string FullSystemPath = FolderPath + "sys.xml";
                XDocument XDoc = XDocument.Load(FullSystemPath);
                XElement XConfFile = XDoc.Element("SystemConfig").Element("ConfFile");
                XConfFile.Value = cboxConfList.SelectedItem.ToString();
                XDoc.Save(FullSystemPath);

                ////////////////////////////////////////////////////////////////////////////////

                string ConfPath = Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "/../Parameter Files/CPxP/conf/";
                string ConfFile = cboxConfList.SelectedItem.ToString();
                string FullConfPath = ConfPath + ConfFile + ".xml";
                XDocument XDocConf = XDocument.Load(FullConfPath);
                XElement xImgRowsSet = XDocConf.Element("Config").Element("MapVariable").Element("ImgRowsSet");
                xImgRowsSet.Value = ndImgRows.Value.ToString();
                XElement xImgColsSet = XDocConf.Element("Config").Element("MapVariable").Element("ImgColsSet");
                xImgColsSet.Value = ndImgCols.Value.ToString();
                XElement xMapProportion = XDocConf.Element("Config").Element("MapVariable").Element("MapProportion");
                xMapProportion.Value = cboxMapSize.SelectedIndex.ToString();
                XElement xShowGridSet = XDocConf.Element("Config").Element("MapVariable").Element("ShowGridSet");
                xShowGridSet.Value = rbMapGridOn.Checked ? "1":"0";
                XElement xMapGridSet = XDocConf.Element("Config").Element("MapVariable").Element("MapGridSet"); //選擇使用的Grid間隔依據 0: EachCellSize , 1: EachCellCount
                xMapGridSet.Value = rbFixCellSize.Checked ? "0" : "1";

                XElement xMapMDSet = XDocConf.Element("Config").Element("MapVariable").Element("MapMDSet");
                XElement xMapCDSet = XDocConf.Element("Config").Element("MapVariable").Element("MapCDSet");
                if (rbFixCellSize.Checked)
                {
                    xMapMDSet.Value = tboxFixSizeMD.Text;
                    xMapCDSet.Value = tboxFixSizeCD.Text;
                    MapWindowVariable.MapCDSet = double.TryParse(tboxFixSizeCD.Text, out MapWindowVariable.MapCDSet) ? MapWindowVariable.MapCDSet : 3;
                    MapWindowVariable.MapMDSet = double.TryParse(tboxFixSizeMD.Text, out MapWindowVariable.MapMDSet) ? MapWindowVariable.MapMDSet : 3; 

                }
                else if (rbCountSize.Checked)
                {
                    xMapMDSet.Value = tboxCountSizeMD.Text;
                    xMapCDSet.Value = tboxCountSizeCD.Text;
                    MapWindowVariable.MapCDSet = double.TryParse(tboxCountSizeCD.Text, out MapWindowVariable.MapCDSet) ? MapWindowVariable.MapCDSet : 3;
                    MapWindowVariable.MapMDSet = double.TryParse(tboxCountSizeMD.Text, out MapWindowVariable.MapMDSet) ? MapWindowVariable.MapMDSet : 3; 
                }

                XElement xSeriesSet = XDocConf.Element("Config").Element("MapVariable").Element("SeriesSet");
                if (rbSharp.Checked)
                    xSeriesSet.Value = "0";
                else if (rbLetter.Checked)
                    xSeriesSet.Value = "1";
                XElement xBottomAxe = XDocConf.Element("Config").Element("MapVariable").Element("BottomAxe");
                xBottomAxe.Value = cboxButtomAxe.SelectedIndex.ToString();

                XElement xMDInver = XDocConf.Element("Config").Element("MapVariable").Element("MDInver");
                xMDInver.Value = cbMDInverse.Checked ? "1" : "0";
                XElement xCDInver = XDocConf.Element("Config").Element("MapVariable").Element("CDInver");
                xCDInver.Value = cbCDInverse.Checked ? "1" : "0";
                XDocConf.Save(FullConfPath);
                
                
                ////////////////////////////////////////////////////////////////////////////////
                
                MessageBox.Show("設定已套用！");
            }
            catch (Exception ex)
            {
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void rbFixCellSize_CheckedChanged(object sender, EventArgs e)
        {
            tboxFixSizeCD.Enabled = rbFixCellSize.Checked;
            tboxFixSizeMD.Enabled = rbFixCellSize.Checked;
            tboxCountSizeCD.Enabled = rbCountSize.Checked;
            tboxCountSizeMD.Enabled = rbCountSize.Checked;
            tboxFixSizeCD.Text = MapWindowVariable.MapCDSet.ToString();
            tboxFixSizeMD.Text = MapWindowVariable.MapMDSet.ToString();
            tboxCountSizeCD.Text = "";
            tboxCountSizeMD.Text = "";
        }
        private void rbCountSize_CheckedChanged(object sender, EventArgs e)
        {
            tboxFixSizeCD.Enabled = rbFixCellSize.Checked;
            tboxFixSizeMD.Enabled = rbFixCellSize.Checked;
            tboxCountSizeCD.Enabled = rbCountSize.Checked;
            tboxCountSizeMD.Enabled = rbCountSize.Checked;
            tboxCountSizeCD.Text = MapWindowVariable.MapCDSet.ToString();
            tboxCountSizeMD.Text = MapWindowVariable.MapMDSet.ToString();
            tboxFixSizeCD.Text = "";
            tboxFixSizeMD.Text = "";
            
        }
        #endregion

       

        

        

       
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using PxP.Toolkit;

namespace PxP
{
    public partial class MapSetup : Form
    {
        #region Local Variables

        IList<FlawTypeNameExtend> TmpFlawTypeName;
        DataSet UnitsData = new DataSet();
        bool IsFirstLoadConfig = false;
        #endregion

        #region Constructor

        public MapSetup()
        {
            InitializeComponent();
            InitAllObject();
            IsFirstLoadConfig = true;

        }

        ~MapSetup()
        {
           
        }

        #endregion

        #region Reflactoring

        // 取得 Folder 底下所有 XML 清單
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

        // 載入所有物件預設值
        void InitAllObject()
        {
            // 組態檔繫結
            bsConfList.DataSource = GetConfList();
            cboxConfList.DataSource = bsConfList.DataSource;
            cboxConfList.SelectedItem = SystemVariable.ConfigFileName.ToString().Substring(0, SystemVariable.ConfigFileName.ToString().LastIndexOf("."));

            ndImgCols.Value = PxPVariable.ImgColsSet;
            ndImgRows.Value = PxPVariable.ImgRowsSet;

            cboxMapSize.SelectedIndex = MapWindowVariable.MapProportion;  // 紀錄 Map 比例 0->1:1, 2->2:1, 2->4:3, 3->3:4, 4->16:9

            rbMapGridOff.Checked = !MapWindowVariable.ShowGridSet;
            rbMapGridOn.Checked = MapWindowVariable.ShowGridSet;
            switch (MapWindowVariable.SeriesSet)
            {
                case 0:  // 形伏
                    rbSharp.Checked = true;
                    break;
                case 1:  // 字母
                    rbLetter.Checked = true;
                    break;
            }

            switch (MapWindowVariable.MapGridSet)
            {
                case 0:  // EachCellSize
                    rbFixCellSize.Checked = true;
                    tboxFixSizeCD.Text = MapWindowVariable.MapCDSet.ToString();
                    tboxFixSizeMD.Text = MapWindowVariable.MapMDSet.ToString();
                    tboxCountSizeCD.Text = "";
                    tboxCountSizeMD.Text = "";
                    break;

                case 1:  // EachCellCount
                    rbCountSize.Checked = true;
                    tboxFixSizeCD.Text = "";
                    tboxFixSizeMD.Text = "";
                    tboxCountSizeCD.Text = MapWindowVariable.MapCDSet.ToString();
                    tboxCountSizeMD.Text = MapWindowVariable.MapMDSet.ToString();
                    break;
            }

            tboxFixSizeCD.Enabled = rbFixCellSize.Checked;
            tboxFixSizeMD.Enabled = rbFixCellSize.Checked;
            tboxCountSizeCD.Enabled = rbCountSize.Checked;
            tboxCountSizeMD.Enabled = rbCountSize.Checked;

            cboxButtomAxe.SelectedIndex = MapWindowVariable.BottomAxe;

            cbCDInverse.Checked = (MapWindowVariable.CDInver == 1) ? true : false;
            cbMDInverse.Checked = (MapWindowVariable.MDInver == 1) ? true : false;

            // Change Specify Cell Size Unit Label
            lbSCCD.Text = PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw Map CD"]].ItemArray[1].ToString();
            lbSCMD.Text = PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw Map MD"]].ItemArray[1].ToString();

            // Add List here and binding to datagridvew
            if (PxPVariable.FlawTypeName != null && PxPVariable.FlawTypeName.Count > 0)
            {
                TmpFlawTypeName = new List<FlawTypeNameExtend>();
                foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
                {
                    FlawTypeNameExtend tmp = new FlawTypeNameExtend();
                    tmp.Color = ft.Color;
                    tmp.FlawType = ft.FlawType;
                    tmp.Name = ft.Name;
                    tmp.Shape = ft.Shape;
                    tmp.Display = ft.Display;
                    tmp.Count = ft.Count;
                    tmp.JobNum = ft.JobNum;
                    tmp.DoffNum = ft.DoffNum;
                    TmpFlawTypeName.Add(tmp);
                }

                bsFlawTypeName.DataSource = TmpFlawTypeName;
                gvSeries.DataSource = bsFlawTypeName;
                gvSeries.AutoGenerateColumns = false;
               
                foreach (DoffGridColumns column in MapWindowVariable.DoffTypeGridSetup)
                {
                    gvSeries.Columns[column.ColumnName].SortMode = DataGridViewColumnSortMode.Automatic;
                    gvSeries.Columns[column.ColumnName].HeaderText = column.HeaderText;
                    gvSeries.Columns[column.ColumnName].DisplayIndex = column.Index;
                    gvSeries.Columns[column.ColumnName].Width = column.Width;

                    if (column.ColumnName == "Shape")
                    {
                        DataGridViewComboBoxColumn cboxShape = new DataGridViewComboBoxColumn();
                        cboxShape.DataPropertyName = "Shape";
                        cboxShape.HeaderText = column.HeaderText;
                        cboxShape.DisplayIndex = column.Index;
                        cboxShape.Width = column.Width;
                        cboxShape.DataSource = EnumHelper.ToList(typeof(Shape));
                        cboxShape.DisplayMember = "Value";
                        cboxShape.ValueMember = "Value";

                        this.gvSeries.Columns.Add(cboxShape);
                     }
                }
                // Display and Change Order
                gvSeries.Columns["FlawType"].DisplayIndex = 0;
                gvSeries.Columns["Display"].Visible = false;
                gvSeries.Columns["Count"].Visible = false;
                gvSeries.Columns["DoffNum"].Visible = false;
                gvSeries.Columns["Shape"].Visible = false;
                gvSeries.Columns["JobNum"].Visible = false;
            }
        }
       
        #endregion
        
        #region Action Events

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                // Save config of setup filename 
                string FolderPath = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\sys_conf\\";
                string FullSystemPath = FolderPath + "sys.xml";
                XDocument XDoc = XDocument.Load(FullSystemPath);
                XElement XConfFile = XDoc.Element("SystemConfig").Element("ConfFile");
                XConfFile.Value = cboxConfList.SelectedItem.ToString();
                SystemVariable.ConfigFileName = cboxConfList.SelectedItem.ToString() + ".xml";
                XDoc.Save(FullSystemPath);

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
               
                // Save FlawTypeName Grid
                if (gvSeries.Rows.Count > 0)
                {
                    XDocConf.Element("Config").Element("MapVariable").Elements("FlawTypeName").Remove();
                    foreach (DataGridViewRow dr in gvSeries.Rows)
                    {
                        XDocConf.Element("Config").Element("MapVariable").Add(new XElement("FlawTypeName",
                            new XElement("FlawType", dr.Cells[1].Value.ToString()),
                            new XElement("Name", dr.Cells[0].Value.ToString()),
                            new XElement("Color", dr.Cells[4].Value.ToString()),
                            new XElement("Shape", EnumHelper.GetItemString(dr.Cells[5].Value.ToString())),
                            new XElement("Display", (dr.Cells[2].Value.ToString().ToLower().Trim() == "true") ? "1" : "0")
                            ));
                    }
                }
                XDocConf.Save(FullConfPath);

                // Change Global Variable
                PxPVariable.ImgRowsSet = int.TryParse(xImgRowsSet.Value, out PxPVariable.ImgRowsSet) ? PxPVariable.ImgRowsSet : 3;
                PxPVariable.ImgColsSet = int.TryParse(xImgColsSet.Value, out PxPVariable.ImgColsSet) ? PxPVariable.ImgColsSet : 3;
                MapWindowVariable.MapProportion = cboxMapSize.SelectedIndex;
                MapWindowVariable.ShowGridSet = rbMapGridOn.Checked ? true : false;
                MapWindowVariable.MapGridSet = rbFixCellSize.Checked ? 0 : 1;
                MapWindowVariable.BottomAxe = cboxButtomAxe.SelectedIndex;
                MapWindowVariable.MDInver = cbMDInverse.Checked ? 1 : 0;
                MapWindowVariable.CDInver = cbCDInverse.Checked ? 1 : 0;
                PxPVariable.FlawTypeName.Clear();
                if (TmpFlawTypeName != null)
                    PxPVariable.FlawTypeName.AddRange(TmpFlawTypeName);
                
                MapWindowVariable.MapWindowController.SetMapProperty();
                MapWindowVariable.MapWindowController.SetMapAxis();
                MapWindowThreadStatus.IsPageRefresh = true;
                PxPTab.MapThreadEvent.Set();
                MessageBox.Show("Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
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

        private void gvSeries_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4)
                e.CellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(e.Value.ToString());
        }

        private void gvSeries_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (e.ColumnIndex == 4)
            {
                DialogResult dr = cd.ShowDialog();
                if (dr == DialogResult.OK)
                    gvSeries.Rows[e.RowIndex].Cells[4].Value = String.Format("#{0:X2}{1:X2}{2:X2}", cd.Color.R, cd.Color.G, cd.Color.B);
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            string ConfPath = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "/../Parameter Files/CPxP/conf/";

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = " txt files(*.xml)|*.xml|All files(*.*)|*.*";
            sfd.Title = "Save New Config File";
            sfd.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles) + "\\Web Ranger\\Parameter Files\\CPxP\\conf\\";
            sfd.CreatePrompt = false;
            sfd.OverwritePrompt = true;
            sfd.AddExtension = true;
            sfd.CheckFileExists = false;
            sfd.FileName = "Custom-Config-" + DateTime.Now.ToString("yyyy-MM-dd");
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                try
                {
                    // Save config of setup filename 
                    string FolderPath = Path.GetDirectoryName(
                        Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\sys_conf\\";
                    string FullSystemPath = FolderPath + "sys.xml";
                    XDocument XDoc = XDocument.Load(FullSystemPath);
                    XElement XConfFile = XDoc.Element("SystemConfig").Element("ConfFile");
                    XConfFile.Value = cboxConfList.SelectedItem.ToString();
                    XDoc.Save(FullSystemPath);

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
                    xShowGridSet.Value = rbMapGridOn.Checked ? "1" : "0";
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

                    // Save FlawTypeName Grid
                    XDocConf.Element("Config").Element("MapVariable").Elements("FlawTypeName").Remove();
                    foreach (DataGridViewRow dr in gvSeries.Rows)
                    {
                        XDocConf.Element("Config").Element("MapVariable").Add(new XElement("FlawTypeName",
                            new XElement("FlawType", dr.Cells[1].Value.ToString()),
                            new XElement("Name", dr.Cells[0].Value.ToString()),
                            new XElement("Color", dr.Cells[4].Value.ToString()),
                            new XElement("Shape", EnumHelper.GetItemString(dr.Cells[5].Value.ToString())),
                            new XElement("Display", (dr.Cells[2].Value.ToString().ToLower().Trim() == "true") ? "1" : "0")
                            ));
                    }
                    XDocConf.Save(fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        #endregion

        #region 增加ILIST To DataTable 模組功能

        public static DataTable ListToDataTable(IList ResList)
        {
            DataTable TempDT = new DataTable();

            System.Reflection.PropertyInfo[] p = ResList[0].GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo pi in p)
            {
                TempDT.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString()));
            }

            for (int i = 0; i < ResList.Count; i++)
            {
                IList TempList = new ArrayList();
                foreach (System.Reflection.PropertyInfo pi in p)
                {
                    object oo = pi.GetValue(ResList[i], null);
                    TempList.Add(oo);
                }

                object[] itm = new object[p.Length];
                for (int j = 0; j < TempList.Count; j++)
                {
                    itm.SetValue(TempList[j], j);
                }
                TempDT.LoadDataRow(itm, true);
            }
            return TempDT;
        }
    
        #endregion

        private void cboxConfList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsFirstLoadConfig)
            {

                SystemVariable.ConfigFileName = cboxConfList.SelectedItem.ToString() + ".xml";
                string path = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\CPxP\\conf\\";
                string FullFilePath = string.Format("{0}{1}", path, SystemVariable.ConfigFileName);
                XDocument xdSetupXml = XDocument.Load(FullFilePath);
                XDocument XConf = xdSetupXml;
                XElement MapVariable = XConf.Element("Config").Element("MapVariable");
                try
                {
                    PxPVariable.ImgRowsSet = int.Parse(MapVariable.Element("ImgRowsSet").Value);
                    PxPVariable.ImgColsSet = int.Parse(MapVariable.Element("ImgColsSet").Value);
                    MapWindowVariable.MapProportion = int.Parse(MapVariable.Element("MapProportion").Value);
                    MapWindowVariable.ShowGridSet = (int.Parse(MapVariable.Element("ShowGridSet").Value) == 1) ? true : false;
                    MapWindowVariable.MapGridSet = int.Parse(MapVariable.Element("MapGridSet").Value);
                    MapWindowVariable.MapMDSet = double.Parse(MapVariable.Element("MapMDSet").Value);
                    MapWindowVariable.MapCDSet = double.Parse(MapVariable.Element("MapCDSet").Value);
                    MapWindowVariable.SeriesSet = int.Parse(MapVariable.Element("SeriesSet").Value);
                    MapWindowVariable.BottomAxe = int.Parse(MapVariable.Element("BottomAxe").Value);
                    MapWindowVariable.MDInver = int.Parse(MapVariable.Element("MDInver").Value);
                    MapWindowVariable.CDInver = int.Parse(MapVariable.Element("CDInver").Value);
                    MapWindowVariable.ShowFlag = int.Parse(MapVariable.Element("ShowFlag").Value);
                    MapWindowVariable.LastMapCDConvertion = double.Parse(MapVariable.Element("LastMapCDConvertion").Value);
                    MapWindowVariable.LastMapMDConvertion = double.Parse(MapVariable.Element("LastMapMDConvertion").Value);

                    PxPVariable.PageSize = PxPVariable.ImgRowsSet * PxPVariable.ImgColsSet;
                }
                catch (Exception ex)
                {
                    PxPVariable.ImgRowsSet = 2;
                    PxPVariable.ImgColsSet = 2;
                    MapWindowVariable.MapProportion = 0;
                    MapWindowVariable.ShowGridSet = true;
                    MapWindowVariable.MapGridSet = 1;
                    MapWindowVariable.MapMDSet = 3;
                    MapWindowVariable.MapCDSet = 3;
                    MapWindowVariable.SeriesSet = 0;
                    MapWindowVariable.BottomAxe = 0;
                    MapWindowVariable.MDInver = 0;
                    MapWindowVariable.CDInver = 0;
                    MapWindowVariable.LastMapCDConvertion = 1.00;
                    MapWindowVariable.LastMapMDConvertion = 1.00;
                }

                IEnumerable<XElement> xMapFlawTypeName = XConf.Element("Config").Element("MapVariable").Elements("FlawTypeName");
                try
                {
                    PxPVariable.FlawTypeName.Clear();
                    foreach (XElement el in xMapFlawTypeName)
                    {
                        FlawTypeNameExtend tmp = new FlawTypeNameExtend();
                        tmp.Display = (int.Parse(el.Element("Display").Value) == 1) ? true : false;
                        tmp.Color = el.Element("Color").Value.ToString();
                        tmp.Name = el.Element("Name").Value.ToString();
                        tmp.Shape = ((Shape)Enum.Parse(typeof(Shape), el.Element("Shape").Value.ToString(), false)).ToGraphic();

                        tmp.FlawType = int.Parse(el.Element("FlawType").Value.ToString());
                        PxPVariable.FlawTypeName.Add(tmp);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("載入 /CPxP/conf/setup.xml :  MapDoffTypeGrid \n" + ex.Message);
                }

                ndImgCols.Value = PxPVariable.ImgColsSet;
                ndImgRows.Value = PxPVariable.ImgRowsSet;

                cboxMapSize.SelectedIndex = MapWindowVariable.MapProportion;  // 紀錄 Map 比例 0->1:1, 2->2:1, 2->4:3, 3->3:4, 4->16:9

                rbMapGridOff.Checked = !MapWindowVariable.ShowGridSet;
                rbMapGridOn.Checked = MapWindowVariable.ShowGridSet;
                switch (MapWindowVariable.SeriesSet)
                {
                    case 0:  // 形伏
                        rbSharp.Checked = true;
                        break;
                    case 1:  // 字母
                        rbLetter.Checked = true;
                        break;
                }

                switch (MapWindowVariable.MapGridSet)
                {
                    case 0:  // EachCellSize
                        rbFixCellSize.Checked = true;
                        tboxFixSizeCD.Text = MapWindowVariable.MapCDSet.ToString();
                        tboxFixSizeMD.Text = MapWindowVariable.MapMDSet.ToString();
                        tboxCountSizeCD.Text = "";
                        tboxCountSizeMD.Text = "";
                        break;

                    case 1:  // EachCellCount
                        rbCountSize.Checked = true;
                        tboxFixSizeCD.Text = "";
                        tboxFixSizeMD.Text = "";
                        tboxCountSizeCD.Text = MapWindowVariable.MapCDSet.ToString();
                        tboxCountSizeMD.Text = MapWindowVariable.MapMDSet.ToString();
                        break;
                }

                tboxFixSizeCD.Enabled = rbFixCellSize.Checked;
                tboxFixSizeMD.Enabled = rbFixCellSize.Checked;
                tboxCountSizeCD.Enabled = rbCountSize.Checked;
                tboxCountSizeMD.Enabled = rbCountSize.Checked;

                cboxButtomAxe.SelectedIndex = MapWindowVariable.BottomAxe;

                cbCDInverse.Checked = (MapWindowVariable.CDInver == 1) ? true : false;
                cbMDInverse.Checked = (MapWindowVariable.MDInver == 1) ? true : false;

                // Change Specify Cell Size Unit Label
                lbSCCD.Text = PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw Map CD"]].ItemArray[1].ToString();
                lbSCMD.Text = PxPVariable.UnitsData.Tables["unit"].Rows[PxPVariable.UnitsKeys["Flaw Map MD"]].ItemArray[1].ToString();

                // Add List here and binding to datagridvew
                if (PxPVariable.FlawTypeName != null && PxPVariable.FlawTypeName.Count > 0)
                {
                    TmpFlawTypeName = new List<FlawTypeNameExtend>();
                    foreach (FlawTypeNameExtend ft in PxPVariable.FlawTypeName)
                    {
                        FlawTypeNameExtend tmp = new FlawTypeNameExtend();
                        tmp.Color = ft.Color;
                        tmp.FlawType = ft.FlawType;
                        tmp.Name = ft.Name;
                        tmp.Shape = ft.Shape;
                        tmp.Display = ft.Display;
                        tmp.Count = ft.Count;
                        tmp.JobNum = ft.JobNum;
                        tmp.DoffNum = ft.DoffNum;
                        TmpFlawTypeName.Add(tmp);
                    }
                    
                    bsFlawTypeName.DataSource = TmpFlawTypeName;
                    
                }
            }
        }
    }
}

// Decompiled with JetBrains decompiler
// Type: CascadeManager.MainForm
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using BasicComponents;
using CascadeManager.Properties;
using CS.Client.Common.Views;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using TS.Sdk.StaticFace.NetBinding;

namespace CascadeManager
{
  public class MainForm : XtraForm
  {
    public static string ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Technoserv\\";
    private IContainer components;
    public static IEngine Engine;
    public static BcUser CurrentUser;
    private bool _allowClose;
    private Icon _mainIcon;
    private FrmFaces _faceForm;
    private FrmObjects _objectForm;
    private FrmWorkStatins _stationForm;
    private FrmQueryTemplates _queryForm;
    private FrmUsers _userForm;
    private FrmResults _resultForm;
    private FrmStatistic _statisticForm;
    private FrmDbSearch _searchForm;
    private FrmCommonSettings _racursForm;
    private FrmCommonCategories _categoryForm;
    private FrmLogSearch _logSearchForm;
    private BarManager barManager1;
    private Bar _mainFunctionsBar;
    private BarDockControl barDockControlTop;
    private BarDockControl barDockControlBottom;
    private BarDockControl barDockControlLeft;
    private BarDockControl barDockControlRight;
    private BarLargeButtonItem btSettings;
    private BarLargeButtonItem btStatistic;
    private Bar bar1;
    private Bar bar3;
    private BarManager barManager2;
    private BarDockControl barDockControl4;
    private RepositoryItemImageEdit repositoryItemImageEdit1;
    private RepositoryItemButtonEdit repositoryItemButtonEdit1;
    private RepositoryItemTextEdit repositoryItemTextEdit1;
    private RepositoryItemDateEdit repositoryItemDateEdit1;
    private BarLargeButtonItem btObjects;
    private BarLargeButtonItem btFaces;
    private BarLargeButtonItem btUsers;
    private BarLargeButtonItem btResults;
    private BarLargeButtonItem btWorkStations;
    private BarLargeButtonItem btCategories;
    private BarLargeButtonItem btSearch;
    private BarLargeButtonItem btLogSearch;
    private BarLargeButtonItem btQueryTemplates;

    public MainForm()
    {
      InitializeComponent();
      try
      {
        FrmFaces frmFaces = new FrmFaces();
        frmFaces.MdiParent = this;
        _faceForm = frmFaces;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + "-3");
      }
    }

    public static bool SetScore(float[] val)
    {
      try
      {
        SqlCommand sqlCommand1 = new SqlCommand("Select *from ScoreSettings", new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand1.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
        bool flag = false;
        if (sqlDataReader.Read())
          flag = true;
        sqlCommand1.Connection.Close();
        if (flag)
        {
          CultureInfo cultureInfo = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
          cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
          cultureInfo.DateTimeFormat.DateSeparator = ".";
          cultureInfo.DateTimeFormat.PMDesignator = "";
          cultureInfo.DateTimeFormat.AMDesignator = "";
          cultureInfo.DateTimeFormat.TimeSeparator = ":";
          cultureInfo.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
          cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
          cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
          Thread.CurrentThread.CurrentCulture = cultureInfo;
          SqlCommand sqlCommand2 = new SqlCommand("update ScoreSettings set Score1 = @val1,\r\nScore2 = @val2,\r\nQuality = @val3,\r\nPeriod = @val4, Time = @val5, Yaw = @val6, Inplane = @val7", new SqlConnection(CommonSettings.ConnectionString));
          sqlCommand2.Parameters.AddWithValue("@val1", val[0]);
          sqlCommand2.Parameters.AddWithValue("@val2", val[1]);
          sqlCommand2.Parameters.AddWithValue("@val3", val[2]);
          sqlCommand2.Parameters.AddWithValue("@val4", val[3]);
          sqlCommand2.Parameters.AddWithValue("@val5", val[4]);
          sqlCommand2.Parameters.AddWithValue("@val6", val[5]);
          sqlCommand2.Parameters.AddWithValue("@val7", val[6]);
          sqlCommand2.Connection.Open();
          sqlCommand2.ExecuteNonQuery();
          sqlCommand2.Connection.Close();
          return true;
        }
        CultureInfo cultureInfo1 = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
        cultureInfo1.NumberFormat.NumberDecimalSeparator = ",";
        cultureInfo1.DateTimeFormat.DateSeparator = ".";
        cultureInfo1.DateTimeFormat.PMDesignator = "";
        cultureInfo1.DateTimeFormat.AMDesignator = "";
        cultureInfo1.DateTimeFormat.TimeSeparator = ":";
        cultureInfo1.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
        cultureInfo1.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
        cultureInfo1.DateTimeFormat.LongTimePattern = "HH:mm:ss";
        Thread.CurrentThread.CurrentCulture = cultureInfo1;
        SqlCommand sqlCommand3 = new SqlCommand("Insert into ScoreSettings (Score1,\r\nScore2,\r\nQuality ,\r\nPeriod, Time,Yaw,Inplane) Values(@val1,@val2,@val3,@val4,@val5,@val6,@val7)", new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand3.Parameters.AddWithValue("@val1", val[0]);
        sqlCommand3.Parameters.AddWithValue("@val2", val[1]);
        sqlCommand3.Parameters.AddWithValue("@val3", val[2]);
        sqlCommand3.Parameters.AddWithValue("@val4", val[3]);
        sqlCommand3.Parameters.AddWithValue("@val5", val[4]);
        sqlCommand3.Parameters.AddWithValue("@val6", val[5]);
        sqlCommand3.Parameters.AddWithValue("@val7", val[6]);
        sqlCommand3.Connection.Open();
        sqlCommand3.ExecuteNonQuery();
        sqlCommand3.Connection.Close();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public static float[] GetScore()
    {
      CultureInfo cultureInfo = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
      cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
      cultureInfo.DateTimeFormat.DateSeparator = ".";
      cultureInfo.DateTimeFormat.PMDesignator = "";
      cultureInfo.DateTimeFormat.AMDesignator = "";
      cultureInfo.DateTimeFormat.TimeSeparator = ":";
      cultureInfo.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
      cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
      cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
      float[] numArray = new float[7];
      try
      {
        SqlCommand sqlCommand = new SqlCommand("Select * from ScoreSettings", new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
          numArray[0] = Convert.ToSingle(sqlDataReader[0].ToString().Replace(".", ","));
          numArray[1] = Convert.ToSingle(sqlDataReader[1].ToString().Replace(".", ","));
          numArray[2] = Convert.ToSingle(sqlDataReader[2].ToString().Replace(".", ","));
          numArray[3] = Convert.ToSingle(sqlDataReader[3].ToString().Replace(".", ","));
          numArray[4] = Convert.ToSingle(sqlDataReader[4].ToString().Replace(".", ","));
          numArray[5] = Convert.ToSingle(sqlDataReader[5].ToString().Replace(".", ","));
          numArray[6] = Convert.ToSingle(sqlDataReader[6].ToString().Replace(".", ","));
        }
        sqlCommand.Connection.Close();
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return numArray;
    }

    private void EmplooyersForm_Load(object sender, EventArgs e)
    {
      try
      {
        Bitmap bitmap = new Bitmap(1, 1);
        Graphics.FromImage(bitmap).FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 1, 1));
        _mainIcon = Icon.FromHandle(bitmap.GetHicon());
      }
      catch
      {
      }
      AuthorizationForm authorizationForm = new AuthorizationForm("CascadeManager", new Messages());
      if (authorizationForm.ShowDialog() != DialogResult.OK)
      {
        _allowClose = true;
        Close();
      }
      else
      {
        CurrentUser = authorizationForm.User;
        if (CurrentUser.Role == 2)
          btUsers.Enabled = true;
        else
          btUsers.Enabled = false;
        int[] actions = CurrentUser.GetActions();
        for (int index = 0; index < actions.Length; ++index)
        {
          if (actions[index] == 8)
            btObjects.Enabled = true;
          if (actions[index] == 7)
            btSettings.Enabled = true;
          if (actions[index] == 0)
          {
            btFaces.Enabled = true;
            if (_faceForm == null || _faceForm.IsDisposed)
            {
              _faceForm = new FrmFaces();
              _faceForm.MdiParent = this;
            }
            _faceForm.Icon = _mainIcon;
            _faceForm.Show();
          }
          if (actions[index] == 12)
            btStatistic.Enabled = true;
          if (actions[index] == 13)
            btResults.Enabled = true;
          if (actions[index] == 10)
            btCategories.Enabled = true;
          if (actions[index] == 11)
            btWorkStations.Enabled = true;
          if (actions[index] == 6)
            btSearch.Enabled = true;
          if (actions[index] == 17)
            btLogSearch.Enabled = true;
          if (actions[index] == 4)
            btQueryTemplates.Enabled = true;
        }
      }
    }

    private void EmplooyersForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!_allowClose)
      {
        if (XtraMessageBox.Show(Messages.DoYouWantToExitNow, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        {
          e.Cancel = true;
        }
        else
        {
          if (_stationForm != null && _stationForm.IsDisposed)
            _stationForm.BreakFlag = true;
          if (_faceForm != null)
            _faceForm.Close();
          if (_resultForm != null)
            _resultForm.Close();
          if (_statisticForm != null)
            _statisticForm.Close();
          if (_objectForm != null)
            _objectForm.Close();
          if (_userForm != null)
            _userForm.Close();
        }
      }
      else
        Application.Exit();
    }

    private void btClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void btObjects_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_objectForm == null || _objectForm.IsDisposed)
      {
        _objectForm = new FrmObjects();
        _objectForm.MdiParent = this;
      }
      else
      {
        _objectForm.Dispose();
        _objectForm = new FrmObjects();
        _objectForm.MdiParent = this;
      }
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _objectForm.Icon = _mainIcon;
      _objectForm.Show();
      _objectForm.ControlBox = false;
      Activate();
    }

    private void biJornal_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_statisticForm == null || _statisticForm.IsDisposed)
      {
        _statisticForm = new FrmStatistic();
        _statisticForm.MdiParent = this;
      }
      else
        _statisticForm.MdiParent = this;
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _statisticForm.Icon = _mainIcon;
      _statisticForm.Show();
      _statisticForm.ControlBox = false;
      Activate();
    }

    private void btWorkStations_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_stationForm == null || _stationForm.IsDisposed)
      {
        _stationForm = new FrmWorkStatins();
        _stationForm.MdiParent = this;
      }
      else
        _stationForm.MdiParent = this;
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _stationForm.Icon = _mainIcon;
      _stationForm.Show();
      _stationForm.ControlBox = false;
      Activate();
    }

    private void btCategories_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_categoryForm == null || _categoryForm.IsDisposed)
      {
        _categoryForm = new FrmCommonCategories();
        _categoryForm.MdiParent = this;
      }
      else
      {
        _categoryForm.Dispose();
        _categoryForm = new FrmCommonCategories();
        _categoryForm.MdiParent = this;
      }
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _categoryForm.Icon = _mainIcon;
      _categoryForm.Show();
      _categoryForm.ControlBox = false;
      Activate();
    }

    private void btSearch_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_searchForm == null || _searchForm.IsDisposed)
      {
        _searchForm = new FrmDbSearch();
        _searchForm.MdiParent = this;
      }
      else
        _searchForm.MdiParent = this;
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _searchForm.Icon = _mainIcon;
      _searchForm.Show();
      _searchForm.ControlBox = false;
      Activate();
    }

    private void btFaces_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_faceForm == null || _faceForm.IsDisposed)
      {
        _faceForm = new FrmFaces();
        _faceForm.MdiParent = this;
      }
      else
      {
        _faceForm.Dispose();
        _faceForm = new FrmFaces();
        _faceForm.MdiParent = this;
      }
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _faceForm.Icon = _mainIcon;
      _faceForm.Show();
      _faceForm.ControlBox = false;
      Activate();
    }

    private void btResults_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_resultForm == null || _resultForm.IsDisposed)
      {
        _resultForm = new FrmResults();
        _resultForm.MdiParent = this;
      }
      else
      {
        _resultForm.MdiParent = this;
        _resultForm.ReloadObjects();
      }
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _resultForm.Icon = _mainIcon;
      _resultForm.Show();
      _resultForm.ControlBox = false;
      Activate();
    }

    private void btSettings_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_racursForm == null || _racursForm.IsDisposed)
      {
        _racursForm = new FrmCommonSettings();
        _racursForm.MdiParent = this;
      }
      else
      {
        _racursForm.Dispose();
        _racursForm = new FrmCommonSettings();
        _racursForm.MdiParent = this;
      }
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _racursForm.Icon = _mainIcon;
      _racursForm.Show();
      _racursForm.ControlBox = false;
      Activate();
    }

    private void btUsers_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_userForm == null || _userForm.IsDisposed)
      {
        _userForm = new FrmUsers();
        _userForm.MdiParent = this;
      }
      else
      {
        _userForm.Dispose();
        _userForm = new FrmUsers();
        _userForm.MdiParent = this;
      }
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _userForm.Icon = _mainIcon;
      _userForm.Show();
      _userForm.ControlBox = false;
      Activate();
    }

    private void barLargeButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_logSearchForm == null || _logSearchForm.IsDisposed)
      {
        _logSearchForm = new FrmLogSearch();
        _logSearchForm.MdiParent = this;
      }
      else
      {
        _logSearchForm.MdiParent = this;
        _logSearchForm.ReloadObjects();
      }
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _logSearchForm.Icon = _mainIcon;
      _logSearchForm.Show();
      _logSearchForm.ControlBox = false;
      Activate();
    }

    private void btQueryTemplates_ItemClick(object sender, ItemClickEventArgs e)
    {
      foreach (BarItem barItem in barManager1.Items)
      {
        try
        {
          ((BarBaseButtonItem) barItem).Down = false;
        }
        catch
        {
        }
      }
      (e.Item as BarLargeButtonItem).Down = true;
      if (_queryForm == null || _queryForm.IsDisposed)
      {
        _queryForm = new FrmQueryTemplates();
        _queryForm.MdiParent = this;
      }
      else
      {
        _queryForm.Dispose();
        _queryForm = new FrmQueryTemplates();
        _queryForm.MdiParent = this;
      }
      foreach (Form form in MdiChildren)
      {
        form.Hide();
        form.ControlBox = false;
      }
      _queryForm.Icon = _mainIcon;
      _queryForm.Show();
      _queryForm.ControlBox = false;
      Activate();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      components = new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      barManager1 = new BarManager(components);
      _mainFunctionsBar = new Bar();
      btFaces = new BarLargeButtonItem();
      btObjects = new BarLargeButtonItem();
      btCategories = new BarLargeButtonItem();
      btWorkStations = new BarLargeButtonItem();
      btSearch = new BarLargeButtonItem();
      btLogSearch = new BarLargeButtonItem();
      btResults = new BarLargeButtonItem();
      btStatistic = new BarLargeButtonItem();
      btSettings = new BarLargeButtonItem();
      btUsers = new BarLargeButtonItem();
      btQueryTemplates = new BarLargeButtonItem();
      barDockControlTop = new BarDockControl();
      barDockControlBottom = new BarDockControl();
      barDockControlLeft = new BarDockControl();
      barDockControlRight = new BarDockControl();
      repositoryItemImageEdit1 = new RepositoryItemImageEdit();
      repositoryItemButtonEdit1 = new RepositoryItemButtonEdit();
      repositoryItemTextEdit1 = new RepositoryItemTextEdit();
      repositoryItemDateEdit1 = new RepositoryItemDateEdit();
      bar1 = new Bar();
      bar3 = new Bar();
      barManager2 = new BarManager(components);
      barDockControl4 = new BarDockControl();
      barManager1.BeginInit();
      repositoryItemImageEdit1.BeginInit();
      repositoryItemButtonEdit1.BeginInit();
      repositoryItemTextEdit1.BeginInit();
      repositoryItemDateEdit1.BeginInit();
      repositoryItemDateEdit1.CalendarTimeProperties.BeginInit();
      barManager2.BeginInit();
      SuspendLayout();
      barManager1.AllowCustomization = false;
      barManager1.AllowMoveBarOnToolbar = false;
      barManager1.Bars.AddRange(new Bar[1]
      {
        _mainFunctionsBar
      });
      barManager1.CloseButtonAffectAllTabs = false;
      barManager1.DockControls.Add(barDockControlTop);
      barManager1.DockControls.Add(barDockControlBottom);
      barManager1.DockControls.Add(barDockControlLeft);
      barManager1.DockControls.Add(barDockControlRight);
      barManager1.Form = this;
      barManager1.Items.AddRange(new BarItem[11]
      {
        btFaces,
        btObjects,
        btCategories,
        btWorkStations,
        btSearch,
        btStatistic,
        btSettings,
        btResults,
        btUsers,
        btLogSearch,
        btQueryTemplates
      });
      barManager1.MainMenu = _mainFunctionsBar;
      barManager1.MaxItemId = 42;
      barManager1.RepositoryItems.AddRange(new RepositoryItem[4]
      {
        repositoryItemImageEdit1,
        repositoryItemButtonEdit1,
        repositoryItemTextEdit1,
        repositoryItemDateEdit1
      });
      barManager1.ShowFullMenus = true;
      barManager1.ShowScreenTipsInToolbars = false;
      barManager1.ShowShortcutInScreenTips = false;
      _mainFunctionsBar.BarItemHorzIndent = 5;
      _mainFunctionsBar.BarItemVertIndent = 5;
      _mainFunctionsBar.BarName = "Main menu";
      _mainFunctionsBar.CanDockStyle = BarCanDockStyle.Top;
      _mainFunctionsBar.DockCol = 0;
      _mainFunctionsBar.DockRow = 0;
      _mainFunctionsBar.DockStyle = BarDockStyle.Top;
      _mainFunctionsBar.LinksPersistInfo.AddRange(new LinkPersistInfo[11]
      {
        new LinkPersistInfo(btFaces),
        new LinkPersistInfo(btObjects),
        new LinkPersistInfo(BarLinkUserDefines.PaintStyle, btCategories, BarItemPaintStyle.CaptionGlyph),
        new LinkPersistInfo(BarLinkUserDefines.PaintStyle, btWorkStations, BarItemPaintStyle.CaptionGlyph),
        new LinkPersistInfo(BarLinkUserDefines.PaintStyle, btSearch, BarItemPaintStyle.CaptionGlyph),
        new LinkPersistInfo(btLogSearch),
        new LinkPersistInfo(btResults),
        new LinkPersistInfo(btStatistic),
        new LinkPersistInfo(btSettings),
        new LinkPersistInfo(btUsers),
        new LinkPersistInfo(btQueryTemplates)
      });
      _mainFunctionsBar.OptionsBar.AllowQuickCustomization = false;
      _mainFunctionsBar.OptionsBar.DisableCustomization = true;
      _mainFunctionsBar.OptionsBar.DrawDragBorder = false;
      _mainFunctionsBar.OptionsBar.RotateWhenVertical = false;
      _mainFunctionsBar.OptionsBar.UseWholeRow = true;
      componentResourceManager.ApplyResources(_mainFunctionsBar, "_mainFunctionsBar");
      btFaces.ActAsDropDown = true;
      btFaces.AllowAllUp = true;
      btFaces.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources(btFaces, "btFaces");
      btFaces.DropDownEnabled = false;
      btFaces.Enabled = false;
      btFaces.Glyph = Resources.Apps_system_users_icon48;
      btFaces.Id = 26;
      btFaces.MinSize = new Size(115, 80);
      btFaces.Name = "btFaces";
      btFaces.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btFaces.ItemClick += btFaces_ItemClick;
      btObjects.ActAsDropDown = true;
      btObjects.AllowAllUp = true;
      btObjects.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources(btObjects, "btObjects");
      btObjects.DropDownEnabled = false;
      btObjects.Enabled = false;
      btObjects.Glyph = Resources.Company_icon;
      btObjects.Id = 23;
      btObjects.MinSize = new Size(115, 80);
      btObjects.Name = "btObjects";
      btObjects.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btObjects.ItemClick += btObjects_ItemClick;
      btCategories.ActAsDropDown = true;
      btCategories.AllowAllUp = true;
      btCategories.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources(btCategories, "btCategories");
      btCategories.DropDownEnabled = false;
      btCategories.Enabled = false;
      btCategories.Glyph = Resources.Apps_preferences_desktop_user_password_icon64;
      btCategories.Id = 31;
      btCategories.MinSize = new Size(115, 80);
      btCategories.Name = "btCategories";
      btCategories.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btCategories.ItemClick += btCategories_ItemClick;
      btWorkStations.ActAsDropDown = true;
      btWorkStations.AllowAllUp = true;
      btWorkStations.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources(btWorkStations, "btWorkStations");
      btWorkStations.DropDownEnabled = false;
      btWorkStations.Enabled = false;
      btWorkStations.Glyph = Resources.Computer_2;
      btWorkStations.Id = 29;
      btWorkStations.MinSize = new Size(115, 80);
      btWorkStations.Name = "btWorkStations";
      btWorkStations.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btWorkStations.ItemClick += btWorkStations_ItemClick;
      btSearch.ActAsDropDown = true;
      btSearch.AllowAllUp = true;
      btSearch.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources(btSearch, "btSearch");
      btSearch.DropDownEnabled = false;
      btSearch.Enabled = false;
      btSearch.Glyph = Resources.Search64;
      btSearch.Id = 32;
      btSearch.ItemAppearance.Normal.Options.UseTextOptions = true;
      btSearch.ItemAppearance.Normal.TextOptions.WordWrap = WordWrap.Wrap;
      btSearch.MinSize = new Size(115, 80);
      btSearch.Name = "btSearch";
      btSearch.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btSearch.ItemClick += btSearch_ItemClick;
      btLogSearch.Border = BorderStyles.Default;
      btLogSearch.ButtonStyle = BarButtonStyle.Check;
      componentResourceManager.ApplyResources(btLogSearch, "btLogSearch");
      btLogSearch.Enabled = false;
      btLogSearch.Glyph = Resources.CamImage;
      btLogSearch.Id = 38;
      btLogSearch.MinSize = new Size(115, 80);
      btLogSearch.Name = "btLogSearch";
      btLogSearch.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btLogSearch.ItemClick += barLargeButtonItem1_ItemClick;
      btResults.ActAsDropDown = true;
      btResults.AllowAllUp = true;
      btResults.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources(btResults, "btResults");
      btResults.DropDownEnabled = false;
      btResults.Enabled = false;
      btResults.Glyph = Resources.Info64;
      btResults.Id = 28;
      btResults.MinSize = new Size(115, 80);
      btResults.Name = "btResults";
      btResults.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btResults.ItemClick += btResults_ItemClick;
      btStatistic.ActAsDropDown = true;
      btStatistic.AllowAllUp = true;
      btStatistic.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources(btStatistic, "btStatistic");
      btStatistic.DropDownEnabled = false;
      btStatistic.Enabled = false;
      btStatistic.Glyph = Resources.statistics64;
      btStatistic.Id = 2;
      btStatistic.MinSize = new Size(115, 80);
      btStatistic.Name = "btStatistic";
      btStatistic.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btStatistic.ItemClick += biJornal_ItemClick;
      btSettings.ActAsDropDown = true;
      btSettings.AllowAllUp = true;
      btSettings.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources(btSettings, "btSettings");
      btSettings.DropDownEnabled = false;
      btSettings.Enabled = false;
      btSettings.Glyph = Resources.ProfileFaces48;
      btSettings.Id = 0;
      btSettings.ImageIndex = 0;
      btSettings.ImageIndexDisabled = 0;
      btSettings.ItemAppearance.Normal.Options.UseTextOptions = true;
      btSettings.ItemAppearance.Normal.TextOptions.WordWrap = WordWrap.Wrap;
      btSettings.LargeImageIndex = 0;
      btSettings.LargeImageIndexDisabled = 0;
      btSettings.MinSize = new Size(115, 80);
      btSettings.Name = "btSettings";
      btSettings.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btSettings.Visibility = BarItemVisibility.Never;
      btSettings.ItemClick += btSettings_ItemClick;
      btUsers.ActAsDropDown = true;
      btUsers.AllowAllUp = true;
      btUsers.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources(btUsers, "btUsers");
      btUsers.DropDownEnabled = false;
      btUsers.Enabled = false;
      btUsers.Glyph = Resources.user_info64;
      btUsers.Id = 27;
      btUsers.MinSize = new Size(115, 80);
      btUsers.Name = "btUsers";
      btUsers.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btUsers.ItemClick += btUsers_ItemClick;
      btQueryTemplates.Border = BorderStyles.Default;
      btQueryTemplates.ButtonStyle = BarButtonStyle.Check;
      componentResourceManager.ApplyResources(btQueryTemplates, "btQueryTemplates");
      btQueryTemplates.DropDownEnabled = false;
      btQueryTemplates.Enabled = false;
      btQueryTemplates.Glyph = Resources.SQL64;
      btQueryTemplates.Id = 39;
      btQueryTemplates.MinSize = new Size(100, 0);
      btQueryTemplates.Name = "btQueryTemplates";
      btQueryTemplates.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      btQueryTemplates.Visibility = BarItemVisibility.Never;
      btQueryTemplates.ItemClick += btQueryTemplates_ItemClick;
      barDockControlTop.CausesValidation = false;
      componentResourceManager.ApplyResources(barDockControlTop, "barDockControlTop");
      barDockControlBottom.CausesValidation = false;
      componentResourceManager.ApplyResources(barDockControlBottom, "barDockControlBottom");
      barDockControlLeft.CausesValidation = false;
      componentResourceManager.ApplyResources(barDockControlLeft, "barDockControlLeft");
      barDockControlRight.CausesValidation = false;
      componentResourceManager.ApplyResources(barDockControlRight, "barDockControlRight");
      componentResourceManager.ApplyResources(repositoryItemImageEdit1, "repositoryItemImageEdit1");
      repositoryItemImageEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("repositoryItemImageEdit1.Buttons"))
      });
      repositoryItemImageEdit1.Name = "repositoryItemImageEdit1";
      componentResourceManager.ApplyResources(repositoryItemButtonEdit1, "repositoryItemButtonEdit1");
      repositoryItemButtonEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
      componentResourceManager.ApplyResources(repositoryItemTextEdit1, "repositoryItemTextEdit1");
      repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
      componentResourceManager.ApplyResources(repositoryItemDateEdit1, "repositoryItemDateEdit1");
      repositoryItemDateEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("repositoryItemDateEdit1.Buttons"))
      });
      repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
      bar1.BarAppearance.Normal.Font = (Font) componentResourceManager.GetObject("bar1.BarAppearance.Normal.Font");
      bar1.BarAppearance.Normal.Options.UseFont = true;
      bar1.BarName = "Main menu";
      bar1.DockCol = 0;
      bar1.DockRow = 0;
      bar1.DockStyle = BarDockStyle.Top;
      bar1.LinksPersistInfo.AddRange(new LinkPersistInfo[2]
      {
        new LinkPersistInfo(btSettings),
        new LinkPersistInfo(btStatistic)
      });
      bar1.OptionsBar.UseWholeRow = true;
      componentResourceManager.ApplyResources(bar1, "bar1");
      bar3.BarAppearance.Normal.Font = (Font) componentResourceManager.GetObject("bar3.BarAppearance.Normal.Font");
      bar3.BarAppearance.Normal.Options.UseFont = true;
      bar3.BarName = "Main menu";
      bar3.DockCol = 0;
      bar3.DockRow = 0;
      bar3.DockStyle = BarDockStyle.Top;
      bar3.OptionsBar.UseWholeRow = true;
      componentResourceManager.ApplyResources(bar3, "bar3");
      barManager2.AllowCustomization = false;
      barManager2.AllowMoveBarOnToolbar = false;
      barManager2.AllowShowToolbarsPopup = false;
      barManager2.CloseButtonAffectAllTabs = false;
      barManager2.DockingEnabled = false;
      barManager2.Form = this;
      barManager2.MaxItemId = 0;
      barManager2.ShowCloseButton = true;
      barManager2.ShowFullMenus = true;
      barManager2.ShowFullMenusAfterDelay = false;
      barDockControl4.CausesValidation = false;
      componentResourceManager.ApplyResources(barDockControl4, "barDockControl4");
      Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(barDockControlLeft);
      Controls.Add(barDockControlRight);
      Controls.Add(barDockControlBottom);
      Controls.Add(barDockControlTop);
      IsMdiContainer = true;
      KeyPreview = true;
      Name = "MainForm";
      SizeGripStyle = SizeGripStyle.Hide;
      WindowState = FormWindowState.Maximized;
      FormClosing += EmplooyersForm_FormClosing;
      Load += EmplooyersForm_Load;
      barManager1.EndInit();
      repositoryItemImageEdit1.EndInit();
      repositoryItemButtonEdit1.EndInit();
      repositoryItemTextEdit1.EndInit();
      repositoryItemDateEdit1.CalendarTimeProperties.EndInit();
      repositoryItemDateEdit1.EndInit();
      barManager2.EndInit();
      ResumeLayout(false);
    }
  }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookSync.Properties;
using BookSync.Classes.FTP;
using BookSync.Classes.Drive;
using BookSync.Classes.Algemeen;
using System.IO;
using BookSync.Classes.Data.DB;

namespace BookSync
{
    public partial class FormMain : Form
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private Settings m_Settings;

        private DriveSync m_DriveSync;

        private LiteDB m_LiteDBOud;
        private LiteDB m_LiteDBNieuw;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            m_Settings = new Settings();
            m_Settings.Reload();

            Globals.TextBox = rtbLog;

            if (!string.IsNullOrEmpty(m_Settings.PlacesFile))
            {
                txtFile.Text = m_Settings.PlacesFile;
            }

            if (!File.Exists(txtFile.Text))
            {
                btnOpenFile_Click(null, new EventArgs());
            }

            if (File.Exists(txtFile.Text))
            {
                if (!DeleteIfExists(Globals.PlacesFileNieuw))
                {
                    MessageBox.Show(string.Format("Fout bij verwijderen bestand {0}", Globals.PlacesFileNieuw));
                }
                else
                {
                    File.Copy(txtFile.Text, Globals.PlacesFileNieuw);
                }
            }

            if (!File.Exists(Globals.PlacesFileOud))
            {
                File.Copy(Globals.PlacesFileNieuw, Globals.PlacesFileOud); 
            }
        }

        private bool DeleteIfExists(string BestandsNaam)
        {
            bool Succes = false;

            try
            {
                if (System.IO.File.Exists(BestandsNaam)) System.IO.File.Delete(BestandsNaam);

                Succes = true;

            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "FormMain.cs", "DeleteIfExists", ex);
            }

            return Succes;
        }
        #endregion Constructor, Load & Closing

        #region Button Events
        /********************************************** Button Events ******************************************************************/

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (ofdDialog.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = ofdDialog.FileName;
                m_Settings.PlacesFile = txtFile.Text;
                m_Settings.Save();
            }
        }

        private void btnOpenDBOud_Click(object sender, EventArgs e)
        {
            OpenDBOud();
        }

        private void btnOpenDBNieuw_Click(object sender, EventArgs e)
        {
            OpenDBNieuw();
        }

        #endregion Button Events



        #region Control Events
        /********************************************** Control Events ******************************************************************/


        #endregion Control Events

        #region Functions
        /********************************************** Functions ******************************************************************/

        private void ConnectGoogle()
        {
            m_DriveSync.Connect();
        }

        private void StartVoortgang()
        {
            pbVoortgang.Value = 0;

            Application.DoEvents();
        }

        private void StopVoortgang()
        {
            pbVoortgang.Value = 100;

            Application.DoEvents();
        }

        private void Voortgang(string Status)
        {
            lblVoortgang.Text = Status;

            Application.DoEvents();
        }

        private void OpenDBOud()
        {
            m_LiteDBOud = new LiteDB(Globals.PlacesFileOud);
        }

        private void OpenDBNieuw()
        {
            m_LiteDBNieuw = new LiteDB(Globals.PlacesFileNieuw);
        }

        #endregion Functions

        #region Events
        /********************************************** Events ******************************************************************/

        private void MyDriveProgressEvent(DriveProgressEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateDriveProgress(MyDriveProgressEvent), e);
            }
            else
            {
                try
                {
                    pbVoortgang.Value = (int)Math.Round(e.Percentage);
                    Application.DoEvents();
                }
                catch { }
            }
        }

        private void MyDriveFinishedEvent(DriveFinishedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateDriveFinished(MyDriveFinishedEvent), e);
            }
            else
            {
                try
                {
                    StopVoortgang();
                    if (e.Succes)
                    {
                        Voortgang("Gereed");
                    }
                    else
                    {
                        Voortgang("Mislukt");
                    }
                }
                catch (Exception ex)
                {
                    ErrorDump.AddError(System.IntPtr.Zero, "FormMain.cs", "MyActionFinishedEvent", ex);
                }
            }
        }

        private void MyDriveConnectEvent(DriveConnectEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateDriveConnect(MyDriveConnectEvent), e);
            }
            else
            {
                try
                {
                    if (e.Connected)
                    {
                        lblDriveStatus.Text = "Verbonden";
                    }
                    else
                    {
                        lblDriveStatus.Text = "Niet Verbonden";
                    }

                    Application.DoEvents();
                }
                catch { }
            }
        }

        private void MyDriveLoadedEvent(DriveLoadedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateDriveLoaded(MyDriveLoadedEvent), e);
            }
            else
            {
                try
                {

                    Application.DoEvents();
                }
                catch { }
            }
        }

        #endregion Events

        private void btnConnect_Click(object sender, EventArgs e)
        {
            m_DriveSync = new DriveSync();

            m_DriveSync.ProgressEvent += MyDriveProgressEvent;
            m_DriveSync.FinishedEvent += MyDriveFinishedEvent;
            m_DriveSync.ConnectEvent += MyDriveConnectEvent;
            m_DriveSync.LoadedEvent += MyDriveLoadedEvent;

            ConnectGoogle();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            Voortgang("Downloaden");

            StartVoortgang();

            Application.DoEvents();

            m_DriveSync.DownloadPlaces();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            Voortgang("Uploaden");

            StartVoortgang();

            Application.DoEvents();

            m_DriveSync.UploadPlaces();
        }






        #region Properties
        /********************************************** Properties *****************************************************************/


        #endregion Properties

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Discovery;
using Google.Apis;
using BookSync.Classes.Drive.Authentication;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using BookSync.Classes.Drive.IO;
using BookSync.Classes.Algemeen;

namespace BookSync.Classes.Drive
{
    public class DriveSync
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private DriveService m_Drive;

        private Google.Apis.Drive.v2.Data.File m_PlacesBestand;
        private Google.Apis.Drive.v2.Data.File m_PlacesFolder;

        private bool m_Connected = false;

        private GDownload m_GDownload;
        private GUpload m_GUpload;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public DriveSync()
        {
            m_Drive = new DriveService(GAuth.CreateAuthenticator());
        }

        #endregion Constructor, Load & Closing

        #region Functions
        /********************************************** Functions ******************************************************************/

        private void StartConnect()
        {
            try
            {
                m_Connected = (GetPlacesFolder() && GetPlacesBestand());

                m_GDownload = new GDownload(m_Drive.Authenticator);
                m_GDownload.ProgressEvent = ProgressEvent;
                m_GDownload.FinishedEvent = FinishedEvent;

                m_GUpload = new GUpload(m_Drive, m_PlacesFolder);
                m_GUpload.ProgressEvent = ProgressEvent;
                m_GUpload.FinishedEvent = FinishedEvent;

            }
            catch (Exception ex)
            {
                m_Connected = false;
                ErrorDump.AddError(System.IntPtr.Zero, "DriveSync.cs", "Connect", ex);
            }

            if (ConnectEvent != null) ConnectEvent(new DriveConnectEventArgs(m_Connected));
        }

        public void Connect()
        {
            System.Threading.Thread myThread = new System.Threading.Thread(StartConnect);
            myThread.Start();
        }

        private bool GetPlacesFolder()
        {
            bool Succes = false;

            try
            {
                FilesResource.ListRequest myList = m_Drive.Files.List();
                myList.Q = "title = 'BookSync' and mimeType = 'application/vnd.google-apps.folder' and trashed = false";
                Google.Apis.Drive.v2.Data.FileList myFiles = myList.Fetch();

                if (myFiles.Items.Count > 0)
                {
                    m_PlacesFolder = myFiles.Items[0];

                    Succes = true;
                }
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "DriveSync.cs", "GetPlacesFolder", ex);
            }

            return Succes;
        }

        private bool GetPlacesBestand()
        {
            bool Succes = false;

            try
            {
                FilesResource.ListRequest myList = m_Drive.Files.List();
                myList.Q = string.Format("title = 'places.sqlite' and mimeType = 'application/octet-stream' and trashed = false and '{0}' in parents", m_PlacesFolder.Id);
                Google.Apis.Drive.v2.Data.FileList myFiles = myList.Fetch();

                if (myFiles.Items.Count > 0)
                {
                    m_PlacesBestand = myFiles.Items[0];

                    Succes = true;
                }

            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "DriveSync.cs", "GetPlacesBestand", ex);
            }

            return Succes;
        }

        public bool UploadPlaces()
        {
            bool Succes = false;

            try
            {
                if (DeleteFilesInFolder(m_PlacesFolder))
                {
                    m_GUpload.ProgressEvent = ProgressEvent;
                    m_GUpload.FinishedEvent = FinishedEvent;

                    m_GUpload.UploadFile(Globals.PlacesFileNieuw, m_PlacesFolder);

                    Succes = true;
                }
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "DriveSync.cs", "UploadPlaces", ex);
                if (FinishedEvent != null) FinishedEvent(new DriveFinishedEventArgs(false, "Fout bij uploaden", ex));
            }

            return Succes;
        }

        public bool DownloadPlaces()
        {
            bool Succes = false;

            try
            {
                m_GDownload.ProgressEvent = ProgressEvent;
                m_GDownload.FinishedEvent = FinishedEvent;

                if (DeleteIfExists(Globals.PlacesFileNieuw))
                {
                    m_GDownload.DownloadFile(m_PlacesBestand, Globals.PlacesFileNieuw);

                    Succes = true;
                }
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "DriveSync.cs", "DownloadPlaces", ex);
                if (FinishedEvent != null) FinishedEvent(new DriveFinishedEventArgs(false, "Fout bij downloaden", ex));
            }

            return Succes;
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
                ErrorDump.AddError(System.IntPtr.Zero, "DriveSync.cs", "DeleteIfExists", ex);
                if (FinishedEvent != null) FinishedEvent(new DriveFinishedEventArgs(false, "Fout bij verwijderen oude database", ex));
            }

            return Succes;
        }

        private bool DeleteFilesInFolder(Google.Apis.Drive.v2.Data.File Folder)
        {
            bool Succes = false;

            try
            {
                FilesResource.ListRequest myList = m_Drive.Files.List();
                myList.Q = string.Format("trashed = false and '{0}' in parents", Folder.Id);
                Google.Apis.Drive.v2.Data.FileList myFiles = myList.Fetch();

                foreach (Google.Apis.Drive.v2.Data.File myFile in myFiles.Items)
                {
                    FilesResource.DeleteRequest myDelRequest = m_Drive.Files.Delete(myFile.Id);
                    myDelRequest.Fetch();
                }

                Succes = true;
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "DriveSync.cs", "DeleteFilesInFolder", ex);
                if (FinishedEvent != null) FinishedEvent(new DriveFinishedEventArgs(false, "Fout bij verwijderen oude database", ex));
            }

            return Succes;
        }

        #endregion Functions


        #region Properties
        /********************************************** Properties *****************************************************************/

        public bool Connected
        {
            get { return this.m_Connected; }
        }

        public DriveService Drive
        {
            get { return this.m_Drive; }
        }

        public Google.Apis.Drive.v2.Data.File PlacesBestand
        {
            get { return this.m_PlacesBestand; }
        }

        public Google.Apis.Drive.v2.Data.File PlacesFolder
        {
            get { return this.m_PlacesFolder; }
        }

        public DelegateDriveProgress ProgressEvent;
        public DelegateDriveFinished FinishedEvent;
        public DelegateDriveConnect ConnectEvent;
        public DelegateDriveLoaded LoadedEvent;

        #endregion Properties

    }

    #region EventClasses
    /********************************************** EventClasses **************************************************/

    public class DriveProgressEventArgs : EventArgs
    {
        private double percentage;

        public DriveProgressEventArgs(double percentage)
        {
            this.percentage = percentage;
        }

        public double Percentage
        {
            get { return percentage; }
        }
    }

    public class DriveFinishedEventArgs : EventArgs
    {
        private bool succes;
        private string message;
        private Exception innerException;

        public DriveFinishedEventArgs(bool succes)
        {
            this.succes = succes;
            this.message = "";
            this.innerException = null;
        }

        public DriveFinishedEventArgs(bool succes, string message)
        {
            this.succes = succes;
            this.message = message;
            this.innerException = null;
        }

        public DriveFinishedEventArgs(bool succes, Exception e)
        {
            this.succes = succes;
            this.message = "";
            this.innerException = e;
        }

        public DriveFinishedEventArgs(bool succes, string message, Exception e)
        {
            this.succes = succes;
            this.message = message;
            this.innerException = e;
        }

        public bool Succes
        {
            get { return succes; }
        }

        public string Message
        {
            get { return message; }
        }

        public Exception InnerException
        {
            get { return innerException; }
        }
    }

    public class DriveConnectEventArgs : EventArgs
    {
        private bool connected;

        public DriveConnectEventArgs(bool connected)
        {
            this.connected = connected;
        }

        public bool Connected
        {
            get { return connected; }
        }
    }

    public class DriveLoadedEventArgs : EventArgs
    {
        private bool loaded;

        public DriveLoadedEventArgs(bool loaded)
        {
            this.loaded = loaded;
        }

        public bool Loaded
        {
            get { return loaded; }
        }
    }

    #endregion EventClasses

    #region Delegates
    /********************************************** Delegates **************************************************/

    public delegate void DelegateDriveProgress(DriveProgressEventArgs e);
    public delegate void DelegateDriveFinished(DriveFinishedEventArgs e);
    public delegate void DelegateDriveConnect(DriveConnectEventArgs e);
    public delegate void DelegateDriveLoaded(DriveLoadedEventArgs e);

    #endregion Delegates
}

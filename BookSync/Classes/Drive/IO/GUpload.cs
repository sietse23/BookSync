using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Discovery;
using Google.Apis;
using Google.Apis.Authentication;
using Google.Apis.Requests;
using Google.Apis.Util;
using Google.Apis.Upload;
using BookSync.Classes.Drive.Authentication;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using BookSync.Classes.Algemeen;

namespace BookSync.Classes.Drive.IO
{
    public class GUpload
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private long m_TotalBytesSize;

        private DriveService m_Drive;

        private Google.Apis.Drive.v2.Data.File m_RHSFolder;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public GUpload(DriveService Drive, Google.Apis.Drive.v2.Data.File RHSFolder)
        {
            m_Drive = Drive;
            m_RHSFolder = RHSFolder;
        }

        #endregion Constructor, Load & Closing

        #region Functions
        /********************************************** Functions ******************************************************************/

        public void UploadFile(string FileName, Google.Apis.Drive.v2.Data.File Folder)
        {
            Google.Apis.Drive.v2.Data.File myFile = new Google.Apis.Drive.v2.Data.File();

            try
            {
                myFile.Title = "places.sqlite";
                myFile.MimeType = "chemical/x-mdl-sdfile";

                myFile.Parents = new List<ParentReference>() { new ParentReference() { Id = Folder.Id } };

                byte[] myByteArray = System.IO.File.ReadAllBytes(FileName);
                MemoryStream myStream = new MemoryStream(myByteArray);
                myStream.Position = 0;

                FilesResource.InsertMediaUpload myRequest = m_Drive.Files.Insert(myFile, myStream, myFile.MimeType);
                myRequest.ChunkSize = 262144;

                myRequest.ProgressChanged += new Action<Google.Apis.Upload.IUploadProgress>(myProgressChanged);

                m_TotalBytesSize = myStream.Length;

                Action myAsyncAction = () =>
                {
                    myRequest.Upload();
                };

                System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(myAsyncAction));
                myThread.Start();
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "GUpload.cs", "UploadFile", ex);
                if (FinishedEvent != null) FinishedEvent(new DriveFinishedEventArgs(false, "Fout bij uploaden", ex));                
            }
        }

        #endregion Functions


        #region Events
        /********************************************** Events *****************************************************************/


        public void myProgressChanged(Google.Apis.Upload.IUploadProgress myProgress)
        {
            try
            {
                if (myProgress.Status == Google.Apis.Upload.UploadStatus.Uploading)
                {
                    double myProcent = myProgress.BytesSent / (m_TotalBytesSize / 100);

                    if (ProgressEvent != null) ProgressEvent(new DriveProgressEventArgs(myProcent));
                }
                else if (myProgress.Status == Google.Apis.Upload.UploadStatus.Completed)
                {
                    if (FinishedEvent != null) FinishedEvent(new DriveFinishedEventArgs(true));
                }
                else if (myProgress.Status == Google.Apis.Upload.UploadStatus.Failed)
                {
                    if (FinishedEvent != null) FinishedEvent(new DriveFinishedEventArgs(false));
                }

            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "GUpload.cs", "myProgressChanged", ex);
            }
        }

        #endregion Events

        #region Properties
        /********************************************** Properties *****************************************************************/

        public DelegateDriveProgress ProgressEvent;
        public DelegateDriveFinished FinishedEvent;

        #endregion Properties




    }

}

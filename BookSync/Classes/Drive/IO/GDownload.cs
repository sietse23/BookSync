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
using BookSync.Classes.Drive.Authentication;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using BookSync.Classes.Drive;
using BookSync.Classes.Algemeen;

namespace BookSync.Classes.Drive.IO
{
    public class GDownload
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private IAuthenticator m_Authenticator;

        private const int BUFFER_SIZE = 1448;
        private GRequestState m_State;
        private Google.Apis.Drive.v2.Data.File m_File;
        private string m_BestandsNaam;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public GDownload(IAuthenticator Authenticator)
        {
            m_Authenticator = Authenticator;
        }

        #endregion Constructor, Load & Closing

        #region Functions
        /********************************************** Functions ******************************************************************/

        public Stream DownloadFileSimple(Google.Apis.Drive.v2.Data.File Bestand)
        {
            Stream myFile = null;

            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(new Uri(Bestand.DownloadUrl));
                m_Authenticator.ApplyAuthenticationToRequest(myRequest);

                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                myFile = myResponse.GetResponseStream();
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "GDownload.cs", "DownloadBestand", ex);
                if (FinishedEvent != null) FinishedEvent(new DriveFinishedEventArgs(false, "Fout bij downloaden", ex));

            }

            return myFile;
        }

        public bool DownloadFile(Google.Apis.Drive.v2.Data.File myFile, string BestandsNaam)
        {
            bool Succes = false;
            m_File = myFile;
            m_BestandsNaam = BestandsNaam;

            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(new Uri(myFile.DownloadUrl));
                m_Authenticator.ApplyAuthenticationToRequest(myRequest);

                m_State = new GRequestState(BUFFER_SIZE);
                m_State.Request = myRequest;
                m_State.WriteStream = System.IO.File.OpenWrite(BestandsNaam);

                m_State.ProgressEvent += ProgressEvent;
                m_State.FinishedEvent += FinishedEvent;

                IAsyncResult myResult = (IAsyncResult)myRequest.BeginGetResponse(new AsyncCallback(RespCallback), m_State);
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "GDownload.cs", "DownloadFile", ex);
                if (FinishedEvent != null) FinishedEvent(new DriveFinishedEventArgs(false, "Fout bij downloaden", ex));

            }

            return Succes;
        }

        private static void RespCallback(IAsyncResult AsyncResult)
        {
            GRequestState myState = ((GRequestState)(AsyncResult.AsyncState));

            try
            {
                WebRequest myRequest = myState.Request;
                string myDescription = "";
                string myContentLength = "";

                HttpWebResponse myResponse = ((HttpWebResponse)(myRequest.EndGetResponse(AsyncResult)));
                myState.Response = myResponse;
                myDescription = myResponse.StatusDescription;
                myState.TotalBytes = myState.Response.ContentLength;
                myContentLength = myState.Response.ContentLength.ToString();

                Stream myResponseStream = myState.Response.GetResponseStream();
                myState.StreamResponse = myResponseStream;

                IAsyncResult myIAsyncResult = myResponseStream.BeginRead(myState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), myState);

                return;
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "GDownload.cs", "RespCallback", ex);
                if (myState.FinishedEvent != null) myState.FinishedEvent(new DriveFinishedEventArgs(false, "Fout bij downloaden", ex));

            }
        }

        private static void ReadCallback(IAsyncResult AsyncResult)
        {
            GRequestState myState = ((GRequestState)(AsyncResult.AsyncState));

            try
            {
                Stream myResponseStream = myState.StreamResponse;
                
                int myBytesRead = myResponseStream.EndRead(AsyncResult);

                myState.WriteStream.Write(myState.BufferRead, 0, myBytesRead);

                if (myBytesRead > 0)
                {
                    myState.BytesRead += myBytesRead;
                    double myPctComplete = ((double)myState.BytesRead / (double)myState.TotalBytes) * 100.0f;

                    if (myState.ProgressEvent != null) myState.ProgressEvent(new DriveProgressEventArgs(myPctComplete));

                    IAsyncResult myIAsyncResult = myResponseStream.BeginRead(myState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), myState);
                    return;
                }
                else
                {
                    myState.WriteStream.Close();
                    myState.StreamResponse.Close();
                    myState.Response.Close();

                    if (myState.FinishedEvent != null) myState.FinishedEvent(new DriveFinishedEventArgs(true));
                }
            }
            catch (Exception ex)
            {
                ErrorDump.AddError(System.IntPtr.Zero, "GDownload.cs", "ReadCallback", ex);
                if (myState.FinishedEvent != null) myState.FinishedEvent(new DriveFinishedEventArgs(false, "Fout bij downloaden", ex));

            }
        }

        #endregion Functions

        #region Properties
        /********************************************** Properties *****************************************************************/

        public DelegateDriveProgress ProgressEvent;
        public DelegateDriveFinished FinishedEvent;

        #endregion Properties



    }




}

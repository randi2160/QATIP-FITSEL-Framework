using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using fit;
using System.IO;
using System.Xml.Linq;
using QaTip.Fitnesse.Demo.DoFixture;

namespace QaTip.Fitnesse.Demo.DoFixture
{
    public class WebServicesFixture:ColumnFixture
    {
        public WebServicesFixture()
        {

        }
        private string _requestType;
        private string _url;
        private string _requestcontent;
        private string _requestFormat;
        private string _Requestresponse;

        public string requestStatus
        {
            get;
            set;
        }
        public string StatusDescriptions
        {
            get;
            set;
        }

        public string Requestresponse
        {

            get { return _Requestresponse; }
            set { _Requestresponse = value; }
        }

        public string RequestType
        {

            get { return _requestType; }
            set { _requestType = value; }
        }
        public string requestFormat
        {

            get { return _requestFormat; }
            set { _requestFormat = value; }
        }
        public string Url
        {

            get;set;
        }

        public string RequestBody
        {
            get { return _requestcontent; }
            set { _requestcontent = value; }
        }





        //Post Json 

        public string PostJson()
        {
          
            string result = "";
            string reqstatus = "";
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = client.UploadString(Url, _requestType, _requestFormat);
                GetStatusCode(client, out reqstatus);
                requestStatus = reqstatus;
            }
            Requestresponse = result;
            Console.WriteLine(result);
            return result;
        }




        public enum HttpStatusCode
        {

            Moved = 301,
            OK = 200,
            Redirect = 302,

        }

        //Post xml request

            public void postxmlrequest()
        {
            QaTipWebRequest req = new QaTipWebRequest(Url,"POST");
            
            
        }
        
        // Get request for xml
        public string GetRequest()
        {
           
            QaTipWebRequest req = new QaTipWebRequest(Url, "GET");
            var reqStatus = req.GetResponse();
            Requestresponse = req.Requestresponse;
            requestStatus = req.Status;

            return reqStatus;
        }


        private string Get(string url)
        {
            using (var client = new WebClient())
            {
                var responseString = client.DownloadString(url);
                requestStatus = client.ResponseHeaders.ToString();
                return responseString;
            }
        }


        private static int GetStatusCode(WebClient client, out string statusDescription)
        {
            FieldInfo responseField = client.GetType().GetField("m_WebResponse", BindingFlags.Instance | BindingFlags.NonPublic);

            if (responseField != null)
            {
                HttpWebResponse response = responseField.GetValue(client) as HttpWebResponse;

                if (response != null)
                {
                    statusDescription = response.StatusDescription;

                    return (int)response.StatusCode;
                }
            }

            statusDescription = null;
            return 0;
        }

    }
}

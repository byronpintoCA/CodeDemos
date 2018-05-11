using System;
using System.IO;
using System.Web;

namespace MiscHelper
{
    public class IISReqRespLog : Stream
    {
        public String QueryString { get; private set; }
        public string InputData { get; internal set; }
        public string OutputData { get { return ReadOutputStream(); } }


        private readonly Stream InnerStream;
        private readonly MemoryStream CopyStream;

        public IISReqRespLog(Stream inner)
        {
            this.InnerStream = inner;
            this.CopyStream = new MemoryStream();
        }

        public string ReadOutputStream()
        {
            lock (this.InnerStream)
            {
                if (this.CopyStream.Length <= 0L ||
                    !this.CopyStream.CanRead ||
                    !this.CopyStream.CanSeek)
                {
                    return String.Empty;
                }

                long pos = this.CopyStream.Position;
                this.CopyStream.Position = 0L;
                try
                {
                    return new StreamReader(this.CopyStream).ReadToEnd();
                }
                finally
                {
                    try
                    {
                        this.CopyStream.Position = pos;
                    }
                    catch { }
                }
            }
        }


        public override bool CanRead
        {
            get { return this.InnerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.InnerStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.InnerStream.CanWrite; }
        }

        public override void Flush()
        {
            this.InnerStream.Flush();
        }

        public override long Length
        {
            get { return this.InnerStream.Length; }
        }

        public override long Position
        {
            get { return this.InnerStream.Position; }
            set { this.CopyStream.Position = this.InnerStream.Position = value; }
        }

        public string Headers { get; private set; }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.InnerStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            this.CopyStream.Seek(offset, origin);
            return this.InnerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.CopyStream.SetLength(value);
            this.InnerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.CopyStream.Write(buffer, offset, count);
            this.InnerStream.Write(buffer, offset, count);
        }

        public void ReadInputData(HttpRequest request)
        {

            try
            {
                request.InputStream.Position = 0;

                if (request.InputStream.Length > 0)
                {
                    byte[] rawBytes = new byte[request.InputStream.Length];
                    request.InputStream.Read(rawBytes, 0, rawBytes.Length);
                    request.InputStream.Position = 0;

                    string logData = $"{DateTime.Now.ToString()} {request.Path} : {request.QueryString} : {System.Text.Encoding.Default.GetString(rawBytes)} {Environment.NewLine}";

                    QueryString = request.QueryString.ToString();
                    InputData = System.Text.Encoding.Default.GetString(rawBytes);

                    var allKeys = request.Headers.AllKeys;

                    Headers = "";

                    for (int i = 0; i < allKeys.Length; i++)
                    {
                        String record = $"{allKeys[i]} : {request.Headers[i]} {Environment.NewLine}";
                        Headers += record;
                    }

                }
            }
            catch { }

        }


    }

    /// Usage
    /// 
    //protected void Application_BeginRequest(object sender, EventArgs e)
    //{
    //    RegisterRequest();
    //}

    //protected void Application_EndRequest(object sender, EventArgs e)
    //{
    //    LogResponse((HttpApplication)sender);
    //}

    //private void LogResponse(HttpApplication sender)
    //{
    //    var filter = (OutputFilterStream)HttpContext.Current.Items["RequestResponse"];

    //    String logData = $"{DateTime.Now} - Request : {filter.InputData} {Environment.NewLine} Response : {filter.OutputData} {Environment.NewLine}";

    //    string filePath = $@"{HttpRuntime.AppDomainAppPath}App_Data\RequestResponse.log";
    //    File.AppendAllText(filePath, logData);

    //}

    //private static void RegisterRequest()
    //{
    //    HttpResponse response = HttpContext.Current.Response;
    //    OutputFilterStream filter = new OutputFilterStream(response.Filter);
    //    response.Filter = filter;
    //    filter.ReadInputData(HttpContext.Current.Request);
    //    HttpContext.Current.Items.Add("RequestResponse", filter);
    //}
}

using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.RequestMethods
{
    public class GetMethod: IRequestMethod
    {
        APIs api;
        public GetMethod()
        {   }
        public void setAPI(APIs api)
        {
            this.api = api;
        }
        public async Task<string> GetResultAsync()
        {
            return api.Link;
        }
            //static Stream GenerateStreamFromString(string s)
            //{
            //    MemoryStream stream = new MemoryStream();
            //    StreamWriter writer = new StreamWriter(stream);
            //    writer.Write(s);
            //    writer.Flush();
            //    stream.Position = 0;
            //    return stream;
            //}
        }
}

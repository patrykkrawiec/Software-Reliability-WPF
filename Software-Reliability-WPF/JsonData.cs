using Newtonsoft.Json.Linq;
using Software_Reliability;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Software_Reliability_WPF
{
    class JsonData
    {
        private ModelInput input;

        public JsonData()
        {
            input = new ModelInput();
        }
        public int ReadFile(string path)
        {
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    JArray a = (JArray)JObject.Parse(json)["timeValues"];
                    IList<int> list = a.ToObject<IList<int>>();
                    input.times = (List<int>)list;
                    return 0;
                }
            }
            catch(Exception ex)
            {
                return -1;
            }
          
            
        }
        public ModelInput getInputFile()
        {
            return input;
        }
        

    }
}

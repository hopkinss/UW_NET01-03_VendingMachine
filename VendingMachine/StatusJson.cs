using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace VendingMachine
{
    public class StatusJson
    {
        private bool isSuccess;
        private string msg;
        private List<string> refund;

        public StatusJson()
        {
            this.refund = new List<string>();
        }

        public List<string> Refund
        {
            get { return refund; }
            set { refund = value; }
        }

        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }

        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        public string WriteJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

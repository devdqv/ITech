using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITech.Models
{
    public class objMessage
    {
        public objMessage() {

        }
        public objMessage(bool Error, string Message="", object data=null) {
            this.Error = Error;
            this.Message = Message;
            this.Data = data;
        }
       
        public bool Error { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCore.Enum;

namespace FCore.Class
{
    [Serializable]
    public class ErrorInfo : ClassInfo
    {
        public bool Ok { get; set; }
        public string QueryName { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string InnerMessage { get;set; }
        public DateTime Date { get; set; }
        public ErrorType ErrorType { get; set; }
    }
}

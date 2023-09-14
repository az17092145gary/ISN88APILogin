using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class jwttoken
    {
        int? responseCode { get; set; }
        string? responseMessage { get; set; }
        string? success { get; set; }
        string? token { get; set; }
        bool hasPopupAnnouncements { get; set; }
    }
}

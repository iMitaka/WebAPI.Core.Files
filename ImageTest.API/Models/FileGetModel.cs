using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageTest.API.Models
{
    public class FileGetModel
    {
        public FileStream File { get; set; }
        public string FileType { get; set; }
    }
}

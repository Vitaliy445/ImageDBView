using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp7.Models
{
    class ImageMinData
    {
        public int ID { get; set; }
        public string Lable { get; set; }
        public ImageMinData(int id, string lable)
        {
            ID = id;
            Lable = lable;
        }
        public override string ToString()
        {
            return ID.ToString() + " " + Lable;
        }
    }
}



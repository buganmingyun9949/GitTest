using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class ClassInfo
    {
        public string Token { get; set; }

        public string Class_id { get; set; }

        public string Class_name { get; set; }

        public string Class_status { get; set; }

        public string Teacher_id { get; set; }
    }

    public class GradeInfo
    {
        public int Grade_Id { get; set; }

        public string Grade_Name { get; set; }
    }
}

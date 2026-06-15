using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ApiResponse<T>
    {
        public string? Code { get; set; }
        public bool Status { get; set; } = true;
        public string? Messages { get; set; }
        public List<T>? Data { get; set; }
        public int? Total { get; set; }
    }
}
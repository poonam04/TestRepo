using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO
{
    public class APIResponse<T>
    {
        public bool Succeeded { get; set; } = false;
        public int ErrorCode { get; set; } = 0;
        public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;
        public T Data { get; set; }
    }
}

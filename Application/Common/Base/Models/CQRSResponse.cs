using System.Net;

namespace Application.Common.Base.Models
{
    public class CQRSResponse<T> : CQRSResponse
    {
        public CQRSResponse(T response, HttpStatusCode status = HttpStatusCode.OK, int statusCode = (int)HttpStatusCode.OK, string message = default) : base(status, statusCode, message)
        {
            Response = response;
            Status = status;
            StatusCode = status == HttpStatusCode.OK ? statusCode : (int)status;
            Message = message;
        }

        public T Response { get; set; }
    }

    public class CQRSResponse
    {
        public CQRSResponse(HttpStatusCode status = HttpStatusCode.OK, int statusCode = (int)HttpStatusCode.OK, string message = default)
        {
            Status = status;
            StatusCode = status == HttpStatusCode.OK ? statusCode : (int)status;
            Message = message;
        }

        public HttpStatusCode Status { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}

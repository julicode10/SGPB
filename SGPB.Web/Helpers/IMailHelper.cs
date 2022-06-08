using SGPB.Web.Responses;

namespace SGPB.Web.Helpers
{
        public interface IMailHelper
        {
                Response SendMail(string to, string subject, string body);
        }
}

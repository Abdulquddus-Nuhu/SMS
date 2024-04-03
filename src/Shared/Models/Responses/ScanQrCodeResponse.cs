using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Responses
{
    public record ScanQrCodeResponse
    {
        public string FullName { get; set; }
        public string Grade { get; set; }
        public DateTime InTimer { get; set; }
        public DateTime OutTimer { get; set; }
        public string Photo { get; set; }
        public string AuthorizedUser { get; set; }
        public bool IsAuthorizedUserParent { get; set; }
    }
}

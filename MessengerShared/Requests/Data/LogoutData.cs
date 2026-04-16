using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerShared.Requests.Data
{
    public class LogoutData
    {
        [Required]
        public string AccessToken { get; set; }
    }
}

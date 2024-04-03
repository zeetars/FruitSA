using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitSA.Model
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = "zeetars@gmail.com";
        public string EntityName { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Changes { get; set; }
    }
}

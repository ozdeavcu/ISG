using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.Entities
{
    public class FaqModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Ranking { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
       public bool IsActive { get; set; }
    }
}

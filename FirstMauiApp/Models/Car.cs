using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMauiApp.Models
{
    [Table("cars")]
    public class Car : BaseEntity
    {
        public string Make { get; set; }

        public string Model { get; set; }

        [MaxLength(20)]
        [Unique]
        public string Vin { get; set; }
    }
}

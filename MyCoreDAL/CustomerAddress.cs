using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyCoreDAL
{
    public class CustomerAddress : FartherEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("Customer")]
        [StringLength(50)]
        public string CustomerGuid { get; set; }

        [StringLength(50)]
        public string AddressGuid { get; set; }

        [StringLength(50)]
        public string Province { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string District { get; set; }

        [StringLength(200)]
        public string Address { get; set; }
    }
}

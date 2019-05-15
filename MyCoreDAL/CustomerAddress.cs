using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MyCoreDAL.FatherEntity;

namespace MyCoreDAL
{
    public class CustomerAddress : FatherEntitys
    {
        public int ID { get; set; }

        [ForeignKey("Customer")]
        [StringLength(50)]
        public string CustomerGuid { get; set; }

        [StringLength(50)]
        public string AddressGuid { get; set; } = Guid.NewGuid().ToString().Replace("-", "");

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

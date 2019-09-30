﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.DataModel
{
    public class FeedBack
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column("DeliveryId")]
        public int Id { get; set; }
        public string DeliveryId { get; set; }
        public string Ratings { get; set; }
        public string FoodFeedbacks { get; set; }
    }
}

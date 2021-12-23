using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flexap_Task.Models
{
    public class Product
    {   
        [Key]
        public int Id { get; set; }
        //[Range(5, 15, ErrorMessage = "")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Product Name Must be between 5 to 15 characters")]
        [Required(ErrorMessage = "Product Name is required.")]
        public string ProductName { get; set; }

        [Range(10, 5000, ErrorMessage = "Please enter valid integer Number")]
        [Required(ErrorMessage = "Price is required.")]
        public int Price { get; set; }

        [StringLength(13, MinimumLength = 13, ErrorMessage = "Barcode  Must be between 0 to 13 characters")]
        //[Range(0,13, ErrorMessage = "Please enter valid Barcode")]
        public string Barcode { get; set; }

        [Range(1, 99999, ErrorMessage = " Please enter valid Price look-up code")]
        [Required(ErrorMessage = "PLU is required.")]
        public int PLU { get; set; }

        public  bool Generated { get; set; } = false;
        [NotMapped]
        public IEnumerable<IFormFile> ProductImages { get; set; }
        public IEnumerable<ProductGalery>ProductGaleries { get; set; }


    }
}

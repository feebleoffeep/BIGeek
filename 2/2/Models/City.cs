﻿using Microsoft.AspNetCore.Identity;

namespace _2.Models 
{
    public class City
    {
        public int CityId { get; set; }
        public string? Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}







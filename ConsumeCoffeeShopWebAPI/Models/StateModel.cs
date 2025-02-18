﻿using System.ComponentModel.DataAnnotations;

namespace ConsumeCoffeeShopWebAPI.Models
{
	public class StateModel
	{
		public int StateID { get; set; }
		public int CountryID { get; set; }
		public string StateName { get; set; }
		public string StateCode { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string? CountryName { get; set; }
		public int CityCount { get; set; }
	}
}

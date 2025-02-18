using ConsumeCoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	public class PropertyController : Controller
	{
		public IActionResult Display()
		{
			List<Property> pro = new List<Property>()
{
	new Property
	{
		Id = "660548b19914597202532ac1",
		Photos = new List<string>
		{
			"https://placehold.co/600x400"
		},
		HomeTitle = "Nirman Sahvas",
		HomeAddress = "Adalaj, Ahmedabad North",
		HomePrice = "1200",
		HomeBedrooms = "2",
		HomeBathrooms = "3",
		HomeSizeinft = "1200",
		HomePropertyType = "Sell"
	},
	new Property
	{
		Id = "66057d0dd7132aef72e8c019",
		Photos = new List<string>
		{
			"https://placehold.co/600x400"
		},
		HomeTitle = "Saumya Saujanya 2",
		HomeAddress = "Khokhara, Ahmedabad East",
		HomePrice = "1200",
		HomeBedrooms = "3",
		HomeBathrooms = "3",
		HomeSizeinft = "1200",
		HomePropertyType = "Sell"
	},
	new Property
	{
		Id = "66057dc9d7132aef72e8c023",
		Photos = new List<string>
		{
			"https://placehold.co/600x400"
		},
		HomeTitle = "RB Elements",
		HomeAddress = "Ghanteshwer, Rajkot",
		HomePrice = "1400",
		HomeBedrooms = "3",
		HomeBathrooms = "2",
		HomeSizeinft = "1200",
		HomePropertyType = "Rent"
	},
	new Property
	{
		Id = "66057e33d7132aef72e8c028",
		Photos = new List<string>
		{
			"https://placehold.co/600x400"
		},
		HomeTitle = "Arvind Aavishkaar",
		HomeAddress = "Naroda Road, Rajkot",
		HomePrice = "1200",
		HomeBedrooms = "2",
		HomeBathrooms = "2",
		HomeSizeinft = "1200",
		HomePropertyType = "Rent"
	},
	new Property
	{
		Id = "66057f1ed7132aef72e8c033",
		Photos = new List<string>
		{
			"https://placehold.co/600x400"
		},
		HomeTitle = "Equestrian Family Home",
		HomeAddress = "Dwarika Nagar, Jamnagar",
		HomePrice = "12000",
		HomeBedrooms = "3",
		HomeBathrooms = "3",
		HomeSizeinft = "1600",
		HomePropertyType = "Buy"
	},
	new Property
	{
		Id = "66057f8cd7132aef72e8c038",
		Photos = new List<string>
		{
			"https://placehold.co/600x400"
		},
		HomeTitle = "Luxury Villa in Rego Park",
		HomeAddress = "Aestron Chowk, Rajkot",
		HomePrice = "82000",
		HomeBedrooms = "2",
		HomeBathrooms = "3",
		HomeSizeinft = "1400",
		HomePropertyType = "Buy"
	},
	new Property
	{
		Id = "66058025d7132aef72e8c040",
		Photos = new List<string>
		{
			"https://placehold.co/600x400"
		},
		HomeTitle = "Kruti Onella",
		HomeAddress = "150 Feet Ring Road, Rajkot",
		HomePrice = "21000",
		HomeBedrooms = "3",
		HomeBathrooms = "2",
		HomeSizeinft = "1400",
		HomePropertyType = "Buy"
	}
};


			return View(pro);
		}
	}
}

namespace ConsumeCoffeeShopWebAPI.Models
{
	public class Property
	{
		public string Id { get; set; }
		public List<string> Photos { get; set; }
		public string HomeTitle { get; set; }
		public string HomeAddress { get; set; }
		public string HomePrice { get; set; }
		public string HomeBedrooms { get; set; }
		public string HomeBathrooms { get; set; }
		public string HomeSizeinft { get; set; }
		public string HomePropertyType { get; set; }
	}
}

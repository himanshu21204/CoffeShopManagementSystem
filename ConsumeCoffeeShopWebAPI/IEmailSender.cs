namespace ConsumeCoffeeShopWebAPI
{
	public interface IEmailSender
	{
		Task<bool> SendEmail(string email, string subject, string message);
	}
}

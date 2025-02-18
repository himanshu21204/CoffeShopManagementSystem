using CoffeeShopWebAPI.Models;
using FluentValidation;

namespace CoffeeShopWebAPI.Validators
{
	public class OrderValidator : AbstractValidator<OrderModel>
	{
		public OrderValidator()
		{
			RuleFor(order => order.OrderDate)
				.NotEmpty().WithMessage("Order Date is required.")
				.Must(date => date.HasValue).WithMessage("Invalid date format.");

			RuleFor(order => order.OrderNumber)
				.NotEmpty().WithMessage("Order Number is required.");

			RuleFor(order => order.TotalAmount)
				.NotEmpty().WithMessage("Total Amount is required.")
				.GreaterThan(0).WithMessage("Total Amount must be greater than zero.");

			RuleFor(order => order.ShippingAddress)
				.NotEmpty().WithMessage("Shipping Address is required.");

			RuleFor(order => order.UserID)
				.NotEmpty().WithMessage("User ID is required.")
				.GreaterThan(0).WithMessage("User ID must be a positive number.");

			RuleFor(order => order.CustomerID)
				.NotEmpty().WithMessage("Customer ID is required.")
				.GreaterThan(0).WithMessage("Customer ID must be a positive number.");
		}
	}
}

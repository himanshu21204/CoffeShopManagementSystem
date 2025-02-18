using CoffeeShopWebAPI.Models;
using FluentValidation;

namespace CoffeeShopWebAPI.Validators
{
	public class OrderDetailValidator :AbstractValidator<OrderDetailModel>
	{
		public OrderDetailValidator()
		{
			RuleFor(detail => detail.OrderDetailID)
				.NotEmpty().WithMessage("Order Detail ID is required.");

			RuleFor(detail => detail.OrderID)
				.NotEmpty().WithMessage("Order ID is required.")
				.GreaterThan(0).WithMessage("Order ID must be a positive number.");

			RuleFor(detail => detail.ProductID)
				.NotEmpty().WithMessage("Product ID is required.");

			RuleFor(detail => detail.Quantity)
				.NotEmpty().WithMessage("Quantity is required.")
				.GreaterThan(0).WithMessage("Quantity must be greater than 0.");

			RuleFor(detail => detail.Amount)
				.NotEmpty().WithMessage("Amount is required.")
				.GreaterThan(0).WithMessage("Amount must be greater than 0.");

			RuleFor(detail => detail.TotalAmount)
				.NotEmpty().WithMessage("Total Amount is required.")
				.GreaterThan(0).WithMessage("Total Amount must be greater than 0.");

			RuleFor(detail => detail.UserID)
				.NotEmpty().WithMessage("User ID is required.")
				.GreaterThan(0).WithMessage("User ID must be a positive number.");
		}
	}
}

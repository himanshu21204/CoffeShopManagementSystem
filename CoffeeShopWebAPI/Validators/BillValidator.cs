using CoffeeShopWebAPI.Models;
using FluentValidation;

namespace CoffeeShopWebAPI.Validators
{
	public class BillValidator : AbstractValidator<BillModel>
	{
		public BillValidator()
		{
			RuleFor(bill => bill.BillNumber)
				.NotEmpty().WithMessage("Bill Number is required.")
				.Length(5,7).WithMessage("Bill Number must be within 5 to 7 Character.");

			RuleFor(bill => bill.BillDate)
				.NotEmpty().WithMessage("Bill Date is required.")
				.Must(date => date.HasValue).WithMessage("Invalid date format.");

			RuleFor(bill => bill.OrderID)
				.NotEmpty().WithMessage("Order ID is required.")
				.GreaterThan(0).WithMessage("Order ID must be a positive number.");

			RuleFor(bill => bill.TotalAmount)
				.NotEmpty().WithMessage("Total Amount is required.")
				.GreaterThan(0).WithMessage("Total Amount must be greater than zero.")
				.LessThan(5000).WithMessage("Total Amount must be less than 5000.");

			RuleFor(bill => bill.Discount)
				.GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
				.GreaterThan(10)
				.LessThan(95).WithMessage("Discount must be within 10 to 95.");

			RuleFor(bill => bill.NetAmount)
				.NotEmpty().WithMessage("Net Amount is required.")
				.GreaterThan(0).WithMessage("Net Amount must be positive number.");

			RuleFor(bill => bill.UserID)
				.NotEmpty().WithMessage("User ID is required.")
				.GreaterThan(0).WithMessage("User ID must be a positive number.");
		}
	}
}

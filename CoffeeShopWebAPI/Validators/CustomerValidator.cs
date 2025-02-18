using CoffeeShopWebAPI.Models;
using FluentValidation;

namespace CoffeeShopWebAPI.Validators
{
	public class CustomerValidator : AbstractValidator<CustomerModel>
	{
		public CustomerValidator()
		{
			RuleFor(customer => customer.CustomerID)
			.NotEmpty().WithMessage("Customer ID is required.");

			RuleFor(customer => customer.CustomerName)
				.NotEmpty().WithMessage("Customer Name is required.");

			RuleFor(customer => customer.HomeAddress)
				.NotEmpty().WithMessage("Home Address is required.");

			RuleFor(customer => customer.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid Email Address.");

			RuleFor(customer => customer.MobileNo)
				.NotEmpty().WithMessage("Mobile Number is required.")
				.Matches(@"^\d{10}$").WithMessage("Invalid Mobile Number.");

			RuleFor(customer => customer.GST_NO)
				.NotEmpty().WithMessage("GST Number is required.")
				.MaximumLength(15).WithMessage("GST Number can't be longer than 15 characters.");

			RuleFor(customer => customer.CityName)
				.NotEmpty().WithMessage("City Name is required.");

			RuleFor(customer => customer.PinCode)
				.NotEmpty().WithMessage("Pin Code is required.")
				.Length(6).WithMessage("Pin Code must be exactly 6 characters.")
				.Matches(@"^\d+$").WithMessage("Pin Code must be numeric.");

			RuleFor(customer => customer.NetAmount)
				.NotEmpty().WithMessage("Net Amount is required.")
				.GreaterThan(0).WithMessage("Net Amount must be greater than zero.");

			RuleFor(customer => customer.UserID)
				.NotEmpty().WithMessage("User ID is required.");
		}
	}
}

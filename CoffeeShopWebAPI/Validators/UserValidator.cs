using CoffeeShopWebAPI.Models;
using FluentValidation;

public class UserValidator : AbstractValidator<UserModel>
{
	public UserValidator()
	{
		RuleFor(user => user.UserID)
				.NotEmpty().WithMessage("User ID is required.");

		RuleFor(user => user.UserName)
			.NotEmpty().WithMessage("User Name is required.")
			.MaximumLength(50).WithMessage("User Name cannot exceed 50 characters.");

		RuleFor(user => user.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Invalid Email format.");

		RuleFor(user => user.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

		RuleFor(user => user.MobileNo)
			.NotEmpty().WithMessage("Mobile Number is required.")
			.Matches(@"^\d{10}$").WithMessage("Mobile Number must be a valid 10-digit number.");

		RuleFor(user => user.Address)
			.NotEmpty().WithMessage("Address is required.")
			.MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

	}
}

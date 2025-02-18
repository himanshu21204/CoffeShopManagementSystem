using CoffeeShopWebAPI.Models;
using FluentValidation;

namespace CoffeeShopWebAPI.Validator
{
	public class StateValidator : AbstractValidator<StateModel>
	{
		public StateValidator()
		{
			RuleFor(x => x.CountryID).NotNull().WithMessage("Country ID is required.")
			.GreaterThan(0).WithMessage("Country ID is required.");

			// StateName validation
			RuleFor(x => x.StateName)
				.NotEmpty().WithMessage("State Name is required.")
				.MaximumLength(100).WithMessage("State Name cannot be longer than 100 characters.");

			// StateCode validation
			RuleFor(x => x.StateCode)
				.NotEmpty().WithMessage("State Code is required.")
				.MaximumLength(10).WithMessage("State Code cannot be longer than 10 characters.");
		}
	}
}

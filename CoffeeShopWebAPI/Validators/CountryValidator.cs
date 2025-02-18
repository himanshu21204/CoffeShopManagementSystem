using CoffeeShopWebAPI.Models;
using FluentValidation;

namespace CoffeeShopWebAPI.Validator
{
	public class CountryValidator : AbstractValidator<CountryModel>
	{
		public CountryValidator()
		{
			RuleFor(x => x.CountryName)
			.NotEmpty().WithMessage("Country Name is required.")
			.MaximumLength(100).WithMessage("Country Name cannot be longer than 100 characters.");

			// CountryCode validation
			RuleFor(x => x.CountryCode)
				.NotEmpty().WithMessage("Country Code is required.")
				.MaximumLength(10).WithMessage("Country Code cannot be longer than 10 characters.");
		}
	}
}

using CoffeeShopWebAPI.Models;
using FluentValidation;

namespace CoffeeShopWebAPI.Validator
{
	public class CityValidation : AbstractValidator<CityModel>
	{
		public CityValidation()
		{
			RuleFor(x => x.StateID).NotNull().WithMessage("State ID is required.")
				.GreaterThan(0).WithMessage("State ID must be a positive number.");

			RuleFor(x => x.CountryID).NotNull().WithMessage("Country ID is required.")
				.GreaterThan(0).WithMessage("Country ID must be a positive number.");

			RuleFor(x => x.CityName)
				.NotEmpty().WithMessage("City Name is required.")
				.MaximumLength(25).WithMessage("City Name cannot be longer than 25 characters.");

			RuleFor(x => x.CityCode)
				.NotEmpty().WithMessage("City Code is required.")
				.MaximumLength(10).WithMessage("City Code cannot be longer than 10 characters.");
			//RuleFor(x => x.CreatedDate)
			//	.NotEmpty().WithMessage("Date Time is required")
			//	.GreaterThan(DateTime.Today).WithMessage("Date Must be Future")
			//	.LessThan(DateTime.Now.AddDays(30)).WithMessage("Date must be within next 30 days")
			//	.Must(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday).WithMessage("Dates on weekend not allowed")
			//	.
		}
	}
}

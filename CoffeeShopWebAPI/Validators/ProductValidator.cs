using CoffeeShopWebAPI.Models;
using FluentValidation;

namespace CoffeeShopWebAPI.Validators
{
	public class ProductValidator : AbstractValidator<ProductModel>
	{
		public ProductValidator()
		{
			RuleFor(product => product.ProductName).NotNull()
				.NotEmpty().WithMessage("Product Name is required.")
				.MaximumLength(100).WithMessage("Product Name Must be within 100 Character");

			RuleFor(product => product.ProductPrice).NotNull()
				.NotEmpty().WithMessage("Price is required.")
				.GreaterThan(0).WithMessage("Product Price must be greater than 0")
				.LessThan(15000).WithMessage("Price must be less than 15000");

			RuleFor(product => product.ProductCode).NotNull()
				.NotEmpty().WithMessage("Product Code is required")
				.Length(5, 10).WithMessage("Product Code Must be within 5 to 10 Character");

			RuleFor(product => product.Description).NotNull()
				.NotEmpty().WithMessage("Description is required.")
				.MaximumLength(500).WithMessage("Description must be within 500 Character");

			RuleFor(product => product.UserID).NotNull()
				.NotEmpty().WithMessage("User ID is required")
				.GreaterThan(0).WithMessage("User ID must be a positive number");
			
		}
	}
}

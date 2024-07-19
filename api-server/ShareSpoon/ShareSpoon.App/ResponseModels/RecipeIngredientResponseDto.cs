using ShareSpoon.Domain.Enums;

namespace ShareSpoon.App.ResponseModels
{
    public class RecipeIngredientResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public QuantityType QuantityType { get; set; }
    }
}

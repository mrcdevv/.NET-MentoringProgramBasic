namespace Task1
{
    public class Product
    {
        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }

        public double Price { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Product prod)
            {
                if (prod.Name == Name && prod.Price == Price)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

using System.ComponentModel;
namespace SimplePOS

{
    public class Product
    {
        public long Id { get; set; }
        public string Barcode { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }

        public decimal Quantity { get; set; }
    }
   
    public class CartItem : INotifyPropertyChanged 
    {
            
        public long ProductId { get; set; }
        public string Barcode { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public decimal CostPrice { get; set;  }

        private int _qty = 1;
        public int Qty { get => _qty; set
           {
                _qty = value;
                OnPropertyChanged(nameof(Qty));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(Profit));
            } }
        public decimal Total => Price * Qty;
        public decimal Profit => (Price - CostPrice) * Qty;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

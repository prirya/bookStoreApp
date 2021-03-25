using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookStoreApp
{
    public class UserModel
    {
        public int Number { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public DateTime birthday { get; set; }
        public bool sex { get; set; }
        public bool typeAdmin { get; set; }
    }
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public bool Sex { get; set; }
        public string Phone { get; set; }
        public CustomerModel() { }
        public CustomerModel(int customerId, string name, string address, string email, DateTime birthday, bool sex, string phone)
        {
            CustomerId = customerId;
            Name = name;
            Address = address;
            Email = email;
            Birthday = birthday;
            Sex = sex;
            Phone = phone;
        }
    }
    public class BookModel
    {
        public int Number { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public BookModel() { }
        public BookModel(int number, string isbn, string title, string type, string description, decimal price, int quantity)
        {
            Number = number;
            ISBN = isbn;
            Title = title;
            Type = type;
            Description = description;
            Price = price;
            Quantity = quantity;
        }
    }
    public class TransactionsSold
    {
        public int Number { get; set; }
        public string ISBN { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public string QuantitySold { get; set; }
        public DateTime TimeSold { get; set; }
        public string PricePerBook { get; set; }
        public TransactionsSold() { }
        public TransactionsSold(int number, string isbn, int customerid, decimal totalPrice, string quantitySold, DateTime timeSold, string pricePerBook)
        {
            Number = number;
            ISBN = isbn;
            CustomerId = customerid;
            TotalPrice = totalPrice;
            QuantitySold = quantitySold;
            TimeSold = timeSold;
            PricePerBook = pricePerBook;
        }
    }
    public class GetdataTransactions
    {
        public string ISBN { get; set; }
        public string TitleBook { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int QuantitySold { get; set; }
        public int Quantity { get; set; }
        public int Number { get; set; }
        public GetdataTransactions() { }
        public GetdataTransactions(string isbn, string titleBook, string type, decimal price, int quantitySold, int quantity, int number)
        {
            ISBN = isbn;
            TitleBook = titleBook;
            Type = type;
            Price = price;
            QuantitySold = quantitySold;
            Quantity = quantity;
            Number = number;

        }
    }
    public class Bill
    {
        public int NumberBill { get; set; }
        public int CustomerID { get; set; }
        public DateTime TimeSold { get; set; }
        public int User { get; set; }
        public Bill() { }
        public Bill(int numberBill,int customerID, DateTime timeSold, int user)
        {
            NumberBill = numberBill;
            CustomerID = customerID;
            TimeSold = timeSold;
            User = user;
        }
    }
    public class BillDetail
    {
        public int NumberBill { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
        public BillDetail() { }
        public BillDetail(int numberBill, string Isbn, int quantity)
        {
            NumberBill = numberBill;
            ISBN = Isbn;
            Quantity = quantity;
        }
    }
}

﻿using System;
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
        public int Phone { get; set; }
        public CustomerModel() { }
        public CustomerModel(int customerId,string name,string address, string email,DateTime birthday ,bool sex ,int phone) 
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
        public BookModel(int number, string isbn, string title, string type, string description, decimal price,int quantity)
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
}

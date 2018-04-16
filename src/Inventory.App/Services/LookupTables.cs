﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;

namespace Inventory.Services
{
    public class LookupTables : ILookupTables
    {
        public LookupTables(IDataServiceFactory dataServiceFactory)
        {
            DataServiceFactory = dataServiceFactory;
        }

        public IDataServiceFactory DataServiceFactory { get; }

        public IList<CategoryModel> Categories { get; private set; }
        public IList<CountryCodeModel> CountryCodes { get; private set; }
        public IList<OrderStatusModel> OrderStatus { get; private set; }
        public IList<PaymentTypeModel> PaymentTypes { get; private set; }
        public IList<ShipperModel> Shippers { get; private set; }
        public IList<TaxTypeModel> TaxTypes { get; private set; }

        public async Task InitializeAsync()
        {
            Categories = await GetCategoriesAsync();
            CountryCodes = await GetCountryCodesAsync();
            OrderStatus = await GetOrderStatusAsync();
            PaymentTypes = await GetPaymentTypesAsync();
            Shippers = await GetShippersAsync();
            TaxTypes = await GetTaxTypesAsync();
        }

        public string GetCategory(int id)
        {
            return Categories.Where(r => r.CategoryID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetCountry(string id)
        {
            return CountryCodes.Where(r => r.CountryCodeID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetOrderStatus(int id)
        {
            return OrderStatus.Where(r => r.Status == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetPaymentType(int? id)
        {
            return id == null ? "" : PaymentTypes.Where(r => r.PaymentTypeID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetShipper(int? id)
        {
            return id == null ? "" : Shippers.Where(r => r.ShipperID == id).Select(r => r.Name).FirstOrDefault();
        }

        public string GetTaxDesc(int id)
        {
            return TaxTypes.Where(r => r.TaxTypeID == id).Select(r => $"{r.Rate} %").FirstOrDefault();
        }
        public decimal GetTaxRate(int id)
        {
            return TaxTypes.Where(r => r.TaxTypeID == id).Select(r => r.Rate).FirstOrDefault();
        }

        private async Task<IList<CategoryModel>> GetCategoriesAsync()
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCategoriesAsync();
                return items.Select(r => new CategoryModel
                {
                    CategoryID = r.CategoryID,
                    Name = r.Name
                })
                .ToList();
            }
        }

        private async Task<IList<CountryCodeModel>> GetCountryCodesAsync()
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCountryCodesAsync();
                return items.OrderBy(r => r.Name).Select(r => new CountryCodeModel
                {
                    CountryCodeID = r.CountryCodeID,
                    Name = r.Name
                })
                .ToList();
            }
        }

        private async Task<IList<OrderStatusModel>> GetOrderStatusAsync()
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetOrderStatusAsync();
                return items.Select(r => new OrderStatusModel
                {
                    Status = r.Status,
                    Name = r.Name
                })
                .ToList();
            }
        }

        private async Task<IList<PaymentTypeModel>> GetPaymentTypesAsync()
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPaymentTypesAsync();
                return items.Select(r => new PaymentTypeModel
                {
                    PaymentTypeID = r.PaymentTypeID,
                    Name = r.Name
                })
                .ToList();
            }
        }

        private async Task<IList<ShipperModel>> GetShippersAsync()
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetShippersAsync();
                return items.Select(r => new ShipperModel
                {
                    ShipperID = r.ShipperID,
                    Name = r.Name,
                    Phone = r.Phone
                })
                .ToList();
            }
        }

        private async Task<IList<TaxTypeModel>> GetTaxTypesAsync()
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetTaxTypesAsync();
                return items.Select(r => new TaxTypeModel
                {
                    TaxTypeID = r.TaxTypeID,
                    Name = r.Name,
                    Rate = r.Rate
                })
                .ToList();
            }
        }
    }
}
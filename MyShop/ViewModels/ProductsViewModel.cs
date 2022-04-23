
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MyShop.Models;
using MyShop.Ultilities;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using System.Reflection;
using System.Windows;

namespace MyShop.ViewModels
{
    public class ProductsViewModel: ViewModelBase
    {
        public int CurrentPage { get; set; } = 1;
        public bool IsNotEmpty { get; set; } = false;
        public int TotalPage { get; set; } = 1;
        public int SelectedCategory { get; set; } = 0;
        public bool SmallQuantityCheck { get; set; } = false;

        public bool FilterPriceCheck { get; set; } = false;
        public int LowPrice { get; set; }
        public int HighPrice { get; set; }
        ProjectMyShopContext myShopEntitiesDbContext;
        public string Keyword { get; set; } = "";
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public FilterCheckViewModel TempViewModel { get; set; } = new FilterCheckViewModel();
        public class FilterCheckViewModel : ViewModelBase
        {
            public bool SmallQuantityCheck { get; set; } = false;
            public bool FilterPriceCheck { get; set; } = false;
            public int LowPrice { get; set; }
            public int HighPrice { get; set; }
        }
        public ProductsViewModel()
        {
            myShopEntitiesDbContext = DbUltility.Instance.Db;
            LoadCategories();
            LoadItems();
        }
        public void LoadCategories()
        {
            var list = myShopEntitiesDbContext.Categories.ToList();
            Categories.Clear();
            Categories.Add(new Category { CategoryName = AppConfigUltility.CurrentLanguage.Equals("EN")?"All" : "Tất cả" });
            foreach (var it in list)
            {
                Categories.Add(it);
            }
        }
        public void LoadItems()
        {

            string sql = "SELECT * FROM Product";
            //filter by keyword
            if (Keyword != "")
            {
                sql += $" Where Product.product_name LIKE N'%{Keyword}%'";
            }
            var query = myShopEntitiesDbContext.Products.FromSqlRaw(sql)
                    .Include(product => product.Category)
                    .Where(product => product.Active != 0);
           
            
            
            //filter by category
            if (SelectedCategory != 0)
            {
                var category = Categories[SelectedCategory];
                query = query.Where(product => product.Category.CategoryId == category.CategoryId);
            }
            if (SmallQuantityCheck)
            {
                query = query.Where(product => product.ProductQuantityInStock <= 5);
            }
            if (FilterPriceCheck)
            {
                query = query.Where(product => product.ProductPrice >= LowPrice && product.ProductPrice <= HighPrice);
            }
            int PageSize = AppConfigUltility.CurrentPageSize;
            if (query != null && query.Any())
            {
                TotalPage = query.Count() / PageSize +
                    (query.Count() % PageSize == 0 ? 0 : 1);
            }
            else TotalPage = 0;


            var items = query.Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToArray();

            Products.Clear();
            foreach (var item in items)
            {
                Products.Add(item);
            }
            IsNotEmpty = Products.Count != 0;
        }
        public void AddProduct(Product product)
        {
            
            try{
                product.Active = 1;
                product.CreateAt = System.DateTime.Now;
                myShopEntitiesDbContext.Products.Add(product);
                myShopEntitiesDbContext.SaveChanges();
                Products.Add(product);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Database Error!");
            }
        }
        public void ImportData(List<Category> categories, List<Product> products)
        {
            AddCategories(categories);
            myShopEntitiesDbContext.SaveChanges();
            CloneImage(products);
            myShopEntitiesDbContext.SaveChanges();
            CurrentPage = 1;
            LoadCategories();
            LoadItems();
        }
        void AddCategories(List<Category> categories)
        {
            foreach(var category in categories)
            {
                var res = myShopEntitiesDbContext.Categories.Where(c => c.Active != 0 && c.CategoryName == category.CategoryName).FirstOrDefault();
                if(res != null)
                {
                    category.CategoryId = res.CategoryId;
                    
                }
                else
                {
                    myShopEntitiesDbContext.Categories.Add(category);
                }
            }
            
        }
        void CloneImage(List<Product> products)
        {
            foreach(var p in products)
            {
                if (!p.ProductImage.Equals("Resources\\Assets\\mobile.jpg"))
                {
                    if (FileUltility.IsExists(p.ProductImage))
                    {
                        var source = p.ProductImage;
                        p.ProductImage = FileUltility.CreateNewImagePath(source);
                        FileUltility.CopyImage(source, p.ProductImage);
                    }
                    else p.ProductImage = "Resources\\Assets\\mobile.jpg";

                }
                var cId = p.Category.CategoryId;
                var tmp = myShopEntitiesDbContext.Categories.Where(c => c.CategoryId == cId).FirstOrDefault();
                p.Category = tmp;
                p.CategoryId = cId;


                myShopEntitiesDbContext.Products.Add(p);

            }
        }
        public void EditProduct(int index, Product product)
        {
            var result = myShopEntitiesDbContext.Products.SingleOrDefault(p => p.ProductId == product.ProductId);
            if (result != null)
            {
                try
                {
                    PropertiesCopy.CopyProperties(product, result);
                    myShopEntitiesDbContext.SaveChanges();
                    Products[index] = product.Clone() as Product;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error!");
                }
            }
            
            
        }
        public void Next() {
            if (CurrentPage < TotalPage)
            {
                CurrentPage++;
                LoadItems();
            }
            
        }
        public void Pre()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadItems();
            }
        }
        public void SelectCategory(int index)
        {
            SelectedCategory = index;
            CurrentPage = 1;
            LoadItems();
        }
        public void CheckSmallQuantity()
        {
            SmallQuantityCheck = !SmallQuantityCheck;
            CurrentPage = 1;
            LoadItems();
        }
        public void UnChecckFilterPrice()
        {
            FilterPriceCheck = false;
            CurrentPage = 1;
            LoadItems();
        }
        public void SetPrices(int lowPrice, int highPrice)
        {
            FilterPriceCheck = true;
            LowPrice = lowPrice;
            HighPrice = highPrice;
            CurrentPage = 1;
            LoadItems();
        }
        public void Search()
        {
            CurrentPage = 1;
            LoadItems();
        }
        public void DeleteProduct(int index)
        {
            var result = myShopEntitiesDbContext.Products.SingleOrDefault(p => p.ProductId == Products[index].ProductId);
            if (result != null)
            {
                try
                {
                    result.Active = 0;
                    myShopEntitiesDbContext.SaveChanges();
                    Products.RemoveAt(index);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error!");
                }
                
            }
            
        }
        public void OpenFilter()
        {
            TempViewModel = new FilterCheckViewModel
            {
                FilterPriceCheck = this.FilterPriceCheck,
                SmallQuantityCheck = this.SmallQuantityCheck,
                LowPrice = this.LowPrice,
                HighPrice = this.HighPrice
            };
        }
        public void SaveFilter()
        {
            FilterPriceCheck = TempViewModel.FilterPriceCheck;
            SmallQuantityCheck = TempViewModel.SmallQuantityCheck;
            LowPrice = TempViewModel.LowPrice;
            HighPrice = TempViewModel.HighPrice;
            CurrentPage = 1;
            LoadItems();
        }

    }
}

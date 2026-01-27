using System.Data;
using InventoryManagementSystem.Controllers;
using InventoryManagementSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

// using InventoryManagementSystem.Controllers; // 🛑 ही ओळ काढून टाकली आहे

namespace InventoryManagementSystem.DAL
{
    public class ProductDAL
    {
        private readonly string _connectionString;

        // Constructor madhe connection string initialize karu
        public ProductDAL(IConfiguration configuration)
        {
            // "DefaultConnection" connection string AppSettings.json madhun gheto
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // --- LOOKUP METHODS (Dropdown sathi Categories ani Suppliers cha data) ---

        // Categories list fetch karun aanto
        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            string query = "SELECT CategoryID, CategoryName FROM Categories";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryID = Convert.ToInt32(reader["CategoryID"]),
                            CategoryName = reader["CategoryName"].ToString()
                        });
                    }
                }
            }
            return categories;
        }

        // Suppliers list fetch karun aanto
        public List<Supplier> GetSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();
            string query = "SELECT SupplierID, SupplierName FROM Suppliers";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suppliers.Add(new Supplier
                        {
                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"].ToString()
                        });
                    }
                }
            }
            return suppliers;
        }


        // --- PRODUCT CRUD OPERATIONS ---

        // R - Read: Sagale Products List (Category/Supplier Name sobat)
        public List<ProductViewModel> GetAllProducts()
        {
            List<ProductViewModel> productList = new List<ProductViewModel>();

            // JOIN query to get related names
            string query = @"SELECT p.ProductID, p.ProductName, p.Price, p.QuantityInStock, 
                            p.CategoryID, c.CategoryName, p.SupplierID, s.SupplierName
                            FROM Products p
                            INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                            INNER JOIN Suppliers s ON p.SupplierID = s.SupplierID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productList.Add(new ProductViewModel
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                            CategoryID = Convert.ToInt32(reader["CategoryID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"].ToString()
                        });
                    }
                }
            }
            return productList;
        }

        // C - Create: Navin Product Add kara
        public void AddProduct(ProductViewModel product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Products (ProductName, Price, QuantityInStock, CategoryID, SupplierID) VALUES (@Name, @Price, @Qty, @CatID, @SupID)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", product.ProductName);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Qty", product.QuantityInStock);
                    cmd.Parameters.AddWithValue("@CatID", product.CategoryID);
                    cmd.Parameters.AddWithValue("@SupID", product.SupplierID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // R - Read: Ek Product ID pramane fetch karun aanto (Edit sathi)
        public ProductViewModel GetProductById(int id)
        {
            ProductViewModel product = null;
            string query = @"SELECT p.ProductID, p.ProductName, p.Price, p.QuantityInStock, 
                            p.CategoryID, p.SupplierID, c.CategoryName, s.SupplierName
                            FROM Products p
                            INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                            INNER JOIN Suppliers s ON p.SupplierID = s.SupplierID
                            WHERE p.ProductID = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        product = new ProductViewModel
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                            CategoryID = Convert.ToInt32(reader["CategoryID"]),
                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            SupplierName = reader["SupplierName"].ToString()
                        };
                    }
                }
            }
            return product;
        }

        // U - Update: Product chi mahiti badalto
        public void UpdateProduct(ProductViewModel product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Products 
                             SET ProductName=@Name, Price=@Price, QuantityInStock=@Qty, 
                             CategoryID=@CatID, SupplierID=@SupID 
                             WHERE ProductID=@Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", product.ProductID);
                    cmd.Parameters.AddWithValue("@Name", product.ProductName);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Qty", product.QuantityInStock);
                    cmd.Parameters.AddWithValue("@CatID", product.CategoryID);
                    cmd.Parameters.AddWithValue("@SupID", product.SupplierID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // D - Delete: Product la database madhun kadhun takto
        public void DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Products WHERE ProductID = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // --- CATEGORY CRUD METHODS ---

        public void AddCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Categories (CategoryName) VALUES (@Name)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", category.CategoryName);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Category GetCategoryById(int id)
        {
            Category category = null;
            string query = "SELECT CategoryID, CategoryName FROM Categories WHERE CategoryID = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        category = new Category
                        {
                            CategoryID = Convert.ToInt32(reader["CategoryID"]),
                            CategoryName = reader["CategoryName"].ToString()
                        };
                    }
                }
            }
            return category;
        }

        public void UpdateCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Categories SET CategoryName=@Name WHERE CategoryID=@Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", category.CategoryID);
                    cmd.Parameters.AddWithValue("@Name", category.CategoryName);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCategory(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Categories WHERE CategoryID = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // --- SUPPLIER CRUD METHODS ---

        public void AddSupplier(Supplier supplier)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Suppliers (SupplierName) VALUES (@Name)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", supplier.SupplierName);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Supplier GetSupplierById(int id)
        {
            Supplier supplier = null;
            string query = "SELECT SupplierID, SupplierName FROM Suppliers WHERE SupplierID = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        supplier = new Supplier
                        {
                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"].ToString()
                        };
                    }
                }
            }
            return supplier;
        }

        public void UpdateSupplier(Supplier supplier)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Suppliers SET SupplierName=@Name WHERE SupplierID=@Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", supplier.SupplierID);
                    cmd.Parameters.AddWithValue("@Name", supplier.SupplierName);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSupplier(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Suppliers WHERE SupplierID = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
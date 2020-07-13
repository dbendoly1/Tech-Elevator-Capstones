using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    class CategorySqlDAO
    {

        
        private string connectionString;
        public CategorySqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Category> ListCategories(int venueSelection)
        {

            List<Category> categoryList = new List<Category>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    string sql_categories = "SELECT c.name From category c \n" +
                                            "JOIN category_venue cv ON cv.category_id = c.id \n" +
                                            "JOIN venue v ON v.id = cv.venue_id \n" +
                                          
                                            "WHERE v.id = @venueselection";

                    SqlCommand cmd = new SqlCommand(sql_categories, conn);
                    cmd.Parameters.AddWithValue("@venueSelection", venueSelection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {

                        Category category = new Category();
                        category = CategoryAttributes(reader);

                        categoryList.Add(category);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.Message);
            }
            return categoryList;
        }


        private Category CategoryAttributes(SqlDataReader reader)
        {
            Category category = new Category();
            category.CatName = Convert.ToString(reader["name"]);
            return category;
        }
    }
}

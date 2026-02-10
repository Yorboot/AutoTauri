using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using DevOne.Security.Cryptography.BCrypt;
using ElectroAutoApi.Seeders;
using Car = ElectroAutoApi.Data.Car;


namespace ElectroAutoApi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpListener lister = new HttpListener();
            var db = new AppDbContext();
            var seed = new CarSeeder(db);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            seed.Seed(280);
                
            lister.Prefixes.Add("http://localhost:8080/");
            lister.Start();
            Console.WriteLine("Listening on http://localhost:8080/");
            while (true)
            {
                HttpListenerContext context = lister.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string responseString = "";

                if (request.Url.AbsolutePath == "/all")
                {
                    var Cars = db.Cars.ToList();

                    responseString += JsonSerializer.Serialize(Cars);
                }else if(request.Url.AbsolutePath == "/update" && request.HttpMethod == "POST"){
                    
                        var form = request.InputStream;
                        var reader = new System.IO.StreamReader(form);
                        var body = reader.ReadToEnd();
                        var Car = JsonSerializer.Deserialize<Car>(body);
                        if (Car != null)
                        {
                            Car CarFromDb = db.Cars.FirstOrDefault(t => t.Id == Car.Id);
                            if (CarFromDb != null)
                            {
                                CarFromDb.UserId = Car.UserId;
                                CarFromDb.LicensePlate = Car.LicensePlate;
                                CarFromDb.Brand = Car.Brand;
                                CarFromDb.Model = Car.Model;
                                CarFromDb.Price = Car.Price;
                                CarFromDb.Mileage = Car.Mileage;
                                CarFromDb.Seats = Car.Seats;
                                CarFromDb.Doors = Car.Doors;
                                CarFromDb.ProductionYear = Car.ProductionYear;
                                CarFromDb.Weight = Car.Weight;
                                CarFromDb.Color = Car.Color;
                                CarFromDb.Image = Car.Image;
                                CarFromDb.SoldAt = Car.SoldAt;
                                CarFromDb.Views = Car.Views;
                                CarFromDb.UpdatedAt = DateTime.Now;
                                db.Cars.Update(CarFromDb);
                                db.SaveChanges();
                                response.StatusCode = 201;
                                continue;
                            }else
                            {
                                response.StatusCode = 404;
                                continue;
                            }

                        }
                        else
                        {
                            response.StatusCode = 404;
                            continue;
                        }
                        
                }else if (request.Url.AbsolutePath == "/create" && request.HttpMethod == "POST")
                {
                        var form = request.InputStream;
                        var reader = new System.IO.StreamReader(form);
                        var body = reader.ReadToEnd();
                        var Car = JsonSerializer.Deserialize<Car>(body);
                        if (Car == null)
                        {
                            response.StatusCode = 404;
                            continue;
                        }
                        db.Cars.Add(Car);
                        db.SaveChanges();
                        response.StatusCode = 201;
                        continue;
                        continue;
                        
                }else if (request.Url.AbsolutePath == "/delete" && request.HttpMethod == "DELETE")
                { 
                        var form = request.InputStream;
                        var reader = new System.IO.StreamReader(form);
                        var body = reader.ReadToEnd();
                        var Car = JsonSerializer.Deserialize<Car>(body);
                        if (Car == null)
                        {
                            response.StatusCode = 404;
                            continue;
                        }

                        var CarToRemove = db.Cars.Find(Car.Id);
                        if (CarToRemove != null)
                        {
                            db.Cars.Remove(CarToRemove);
                            db.SaveChanges();
                            response.StatusCode = 200;
                            continue;
                        }
                        else
                        {
                            response.StatusCode = 404;
                            continue;
                        }
                }

                // }else if ( request.Url.AbsolutePath == "/users")
                // {
                //     using(var db = new AppDbContext())
                //     {
                //         var users = db.Users.ToList();
                //         responseString += JsonSerializer.Serialize(users);
                //     }
                //     
                // } else if (request.Url.AbsolutePath == "/register" && request.HttpMethod == "POST")
                // {
                //     using (var db = new AppDbContext())
                //     {
                //         var form = request.InputStream;
                //         var reader = new System.IO.StreamReader(form);
                //         var body = reader.ReadToEnd();
                //         var user = JsonSerializer.Deserialize<User>(body);
                //         if (user == null)
                //         {
                //             response.StatusCode = 404;
                //             continue;
                //         }
                //         user.Password = BCryptHelper.HashPassword(user.Password, BCryptHelper.GenerateSalt());
                //         db.Users.Add(user);
                //         db.SaveChanges();
                //         response.StatusCode = 201;
                //     }
                // }else if (request.Url.AbsolutePath == "/login" && request.HttpMethod == "POST")
                // {
                //     using (var db = new AppDbContext())
                //     {
                //         var form = request.InputStream;
                //         var reader = new System.IO.StreamReader(form);
                //         var body = reader.ReadToEnd();
                //         var user = JsonSerializer.Deserialize<User>(body);
                //         if (user == null)
                //         {
                //             response.StatusCode = 404;
                //             continue;
                //         }
                //         bool isPasswordValid = false;
                //         var userFromDb = db.Users.FirstOrDefault(u => u.Id == user.Id);
                //         if (userFromDb != null)
                //         {
                //             isPasswordValid = BCryptHelper.CheckPassword(user.Password, userFromDb.Password);
                //             response.StatusCode = isPasswordValid ? 201 : 404;
                //             continue;
                //         }
                //     }
                // } else
                else {
                    responseString = "<html><body>404 Not Found</body></html>";
                    response.StatusCode = 404;
                    continue;
                }
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);   
                output.Close();
            } 
        }
        
    }
}

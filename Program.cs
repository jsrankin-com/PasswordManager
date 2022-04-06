using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;



namespace PasswordManager
{
    class Program
    {
        private string jsonFile = @"C:\Users\Jeff.Rankin\Documents\. old pc\PasswordManager\PasswordManager\user.json";

        private void NewAccount()
        {
            Account acc = new Account();

            Console.WriteLine("Please enter the following information...\n");
            Console.Write("Description:\t\t");
            acc.description = Console.ReadLine();
            Console.Write("User ID:\t\t");
            acc.userid = Console.ReadLine();
            Console.Write("Password:\t\t");
            acc.password = Console.ReadLine();
            Console.Write("Login URL:\t\t");
            acc.loginurl = Console.ReadLine();
            Console.Write("Account #:\t\t");
            acc.accountnum = Console.ReadLine();

            Console.WriteLine("Commit account to the database? y/n");
            string option = Console.ReadLine().ToUpper();

            if (option == "Y")
                AddAccount(acc);
            else
                Main(null);
        }

        private void AddAccount(Account acc)
        {
            Console.WriteLine("Enter Company ID : ");
            var companyId = Console.ReadLine();
            Console.WriteLine("\nEnter Company Name : ");
            var companyName = Console.ReadLine();

            var newCompanyMember = "{ 'companyid': " + companyId + ", 'companyname': '" + companyName + "'}";
            try
            {
                var json = File.ReadAllText(jsonFile);
                var jsonObj = JObject.Parse(json);
                var experienceArrary = jsonObj.GetValue("accounts") as JArray;
                var newCompany = JObject.Parse(newCompanyMember);
                experienceArrary.Add(newCompany);

                jsonObj["accounts"] = experienceArrary;
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);
                Console.WriteLine("Add was successful! Waiting 3 seconds...\n");
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error : " + ex.Message.ToString());
            }
        }

        private void UpdateCompany()
        {
            string json = File.ReadAllText(jsonFile);

            try
            {
                var jObject = JObject.Parse(json);
                JArray experiencesArrary = (JArray)jObject["accounts"];
                Console.Write("Enter Company ID to Update Company : ");
                var companyId = Convert.ToInt32(Console.ReadLine());

                if (companyId > 0)
                {
                    Console.Write("Enter new company name : ");
                    var companyName = Convert.ToString(Console.ReadLine());

                    foreach (var company in experiencesArrary.Where(obj => obj["companyid"].Value<int>() == companyId))
                    {
                        company["companyname"] = !string.IsNullOrEmpty(companyName) ? companyName : "";
                    }

                    jObject["accounts"] = experiencesArrary;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(jsonFile, output);
                }
                else
                {
                    Console.Write("Invalid Company ID, Try Again!");
                    UpdateCompany();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Update Error : " + ex.Message.ToString());
            }
        }

        private void DeleteCompany()
        {
            var json = File.ReadAllText(jsonFile);
            try
            {
                var jObject = JObject.Parse(json);
                JArray experiencesArrary = (JArray)jObject["accounts"];
                Console.Write("Enter Company ID to Delete Company : ");
                var companyId = Convert.ToInt32(Console.ReadLine());

                if (companyId > 0)
                {
                    var companyName = string.Empty;
                    var companyToDeleted = experiencesArrary.FirstOrDefault(obj => obj["companyid"].Value<int>() == companyId);

                    experiencesArrary.Remove(companyToDeleted);

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(jsonFile, output);
                }
                else
                {
                    Console.Write("Invalid Company ID, Try Again!");
                    UpdateCompany();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void GetUserDetails()
        {
            var json = File.ReadAllText(jsonFile);
            try
            {
                var jObject = JObject.Parse(json);

                if (jObject != null)
                {
                    Console.WriteLine("ID :" + jObject["id"].ToString());
                    Console.WriteLine("Name :" + jObject["name"].ToString());

                    var address = jObject["address"];
                    Console.WriteLine("Street :" + address["street"].ToString());
                    Console.WriteLine("City :" + address["city"].ToString());
                    Console.WriteLine("Zipcode :" + address["zipcode"]);
                    JArray experiencesArrary = (JArray)jObject["accounts"];
                    if (experiencesArrary != null)
                    {
                        int i = 0;
                        foreach (var item in experiencesArrary)
                        {
                            Console.WriteLine(++i);
                            Console.WriteLine("company Id :" + item["companyid"]);
                            Console.WriteLine("company Name :" + item["companyname"].ToString());
                        }

                    }
                    Console.WriteLine("Phone Number :" + jObject["phoneNumber"].ToString());
                    Console.WriteLine("Role :" + jObject["role"].ToString());

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PrintAccounts()
        {
            Console.WriteLine("Account Entries");
            var json = File.ReadAllText(jsonFile);
            try
            {
                var jObject = JObject.Parse(json);

                if (jObject != null)
                {
                    JArray experiencesArrary = (JArray)jObject["accounts"];

                    if (experiencesArrary != null)
                    {
                        int index = 0;
                        foreach (var item in experiencesArrary)
                            Console.WriteLine($"{++index}. {item["companyname"].ToString()}");
                    }
                    else
                    {
                        Console.WriteLine("No Account Entries\n");
                    }
                    Console.Write('\n');
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private Account PrintAccount(int index)
        {
            var json = File.ReadAllText(jsonFile);

            var acc = new Account();

            try
            {
                var jObject = JObject.Parse(json);

                if (jObject != null)
                {
                    JArray experiencesArrary = (JArray)jObject["accounts"];

                    if (experiencesArrary != null)
                    {
                        if (index < 1)
                            Console.WriteLine("Index must be greater than 0!");
                        else if (experiencesArrary.Count() < index)
                            Console.WriteLine("Index is greater than the number of accounts!");
                        else
                            acc.description = experiencesArrary[--index]["companyname"].ToString();
                    }
                    else
                    {
                        Console.WriteLine("No Account Entries\n");
                    }
                    Console.Write('\n');
                }
            }
            catch (Exception)
            {
                throw;
            }
            return acc;
        }


        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Program objProgram = new PasswordManager.Program();
                //Console.WriteLine("Password Management System \n");
                //Console.WriteLine("INFO-3139\nJeffrey Rankin\nJune 2019\n");
                objProgram.PrintAccounts();

                Console.WriteLine("Enter # from list above.\nEnter A to add a new entry\nEnter X to quit.\n");
                string option = Console.ReadLine();

                bool isNumeric = int.TryParse(option, out int n);
                Account acc = null;
                if (isNumeric)
                {
                    acc = objProgram.PrintAccount(Convert.ToInt32(option));
                    Console.WriteLine(acc.description);
                    Console.ReadLine();
                }
                else
                {
                    switch (option.ToUpper())
                    {
                        case "A":
                            objProgram.NewAccount();
                            break;
                        case "X":
                            Environment.Exit(0);
                            break;
                        default:
                            Main(null);
                            break;
                    }
                }
            }


        }
    }
}

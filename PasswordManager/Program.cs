using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;




namespace PasswordManager
{
    class Program
    {
        private string JSONFILE = @"C:\Users\Jeff.Rankin\Documents\. old pc\PasswordManager\PasswordManager\user.json";

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
            string strJson = JsonConvert.SerializeObject(acc);

            try
            {
                var json = File.ReadAllText(JSONFILE);
                var jsonObj = JObject.Parse(json);
                var experienceArrary = jsonObj.GetValue("accounts") as JArray;
                var newCompany = JObject.Parse(strJson);
                experienceArrary.Add(newCompany);

                jsonObj["accounts"] = experienceArrary;
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(JSONFILE, newJsonResult);
                Console.WriteLine("Add was successful! Waiting 3 seconds...\n");
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error : " + ex.Message.ToString());
            }
        }

        private void EditAccount()
        {
            try
            {
                var json = File.ReadAllText(JSONFILE);
                var jsonObj = JObject.Parse(json);
                var experienceArrary = jsonObj.GetValue("accounts") as JArray;
                Console.WriteLine(experienceArrary);
                //var newCompany = JObject.Parse(strJson);
                //experienceArrary.Add(newCompany);

                jsonObj["accounts"] = experienceArrary;
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                //File.WriteAllText(JSONFILE, newJsonResult);
                Console.WriteLine("Add was successful! Waiting 3 seconds...\n");
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error : " + ex.Message.ToString());
            }
        }

        private void DeleteCompany()
        {
            var json = File.ReadAllText(JSONFILE);
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
                    File.WriteAllText(JSONFILE, output);
                }
                else
                {
                    Console.Write("Invalid Company ID, Try Again!");
                    DeleteCompany();
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
            var json = File.ReadAllText(JSONFILE);
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
                            Console.WriteLine($"[{++index}] {item["description"].ToString()}");
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


        private Account SelectAccount(int index)
        {
            var json = File.ReadAllText(JSONFILE);

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
                        {
                            acc = JsonConvert.DeserializeObject<Account>(experiencesArrary[index - 1].ToString());


                            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(acc))
                            {
                                string name = descriptor.Name;
                                object value = descriptor.GetValue(acc);
                                Console.WriteLine($"{name}: {value}");
                            }

                            Console.WriteLine("\nEnter D to delete\nEnter E to edit");
                            string option = Console.ReadLine();

                            switch (option.ToUpper())
                            {
                                case "D":
                                    //objProgram.EditAccount();
                                    break;
                                case "E":
                                    int i = 0;
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(acc))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(acc);
                                        Console.WriteLine($"[{++i}] {name}: {value}");
                                    }
                                    Console.WriteLine("\nEnter # to edit");
                                    string property = Console.ReadLine();

                                    PropertyDescriptor d = TypeDescriptor.GetProperties(acc)[Convert.ToInt32(property) - 1];
                                    acc.GetType().GetProperty(d.Name).SetValue(acc, "123");
                                    //Environment.Exit(0);

                                    jObject["accounts"][index - 1] = JObject.Parse(JsonConvert.SerializeObject(acc));
                                    string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                                    File.WriteAllText(JSONFILE, newJsonResult);
                                    Console.WriteLine("Edit was successful! Waiting 3 seconds...\n");
                                    System.Threading.Thread.Sleep(3000);
                                    SelectAccount(index);
                                    break;
                                default:
                                    Main(null);
                                    break;
                            }

                            //var companyToDeleted = experiencesArrary.FirstOrDefault(obj => obj["companyid"].Value<int>() == companyId);

                            //experiencesArrary.Remove(companyToDeleted);

                        }

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
                objProgram.PrintAccounts();

                Console.WriteLine("Enter # from list above.\nEnter A to add a new entry\nEnter E for edit\nEnter X to quit.\n");
                string option = Console.ReadLine();

                bool isNumeric = int.TryParse(option, out int n);
                Account acc = null;
                if (isNumeric)
                {
                    acc = objProgram.SelectAccount(Convert.ToInt32(option));

                }
                else
                {
                    switch (option.ToUpper())
                    {
                        case "A":
                            objProgram.NewAccount();
                            break;
                        case "E":
                            objProgram.EditAccount();
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

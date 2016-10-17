using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ho_MinhTri_HW6.DAL;
using Ho_MinhTri_HW6.Models;

namespace Ho_MinhTri_HW6.Controllers
{
    public enum Gender { All, Male, Female }
    public enum ComparativeSales { Greater, Less }

    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Home
        public ActionResult Index(String SearchString)
        {
            ViewBag.NumberOfCustomers = db.Customers.Count();
            List<Customer> SelectedCustomers = new List<Customer>();

            //************************************************************************************
            //TODO: Code for textbox searching (textbox contains a string
            //SearchString is the string from the first textbox
            if (SearchString == null || SearchString == "") //they didn't select anything
            {
                ViewBag.SearchString = "Search string was null";

                SelectedCustomers = db.Customers.ToList();
            }
            else //they picked something
            {
                ViewBag.SearchString = "The search string is " + SearchString;

                SelectedCustomers = db.Customers.Where(c => c.FirstName.Contains(SearchString) || c.LastName.Contains(SearchString)).ToList();
            }
            //*************************************************************************************

            ViewBag.NumberofSelectedCustomers = SelectedCustomers.Count();
            SelectedCustomers.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.AverageSale);
            return View(SelectedCustomers);
        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllFrequencies = GetAllFrequencies();

            return View();
        }

        public ActionResult SearchResults(String SearchString, Int16 SelectedFrequency, Gender SelectedGender, String AverageSales, ComparativeSales SelectedSales)
        {
            ViewBag.NumberOfCustomers = db.Customers.Count();
            var query = from c in db.Customers
                        select c;

            //************************************************************************************
            //TODO: Code for textbox searching (textbox contains a string
            //SearchString is the string from the first textbox
            if (SearchString == null || SearchString == "") //they didn't select anything
            {
                ViewBag.NameSearch = "Search string was null";
            }
            else //they picked something
            {
                ViewBag.NameSearch = "The search string is " + SearchString;

                query = query.Where(c => c.FirstName.Contains(SearchString) || c.LastName.Contains(SearchString));
            }
            //*************************************************************************************

            //********************************************************************************************************
            //TODO: Code for drop-down list
            //Selected frequency is the selected value from the dropdown
            if (SelectedFrequency == 0) // they chose "all frequencies" from the drop-down
            {
                ViewBag.SelectedFrequency = "No frequency was selected";
            }
            else //frequency was chosen
            {
                switch (SelectedFrequency)
                {
                    case 1:
                        query = query.Where(c => c.Frequency.Name == "Daily");
                        break;
                    case 2:
                        query = query.Where(c => c.Frequency.Name == "Monthly");
                        break;
                    case 3:
                        query = query.Where(c => c.Frequency.Name == "Never");
                        break;
                    case 4:
                        query = query.Where(c => c.Frequency.Name == "Often");
                        break;
                    case 5:
                        query = query.Where(c => c.Frequency.Name == "Once");
                        break;
                    case 6:
                        query = query.Where(c => c.Frequency.Name == "Seldom");
                        break;
                    case 7:
                        query = query.Where(c => c.Frequency.Name == "Weekly");
                        break;
                    case 8:
                        query = query.Where(c => c.Frequency.Name == "Yearly");
                        break;
                    case 9:
                        query = query.Where(c => c.Frequency.Name == "Not Used");
                        break;
                    default:
                        break;
                }

                ViewBag.SelectedFrequency = "The selected frequency is " + SelectedFrequency;
            }
            //*********************************************************************************************************

            //*************************************************************************************
            //TODO: Code for radio buttons
            //Figure out selected class
            switch (SelectedGender)
            {
                case Gender.All:
                    ViewBag.SelectedClassification = "The selected gender is All";
                    break;
                case Gender.Male:
                    ViewBag.SelectedClassfication = "The selected gender is Male";
                    query = query.Where(c => c.Gender == "Male");
                    break;
                case Gender.Female:
                    ViewBag.SelectedClassfication = "The selected gender is Female";
                    query = query.Where(c => c.Gender == "Female");
                    break;
                default:
                    ViewBag.SelectedClassification = "No gender selected";
                    break;
            }
            //*****************************************************************************************

            //*********************************************************************************************************
            //TODO: Code for textbox with numeric input
            //see if they specified something for GPA
            if (AverageSales != null && AverageSales != "")
            //make sure string is a valid number
            {
                Decimal decAverageSales;
                try
                {
                    decAverageSales = Convert.ToDecimal(AverageSales);

                    //*************************************************************************************
                    //TODO: Code for radio buttons
                    //Figure out selected class
                    switch (SelectedSales)
                    {
                        case ComparativeSales.Greater:
                            ViewBag.SelectedClassification = "The selected sales is Greater";
                            query = query.Where(c => c.AverageSale >= decAverageSales);
                            break;
                        case ComparativeSales.Less:
                            ViewBag.SelectedClassfication = "The selected sales is Less";
                            query = query.Where(c => c.AverageSale <= decAverageSales);
                            break;
                        default:
                            ViewBag.SelectedClassification = "No sales selected";
                            break;
                    }
                    //*****************************************************************************************
                }
                catch  //this code will display when something is wrong
                {
                    //Add a message for the viewbag
                    ViewBag.Message = AverageSales + "is not valid number. Please try again";

                    //Send user back to home page
                    return View("Index");
                }

                //Add value to ViewBag
                ViewBag.SalesSearch = "The average sales selected is " + decAverageSales.ToString("n2");
            }
            else  //they didn't specify Average Sales
            {
                ViewBag.SalesSearch = "No Average Sales was specified";
            }
            //********************************************************************************************************

            //Sort the data
            query = query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.AverageSale);

            //Execute the query
            List<Customer> SelectedCustomers = query.ToList();

            //Count the number of results
            ViewBag.NumberofSelectedCustomers = SelectedCustomers.Count();

            return View("Index", SelectedCustomers);
        }

        public SelectList GetAllFrequencies()
        {
            var query = from c in db.Customers
                        orderby c.Frequency.Name
                        select c.Frequency;
            List<Frequency> CustomerList = query.Distinct().ToList();

            //Add in choice for not selecting a frequency
            Frequency NoChoice = new Frequency() { FrequencyID = 0, Name = "All Frequencies" };
            CustomerList.Add(NoChoice);
            SelectList FrequencyList = new SelectList(CustomerList.OrderBy(f => f.Name), "FrequencyID", "Name");
            return FrequencyList;
        }
    }
}
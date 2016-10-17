using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ho_MinhTri_HW6.DAL;
using Ho_MinhTri_HW6.Models;

namespace Ho_MinhTri_HW6.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public enum Gender { All, Male, Female}
        public enum ComparativeSales { Greater, Less}

        // GET: Home
        public ActionResult Index(String SearchString)
        {
            //************************************************************************************
            //TODO: Code for textbox searching (textbox contains a string
            //SearchString is the string from the first textbox
            if (SearchString == null || SearchString == "") //they didn't select anything
            {
                ViewBag.SearchString = "Search string was null";
                return View(db.Customers.ToList());
            }
            else //they picked something
            {
                ViewBag.SearchString = "The search string is " + SearchString;
                return View();
            }
            //*************************************************************************************
        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllFrequencies = GetAllFrequencies();

            return View();
        }

        public ActionResult SearchResults(String SearchString, Frequency SelectedFrequency, Gender SelectedGender, String AverageSales, ComparativeSales SelectedSales)
        {
            //************************************************************************************
            //TODO: Code for textbox searching (textbox contains a string
            //SearchString is the string from the first textbox
            if (SearchString == null || SearchString == "") //they didn't select anything
            {
                ViewBag.SearchString = "Search string was null";
            }
            else //they picked something
            {
                ViewBag.SearchString = "The search string is " + SearchString;
            }
            //*************************************************************************************

            //********************************************************************************************************
            //TODO: Code for drop-down list
            //Selected frequency is the selected value from the dropdown
            if (SelectedFrequency.FrequencyID == 0) // they chose "all frequencies" from the drop-down
            {
                ViewBag.SelectedFrequency = "No frequency was selected";
            }
            else //frequency was chosen
            {
                //List<Month> AllMonths = MonthUtilities.GetMonths();
                //Month MonthToDisplay = AllMonths.Find(m => m.MonthID == SelectedMonth);
                ViewBag.SelectedFrequency = "The selected frequency is " + SelectedFrequency.Name;
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
                    break;
                case Gender.Female:
                    ViewBag.SelectedClassfication = "The selected gender is Female";
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
                }
                catch  //this code will display when something is wrong
                {
                    //Add a message for the viewbag
                    ViewBag.Message = AverageSales + "is not valid number. Please try again";

                    //Re-populate dropdown
                    //ViewBag.AllMonths = GetAllMonths();
                    //ViewBag.AllDays = GetAllDays();

                    //Send user back to home page
                    return View("Index");
                }


                //Do some math with this number to prove it's a number
                //decGPA += 100;  //this is a stupid thing to do; 
                                //You wouldn't want to do it in real life. 
                                //I'm just showing you that it is a number
                                //and not a string.

                //Add value to ViewBag
                //ViewBag.UpdatedGPA = "The updated GPA is " + decGPA.ToString("n2");
            }
            else  //they didn't specify GPA
            {
                ViewBag.Message = "No Average Sales was specified";
            }
            //*********************************************************************************************************

            //*************************************************************************************
            //TODO: Code for radio buttons
            //Figure out selected class
            switch (SelectedSales)
            {
                case ComparativeSales.Greater:
                    ViewBag.SelectedClassification = "The selected sales is Greater";
                    break;
                case ComparativeSales.Less:
                    ViewBag.SelectedClassfication = "The selected sales is Less";
                    break;
                default:
                    ViewBag.SelectedClassification = "No sales selected";
                    break;
            }
            //*****************************************************************************************

            return View();
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
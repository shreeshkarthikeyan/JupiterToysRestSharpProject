using JupiterToysRestSharpProject.API;
using JupiterToysRestSharpProject.Model;
using JupiterToysRestSharpProject.Support;
using Newtonsoft.Json;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;

namespace JupiterToysRestSharpProject.StepDefinitions
{
    [Binding]
    public class ToysStepDefinitions
    {
        ToyAPI toyObj;
        UserAPI userObj;
        ScenarioContext scenarioContext;
        FeatureContext featureContext;
        public ToysStepDefinitions(ToyAPI toyObj, UserAPI userObj, ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            this.toyObj = toyObj;
            this.userObj = userObj;
            this.scenarioContext = scenarioContext;
            this.featureContext = featureContext;
        }
        [Given(@"the user creates a new toy")]
        public void GivenTheUserCreatesANewToy(Table table)
        {
            var toyDetails = table.CreateInstance<Toy>();
            toyDetails.Links = new List<Link> { new Link { } };
            var toyId = toyObj.PerformCreateToyOperation(toyDetails);
            toyDetails.Id = Int32.Parse(toyId);
            toyDetails.Links.ElementAt(0).Rel = "self";
            toyDetails.Links.ElementAt(0).Href = $"{Config.readFromPropertiesFile("baseurl")}/toy/{toyId}";
            featureContext.Add("Toy", toyDetails);
        }

        [Given(@"the user updates the stock of the toy to zero")]
        public void GivenTheUserUpdatesTheStockOfTheToyToZero()
        {
            /*Toy updateStockCount = new Toy
            {
                Stock = 0
            };*/
            string payload = @"{
                                ""stock"" : 0
                                }";
            toyObj.UpdateToyStock(featureContext.Get<Toy>("Toy").Id.ToString(), payload);
        }


        [Then(@"the user checks the toy is present in the list")]
        public void ThenTheUserChecksTheToyIsPresentInTheList()
        {
            var sctoy = featureContext.Get<Toy>("Toy");
            Console.WriteLine($"Scenario Context: {sctoy.Id}");
            Toy toy = toyObj.PerformGetToyOperation(featureContext.Get<Toy>("Toy").Id.ToString());
            bool isEqual = featureContext.Get<Toy>("Toy").Equals(toy);
            Console.WriteLine($"Is Toy equal: {isEqual}");
        }

        [Then(@"the user deletes the toy")]
        public void ThenTheUserDeletesTheToy()
        {
            var expectedText = $"Toy with id {featureContext.Get<Toy>("Toy").Id} deleted successfully";
            Assert.AreEqual(expectedText, toyObj.PerformDeleteToyOperation(featureContext.Get<Toy>("Toy").Id.ToString()), "Delete operation failed");
        }

        [Given(@"the user creates a customer account")]
        public void GivenTheUserCreatesACustomerAccount()
        {
            var customerDetails = new Customer()
            {
                Id = 0,
                Username = "FitzShield1@gmail",
                Firstname = "Fitz",
                Lastname = "Patrick",
                Gender = "Male",
                PhoneNumber = "0456314971",
                Addresses = new List<Address>
                {
                    new Address
                    {
                        Id = 0,
                        Line1 = "2, Coppin Close",
                        Line2 = "",
                        City = "Hampton Park",
                        Postcode = "3976",
                        State = "VIC",
                        AddressType = "Postal",
                        DeliveryName ="Fitz",
                    }
                },
                TransactionHistory = new List<TransactionHistory>()
            };
            var customerId = userObj.performCreateUserOperation(customerDetails);
            customerDetails.Id = Int32.Parse(customerId);
            scenarioContext.Add("Customer", customerDetails);
            featureContext.Add("Customer", customerDetails);
        }

        [Then(@"the user adds toys to the cart")]
        public void ThenTheUserAddsToysToTheCart()
        {
            List<TransactionItem> item = new List<TransactionItem>
            {
                new TransactionItem
                {
                    Id = 0,
                    ToyDetails = toyObj.PerformGetToyOperation(featureContext.Get<Toy>("Toy").Id.ToString()), // 
                    NumberOfToys = 2,
                    Status = "OK"
                }
            };
            scenarioContext.Get<Customer>("Customer").TransactionHistory = new List<TransactionHistory>
            {
                new TransactionHistory
                {
                    Id = 0,
                    TransactionItems = item,
                    Date = DateTime.Now.ToString("dd/MM/yyyy"),
                    Cost = item.Sum(x => x.ToyDetails.Price * x.NumberOfToys),
                    OrderNumber = ""
                }
            };
            var list = userObj.addToysToCartOperation(scenarioContext.Get<Customer>("Customer").Id.ToString(), scenarioContext.Get<Customer>("Customer").TransactionHistory.ElementAt(0));
            var transactionId = list.Find(x => x.Contains("Transaction Id")).Split("->")[1].Trim();
            scenarioContext.Get<Customer>("Customer").TransactionHistory.ElementAt(0).Id = Int32.Parse(transactionId);
            var orderNumber = list.Find(x => x.Contains("Order Number")).Split("->")[1].Trim();
            scenarioContext.Get<Customer>("Customer").TransactionHistory.ElementAt(0).OrderNumber = orderNumber;
        }

        [Then(@"the user updates the transaction details")]
        public void ThenTheUserUpdatesTheTransactionDetails()
        {
            TransactionHistory paymentStatusUpdate = new TransactionHistory
            {
                PaymentStatus = "Successful"
            };
            userObj.UpdatePaymentStatus(scenarioContext.Get<Customer>("Customer").TransactionHistory.ElementAt(0).Id.ToString(), paymentStatusUpdate);
            
        }

        [Given(@"the user deletes the customer account")]
        public void GivenTheUserDeletesTheCustomerAccount()
        {
            if(!userObj.DeleteCustomer(featureContext.Get<Customer>("Customer").Id.ToString()).Equals("true"))
                throw new Exception("Delete customer operation failed");
        }

    }
}

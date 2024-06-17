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
            if(!(toyObj.UpdateToyStock(featureContext.Get<Toy>("Toy").Id.ToString(), payload) == 0))
                throw new Exception("Update toy stock operation failed");
        }


        [Then(@"the user checks the toy is present in the list")]
        public void ThenTheUserChecksTheToyIsPresentInTheList()
        {
            var sctoy = featureContext.Get<Toy>("Toy");
            Console.WriteLine($"Scenario Context: {sctoy.Id}");
            Toy toy = toyObj.PerformGetToyOperation(featureContext.Get<Toy>("Toy").Id.ToString());
            //bool isEqual = featureContext.Get<Toy>("Toy").Equals(toy);
            //Console.WriteLine($"Is Toy equal: {isEqual}");

            var featureContextToy = featureContext.Get<Toy>("Toy");
            featureContextToy.Id.Should().Be(toy.Id);
            featureContextToy.Price.Should().Be(toy.Price);
            featureContextToy.Category.Should().Be(toy.Category);
            featureContextToy.Title.Should().Be(toy.Title);
            featureContextToy.Size.Should().Be(toy.Size);
            featureContextToy.Image.Should().Be(toy.Image);
            featureContextToy.Stock.Should().Be(toy.Stock);
            featureContextToy.Links.ElementAt(0).Rel.Should().Be(toy.Links[0].Rel);
            featureContextToy.Links.ElementAt(0).Href.Should().Be(toy.Links[0].Href);

        }

        [Then(@"the user deletes the toy")]
        public void ThenTheUserDeletesTheToy()
        {
            var expectedText = $"Toy with id {featureContext.Get<Toy>("Toy").Id} deleted successfully";
            Assert.AreEqual(expectedText, toyObj.PerformDeleteToyOperation(featureContext.Get<Toy>("Toy").Id.ToString()), "Delete operation failed");
        }

        [Given(@"the user creates a customer account")]
        public void GivenTheUserCreatesACustomerAccount(Table table)
        {
            var customerDetails = table.CreateInstance<Customer>();
            customerDetails.Addresses = new List<Address>();
            customerDetails.TransactionHistory = new List<TransactionHistory>();

            var customerId = userObj.performCreateUserOperation(customerDetails);
            customerDetails.Id = Int32.Parse(customerId);
            scenarioContext.Add("Customer", customerDetails);
            featureContext.Add("Customer", customerDetails);
        }

        [Given(@"the user updates the address of the customer")]
        public void GivenTheUserUpdatesTheAddressOfTheCustomer(Table table)
        {
            var address = table.CreateInstance<Address>();

            scenarioContext.Get<Customer>("Customer").Addresses = new List<Address> { address };
            Customer responseData = userObj.UpdateCustomerAddress(scenarioContext.Get<Customer>("Customer").Id.ToString(), scenarioContext.Get<Customer>("Customer"));

            var scenarioContextToy = scenarioContext.Get<Customer>("Customer");
            scenarioContextToy.Id.Should().Be(responseData.Id);
            scenarioContextToy.Username.Should().Be(responseData.Username);
            scenarioContextToy.Firstname.Should().Be(responseData.Firstname);
            scenarioContextToy.Lastname.Should().Be(responseData.Lastname);
            scenarioContextToy.Gender.Should().Be(responseData.Gender);
            scenarioContextToy.PhoneNumber.Should().Be(responseData.PhoneNumber);
            scenarioContextToy.Addresses.ElementAt(0).Line1.Should().Be(responseData.Addresses[0].Line1);
            scenarioContextToy.Addresses.ElementAt(0).Line2.Should().Be(responseData.Addresses[0].Line2);
            scenarioContextToy.Addresses.ElementAt(0).City.Should().Be(responseData.Addresses[0].City);
            scenarioContextToy.Addresses.ElementAt(0).Postcode.Should().Be(responseData.Addresses[0].Postcode);
            scenarioContextToy.Addresses.ElementAt(0).State.Should().Be(responseData.Addresses[0].State);
            scenarioContextToy.Addresses.ElementAt(0).AddressType.Should().Be(responseData.Addresses[0].AddressType);
            scenarioContextToy.Addresses.ElementAt(0).DeliveryName.Should().Be(responseData.Addresses[0].DeliveryName);
        }


        [Then(@"the user adds toys to the cart")]
        public void ThenTheUserAddsToysToTheCart(Table table)
        {
            List<TransactionItem> item = new List<TransactionItem>();
            var toys = table.CreateSet<(string toyName, int quantity)>();
            for (int i = 0; i < toys.Count(); i++) {
                Console.WriteLine($"{toys.ElementAt(i).toyName}'s quantity - {toys.ElementAt(i).quantity}");
                var transactionItem = new TransactionItem
                {
                    Id = 0,
                    ToyDetails = toyObj.PerformGetToyByNameOperation(toys.ElementAt(i).toyName),
                    NumberOfToys = toys.ElementAt(i).quantity,
                    Status = "OK",
                };
                item.Add(transactionItem);
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
            var transactionId = list.Find(x => x.Contains("Transaction Id"))?.Split("->")[1].Trim();
            scenarioContext.Get<Customer>("Customer").TransactionHistory.ElementAt(0).Id = Int32.Parse(transactionId!);
            var orderNumber = list.Find(x => x.Contains("Order Number"))?.Split("->")[1].Trim();
            scenarioContext.Get<Customer>("Customer").TransactionHistory.ElementAt(0).OrderNumber = orderNumber!;
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

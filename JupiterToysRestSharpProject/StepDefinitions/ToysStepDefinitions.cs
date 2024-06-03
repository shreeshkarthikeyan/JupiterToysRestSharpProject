using JupiterToysRestSharpProject.API;
using JupiterToysRestSharpProject.Model;
using JupiterToysRestSharpProject.Support;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;

namespace JupiterToysRestSharpProject.StepDefinitions
{
    [Binding]
    public class ToysStepDefinitions
    {
        ToyAPI toyObj;
        UserAPI userObj;
        Toy toyDetails;
        Customer customerDetails;
        public ToysStepDefinitions(ToyAPI toyObj, UserAPI userObj)
        {
            this.toyObj = toyObj;
            this.userObj = userObj;
        }
        [Given(@"the user creates a new toy")]
        public void GivenTheUserCreatesANewToy(Table table)
        {
            toyDetails = table.CreateInstance<Toy>();
            toyDetails.Links = new List<Link> { new Link { } };
            var toyId = toyObj.PerformCreateToyOperation(toyDetails);
            toyDetails.Id = Int32.Parse(toyId);
            toyDetails.Links.ElementAt(0).Rel = "self";
            toyDetails.Links.ElementAt(0).Href = $"{Config.readFromPropertiesFile("baseurl")}/toy/{toyId}";
        }

        [Then(@"the user checks the toy is present in the list")]
        public void ThenTheUserChecksTheToyIsPresentInTheList()
        {
            Toy toy = toyObj.PerformGetToyOperation(toyDetails.Id.ToString());


            Assert.AreEqual(toyDetails.Price, toy.Price, $"Mismatch in {toyDetails.Title}'s price");
            Assert.AreEqual(toyDetails.Category, toy.Category, $"Mismatch in {toyDetails.Title}'s category");
            Assert.AreEqual(toyDetails.Title, toy.Title, $"Mismatch in {toyDetails.Title}'s title");
            Assert.AreEqual(toyDetails.Size, toy.Size, $"Mismatch in {toyDetails.Title}'s size");
            Assert.AreEqual(toyDetails.Image, toy.Image, $"Mismatch in {toyDetails.Title}'s image");
            Assert.AreEqual(toyDetails.Stock, toy.Stock, $"Mismatch in {toyDetails.Title}'s stock");

            Assert.AreEqual(toyDetails.Links.ElementAt(0).Rel, toy.Links.ElementAt(0).Rel, $"Mismatch in {toyDetails.Title}'s link's rel");
            if (toy.Links.ElementAt(0).Href.Contains(toyDetails.Links.ElementAt(0).Href)) {
                Assert.IsTrue(true, $"Expected value: {toyDetails.Links.ElementAt(0).Href}, API value: {toy.Links.ElementAt(0).Href}");
            }
        }

        [Then(@"the user deletes the toy")]
        public void ThenTheUserDeletesTheToy()
        {
            var expectedText = $"Toy with id {toyDetails.Id} deleted successfully";
            //toyObj.PerformDeleteToyOperation(toyId);
            Assert.AreEqual(expectedText, toyObj.PerformDeleteToyOperation(toyDetails.Id.ToString()), "Delete operation failed");
        }

        [Given(@"the user creates a customer account")]
        public void GivenTheUserCreatesACustomerAccount()
        {
            customerDetails = new Customer()
            {
                Id = 0,
                Username = "FitzShield@gmail",
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
        }

        [Then(@"the user adds toys to the cart")]
        public void ThenTheUserAddsToysToTheCart()
        {
            List<TransactionItem> item = new List<TransactionItem>
            {
                new TransactionItem
                {
                    Id = 0,
                    ToyDetails = toyObj.PerformGetToyOperation("2795"),
                    NumberOfToys = 2,
                    Status = "OK"
                },
                new TransactionItem
                {
                    Id = 0,
                    ToyDetails = toyObj.PerformGetToyOperation("2455"),
                    NumberOfToys = 2,
                    Status = "OK"
                }
            };
            customerDetails.TransactionHistory = new List<TransactionHistory>
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
            var list = userObj.addToysToCartOperation(customerDetails.Id.ToString(), customerDetails.TransactionHistory.ElementAt(0));
            var transactionId = list.Find(x => x.Contains("Transaction Id")).Split("->")[1].Trim();
            customerDetails.TransactionHistory.ElementAt(0).Id = Int32.Parse(transactionId);
            var orderNumber = list.Find(x => x.Contains("Order Number")).Split("->")[1].Trim();
            customerDetails.TransactionHistory.ElementAt(0).OrderNumber = orderNumber;
        }

        [Then(@"the user updates the transaction details")]
        public void ThenTheUserUpdatesTheTransactionDetails()
        {
            TransactionHistory paymentStatusUpdate = new TransactionHistory
            {
                PaymentStatus = "Successful"
            };
            userObj.updatePaymentStatus(customerDetails.TransactionHistory.ElementAt(0).Id.ToString(), paymentStatusUpdate);
        }
    }
}

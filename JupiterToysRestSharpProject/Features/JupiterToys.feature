Feature: JupiterToys

Jupiter Toys API Automation with hub scenarios

@Task1
Scenario: Verify create, view and delete toy
	Given the user creates a new toy
		| FieldName | FieldValue                                           |
		| price     | 18.99                                                |
		| category  | Soft toys                                            |
		| title     | Peppa pig                                            |
		| size      | Small                                                |
		| image     | https://www.pinterest.com.au/pin/766456430336669307/ |
		| stock     | 20                                                   |
	Then the user checks the toy is present in the list
	And the user deletes the toy

@Task2
Scenario: Verify toy purchase
	Given the user creates a customer account
	Then the user adds toys to the cart
	And the user updates the transaction details


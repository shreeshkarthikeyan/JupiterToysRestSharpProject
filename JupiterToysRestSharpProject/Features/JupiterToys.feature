Feature: JupiterToys

Jupiter Toys API Automation with hub scenarios

@Task1
Scenario: Scenario 1 - Verify create and view toy
	Given the user creates a new toy
		| FieldName | FieldValue                                           |
		| price     | 18.99                                                |
		| category  | Soft toys                                            |
		| title     | Peppa pig                                            |
		| size      | Small                                                |
		| image     | https://www.pinterest.com.au/pin/766456430336669307/ |
		| stock     | 20                                                   |
	Then the user checks the toy is present in the list

@Task2
Scenario: Scenario 2 - Verify purchasing a toy from a new customer account
	Given the user creates a customer account
	Then the user adds toys to the cart
	And the user updates the transaction details

@Task3
Scenario: Scenario 3 - Verify deleting customer and toy
	Given the user deletes the customer account
	# And the user updates the stock of the toy to zero
	Then the user deletes the toy
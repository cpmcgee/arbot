# arbot 
This is a framework built in C# that provides abstracted API access to several major cryptocurrency API's (currently working on Bittrex, Bitfinex, and Poloniex but more to come). The goal for this framework is to provide a convenient backbone for creating and implementing trading strategies that utilize multiple exchanges. This is a personal side project for fun, so limited time and effort is being put in. If you would like to contribute or make suggestions, submit a pull request or email me at cpmcgee95@gmail.com

Project Design:

Strategies are classes implementing the interface IStrategy, which has one method: Run(). 
  - Strategies are then run from the Main() method in Program.cs (e.g. new ExampleStrategy.Run()).
  - Strategies can be run in any order, compounded, or run in parallel from the main method
  - See TestStrategy.cs for a trivial strategy that simply compares prices of a list of coins

Access to price information and trading functionality is through the following exchange classes:
  - Bittrex.cs
  - Bittfinex.cs
  - Poloniex.cs
These classes can be constructed with no parameters, when being constructed they will load all their price information from their API's and add it to the currency manager.

The CurrencyManager contains all the currencies and their pricing information from all the exchanges, it can be set up to continuously call the APIs and update the prices in the background

API calls are handled by classes inherting from the Request.cs abstract class, which provides abstract methods for handling the http requests of the different APIs. 
  - Examples of classes conforming are BittrexRequest.cs and BitfinexRequest.cs with
  - Request classes for each exchange implement a method for each endpoint of their respective API, the dot notation of the       method calls will be very similar to the slash notation of the URL (e.g. new                                                   BittrexRequest().Public().GetMarketSummary("ltc") will be like visiting bittrex.com/api/v1.1/public/getmarketsummary?    market=btc-ltc (these methods are not overrides of abstract methods as APIs do not have identical endpoints)
    - These methods return dynamic JSON objects that will be parsed by their calling methods
 
 

Currently Working On:
  - Finishing implemenation and debugging of of all Bittrex, Bitfiniex, and Poloniex endpoints
  - Finish creating configuration provider
  - Add more exchanges
  - Implement Order object which handles open and historical orders
  - Test trading methods with small amounts of currency
 

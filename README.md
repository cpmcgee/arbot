(unfinished)
# arbot 
This was an attempt at a framework built in C# that provides abstracted API access to several major cryptocurrency API's (currently working on Bittrex, Bitfinex, and Poloniex but more to come). The goal for this framework is to provide a convenient backbone for creating and implementing trading strategies that utilize multiple exchanges. This is a personal side project for fun, so limited time and effort is being put in. If you would like to contribute or make suggestions, submit a pull request or email me at cpmcgee95@gmail.com


Project Design:

Strategies are classes implementing the interface IStrategy, which has one method, Run(). 
  - Strategies are then run from the Main() method in Program.cs (e.g. new ExampleStrategy.Run()).
  - Strategies can be run in any order, compounded, or run in parallel from the main method
  - See TestStrategy.cs for a trivial strategy that simply compares prices of a list of coins

Access to price information and trading functionality is through the following exchange classes:
  - Bittrex.cs
  - Bittfinex.cs
  - Poloniex.cs

The CurrencyManager manages all the currencies available through the exchanges; it holds information such as price, account balance, and the symbol string literal on each exchange in a dictionary of Currency objects. It continuously calls the APIs and updates the prices and balances in the background. With this we can have a dynamic list of available currencies immediately accessible at all times with up-to-date information.

API calls are handled by classes inherting from the Request.cs abstract class, which enforces methods for handling the http requests of the different APIs. 
  - Examples of classes conforming are BittrexRequest.cs and BitfinexRequest.cs 
  - Request classes for each exchange implement a method for each endpoint of their respective API, the dot notation of the       method calls will be very similar to the slash notation of the URL (e.g. new                                                   BittrexRequest().Public().GetMarketSummary("ltc") will be like visiting bittrex.com/api/v1.1/public/getmarketsummary?    market=btc-ltc (these methods are not overrides of abstract methods as APIs do not have identical endpoints)
    - These methods return dynamic JSON objects that will be parsed by their calling methods

The OrderManager and Order class function much like a more dynamic version of the CurrencyManager and Currency class. The OrderManager will keep track of all buy and sell orders placed on any exchange during the runtime of the program.
 
To use authenticated methods you will need to set up your config file and key files which will be processed by the Configuration and KeyLoader classes;


Currently Working On:
  - Implement unit test for exchange classes
  - Fix logging system
  - Test buy/sell/cancel/etc with money
  - Add kraken.io exchange
 

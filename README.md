# arbot 
This is a framework built in C# that provides abracted API access to several major cryptocurrency API's (currently working on Bittrex, Bitfinex, and Poloniex but more to come). 

Project Design:

API.cs abstract class - Provides abstract methods such as GetPriceInBtc(string symbol) for getting information from different APIs. Some classes conforming to this are Bittrex.cs and Poloniex.cs
  - Classes conforming to this class ensures implementations of different web API's all have the same methods and .               functionality
  - This make arbitrage strategies and other strategies using multiple API's much easier from Strategy classes 
  - Implementations of these methods make use of Request.cs methods (shown below) and parse the dynamic JSON data returned

Request.cs abstract class - Provides abstract methods for handling the REST calls of the different APIs. 
  - Examples of classes conforming are BittrexRequest.cs and BitfinexRequest.cs with
  - Request classes for each exchange implement a method for each endpoint of their respective API, the dot notation of the       method calls will be very similar to the slash notation of the URL (e.g. new                                                   BittrexRequest().Public().GetMarketSummary("ltc") will be like visiting bittrex.com/api/v1.1/public/getmarketsummary?         market=btc-ltc
  - Methods in these classes return dynamic JSON objects that will be parsed by their calling methods
 
IStrategy.cs interface - Provides a simple interface to implement when creating strategies, has one method, Run(), where the logic from the strategy is implemented.
  - Strategies are run from the Main() method in Program.cs (e.g. TestStrategy.Run())
  - Strategies can be run in any order, compounded, or run in parallel from the main method
  
  
  
  Currently Working On:
  - Finishing implemenation and debugging of of all Bittrex, Bitfiniex, and Poloniex endpoints
  - Finish creating configuration provider
  - Concurrent updating price tracking module
  - Add more exchanges
  - Add test project, unit tests, api tests, etc
 

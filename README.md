<p align="center">
    <img src="https://raw.githubusercontent.com/bmresearch/Solnet/master/assets/icon.png" margin="auto" height="175"/>
</p>

<div style="text-align:center">

<p>

# Introduction

SOL.BRIDGE is an ASP.NET solution that focuses on bridging a gap between developers and [Solnet](https://github.com/bmresearch/Solnet) library. It provides a production-ready ASP.NET template with all Solnet packages and usable examples, allowing you to get started with a blockchain server solution in no time.

</p>

</div>

## Features
- Production ready ASP.NET template
- Real World examples of API endpoints and services that give you a head start and understanding of how to use Solnet library and interact with Solana blockchain
- Swagger documentation for all API endpoints
- Fully customizable and extendable model of API controllers that show how to build and scale your server-side application with Solnet
- Extensive logging and error handling examples, no longer out-of-the-box solutions that you need to figure out how to use
- Ready to deploy to Azure, AWS, or any other cloud provider
- Extensive use of dependency injection and services that show how to build a scalable and maintainable solution
- Wide coverage of Solnet library features and examples, including Solana Wallets, Solana Key Store, Solana RPC calls, Solana Streaming RPC calls, Solana Programs, and more

## Getting Started

- Here's you will see how to get started with SOL.BRIDGE and how to run the solution on your local machine.
It's pretty standard ASP.NET solution, so you can run it in Visual Studio or Visual Studio Code.
- Just clone a repository and then open a solution, build it, and run it. You can also run it from the command line using `dotnet run` command.
- All config files, dependencies, and settings are already set up for you, so you don't need to worry about it.
- Once you open it in the IDE, IDE will find run config files and you will be able to run HTTP/HTTPS server on your local machine with a single click.
- Swagger is enabled by default, so you can see all example API endpoints and interact with them in the browser straight away.

## Solution Structure Overview

### Controllers

- This is where all API controllers are located. Each controller is responsible for a specific part of the application and has a set of API endpoints that can be called from the client-side application.
We have divided them by the Solnet packages they are using, so you can easily find the one you are looking for.
- They are making first validation stage of the data and then passing it to the data controllers.
- Also they return the data to the client-side application from the data controllers.
- The result of the API call also get handled here, so you can see how to handle different types of responses and errors.

Example of API controller:

```c#
[HttpPost, Route("encrypt-account-data")]
public ActionResult<string> EncryptAccountData([FromBody] EncryptAccountPayload payload)
{
    if (string.IsNullOrEmpty(payload.Password))
    {
        return StatusCode(400, "Password is empty");
    }

    if (string.IsNullOrEmpty(payload.AccountData))
    {
        return StatusCode(400, "Account Data is empty");
    }

    if (string.IsNullOrEmpty(payload.PublicKey))
    {
        return StatusCode(400, "Public Key is empty");
    }

    var status = _solnetKeystoreController.EncryptAccountData(payload);
    return status.result ? StatusCode(200, status.text) : StatusCode(500, status.text);
}
```

### Data Controllers

- This is where all data controllers are located. Data controllers are responsible for handling data and making sure that it is stored and retrieved correctly. They are using services to interact with the database and other services.
- They are handling all business logic and data manipulation.
- Of course you can make separate structure to handle I/O operations, but for the sake of simplicity we have made them data controllers as well.
- They are returning the data to the API controllers, so they can return it to the client-side application.

Example of Data controller:

```c#
public (bool result, string text) EncryptAccountData(EncryptAccountPayload payload)
{
    var accountDataBytes = Encoding.UTF8.GetBytes(payload.AccountData);
    // Encrypt a private key, seed or mnemonic associated with a certain address
    var jsonString = _secretKeyStoreService.EncryptAndGenerateDefaultKeyStoreAsJson(
        payload.Password,
        accountDataBytes,
        payload.PublicKey
    );

    // here you can save encrypted data to a file or database

    return (true, jsonString);
}
```

### Models

- This is where all models are located. Models are used to define the structure of the data that is being sent and received from the client-side application. They are used to serialize and deserialize data.
- It's a good practice to make them as simple as possible, so you can easily understand what data is being sent and received.
- Also try to keep them as a struct, so you can easily pass them around without worrying about the accidental object mutation.

Example of Model:

```c#
[Serializable]
public struct EncryptAccountPayload
{
    public string Password { get; set; }
    public string AccountData { get; set; }
    public string PublicKey { get; set; }
}
```

## Solnet Library

Consider checking out the [Solnet](https://github.com/bmresearch/Solnet) library for more information on how to interact with Solana blockchain.
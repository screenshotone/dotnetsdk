# ScreenShotOne dotnetsdk

[![CI](https://github.com/theorigin/screenshotone-dotnet-sdk/actions/workflows/CI.yml/badge.svg?branch=main)](https://github.com/theorigin/screenshotone-dotnet-sdk/actions/workflows/CI.yml/badge.svg?branch=main)

An official [Screenshot API](https://screenshotone.com/) client for .NET. 

It takes minutes to start taking screenshots. Just [sign up](https://screenshotone.com/) to get access and secret keys, import the client, and you are ready to go. 

The SDK client is synchronized with the latest [screenshot API options](https://screenshotone.com/docs/options/).

## Installation

Add the library via nuget using the package manager console: 
```
PM> Install-Package ScreenshotOne.dotnetsdk
```
Or from the .NET CLI as:
```
dotnet add package ScreenshotOne.dotnetsdk
```


## Usage

Generate a screenshot URL without executing request: 
```c#
var client = new Client("_OzqMIjpCw-ARQ", "1ts-QfZmRVsxuA");	
var options = TakeOptions.Url("https://www.amazon.com")
  .FullPage(true)
  .Format(Format.PNG)
  .BlockCookieBanners(true);
  
var url = client.GenerateTakeUrl(options);

// url = https://api.screenshotone.com/take?url=https%3A%2F%2Fwww.amazon.com&full_page=true&format=png&block_cookie_banners=true&access_key=_OzqMIjpCw-ARQ&signature=8a08e62d13a5c3490fda0734b6707791d3decc9ab9ba41e8cc045288a39db502	

```

Take a screenshot and save the image in the file: 
```c#
var client = new Client("_OzqMIjpCw-ARQ", "1ts-QfZmRVsxuA");	
var options = TakeOptions.Url("https://www.google.com")
  .FullPage(true)
  .Format(Format.PNG)
  .BlockCookieBanners(true);
  
var bytes = await client.Take(options);

File.WriteAllBytes(@"c:\temp\example.png", bytes);	
```

## Building 

To build, execute:
```shell
dotnet build
```

## Tests 

To run tests, execute: 
```shell
dotnet test
```

## License 

`screenshotone/dotnetsdk` is released under [the MIT license](LICENSE).
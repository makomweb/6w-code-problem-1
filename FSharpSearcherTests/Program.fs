module FSharpSearcherTests

open System
open System.Linq
open TweetSearchFSharp
open Xunit
open FsUnit.Xunit

[<Fact>]
let when_creating_auth_string_then_should_succeed() = 
    basic_auth
    |> String.IsNullOrEmpty 
    |> Assert.False

[<Fact>]
let when_getting_bearer_token_then_should_succeed() =
    get_bearer_token
    |> Async.RunSynchronously
    |> String.IsNullOrEmpty
    |> Assert.False

[<Fact>]
let when_getting_tweets_for_6Wunderkinder_then_should_succeed() =
    get_tweets("6Wunderkinder")
    |> Async.RunSynchronously
    |> Enumerable.Any
    |> Assert.True

[<Fact>]
let when_getting_tweets_for_wunderkinder_then_should_succeed() =
    get_tweets("wunderkinder")
    |> Async.RunSynchronously
    |> Enumerable.Any
    |> Assert.True
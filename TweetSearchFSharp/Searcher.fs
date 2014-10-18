module TweetSearchFSharp

open System
open System.IO
open System.Text
open System.Net.Http
open System.Net.Http.Headers
open System.Linq
open Newtonsoft;

let key = "" // TODO enter your consumer key here!
let secret = "" // TODO enter your consumer secret here!

let create_basic_auth(key, secret) = 
    sprintf "%s:%s" key secret
    |> Encoding.UTF8.GetBytes
    |> Convert.ToBase64String

let basic_auth =
    (key, secret) |> create_basic_auth

type credentials = {
    access_token : string
}

let deserialize_credentials(raw) =
    Json.JsonConvert.DeserializeObject<credentials>(raw)
    
let get_bearer_token =
    async {
        let http = new HttpClient()
        http.DefaultRequestHeaders.Add("Authorization", "Basic " + basic_auth)
        let content = new System.Net.Http.StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded")
        let! response = http.PostAsync("https://api.twitter.com/oauth2/token", content) |> Async.AwaitTask
        response.EnsureSuccessStatusCode |> ignore
        let! s = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        let creds = deserialize_credentials(s)
        return creds.access_token
    }

type tweet = {
    text : string
}

type search_response = {
    statuses : tweet []
}

let deserialize_tweets(raw) =
    let resp = Json.JsonConvert.DeserializeObject<search_response>(raw)
    resp.statuses

let get_tweets(search_term) =
    async {
        let http = new HttpClient()
        let! bearer_token = get_bearer_token
        http.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearer_token)
        let! response = http.GetAsync("https://api.twitter.com/1.1/search/tweets.json?q=" + search_term + "&count=100") |> Async.AwaitTask
        response.EnsureSuccessStatusCode |> ignore
        let! s = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        let tweets = deserialize_tweets(s)
        return tweets
    }

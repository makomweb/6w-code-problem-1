open TweetSearchFSharp

let f arrayFunction array =
    array |> Seq.iter arrayFunction

let fetch_and_print =
    async {
        let! tweets = get_tweets("6Wunderkinder")
        tweets |> Seq.map(fun t -> printfn "%s" t.text)
        return 0
    }

[<EntryPoint>]
let main argv = 
    fetch_and_print |> Async.RunSynchronously
    0


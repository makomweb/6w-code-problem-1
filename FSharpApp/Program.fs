open TweetSearchFSharp

let f arrayFunction array =
    array |> Seq.iter arrayFunction

let fetch_and_print =
    async {
        let! tweets = get_tweets("6Wunderkinder")
        for x in tweets do
            printfn "%s" x.text
        return 0
    }

[<EntryPoint>]
let main argv = 
    fetch_and_print |> Async.RunSynchronously |> ignore
    0


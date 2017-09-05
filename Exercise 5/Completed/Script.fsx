//you'll need to install the FSharp Data nuget package
//check to make sure the version number matches the installed version of FSharp Data
#r "../packages/FSharp.Data.2.1.1/lib/net40/FSharp.Data.dll"
open FSharp.Data

let SFOFile = new CsvProvider<"./Data/SFO-Passenger-Data.csv">()
let SFOData = SFOFile.Rows |> Seq.toList

let (|December|_|) input = 
    if input.ToString().EndsWith("12") then Some(December)
    else None

let (|Deplaning|_|) input = 
    if input = "Deplaned" then Some(Deplaning)
    else None

let december, other = 
    SFOData 
     |> List.partition (fun x -> match x.``Activity Period`` with
                                 | December -> true
                                 | _ -> false)

let decemberDeplaned, other2 = 
    december
     |> List.partition (fun x -> match x.``Activity Type Code`` with
                                 | Deplaning -> true
                                 | _ -> false)

let minarrivals = decemberDeplaned |> List.minBy (fun x -> x.``Passenger Count``)

minarrivals.``Operating Airline``